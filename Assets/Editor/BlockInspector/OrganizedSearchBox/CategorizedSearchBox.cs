namespace CategorizedSearchBox
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.IMGUI.Controls;

    public partial class CategorizedSearchBox
    {
        #region SearchBar Fields
        SearchField _searchBar = default;
        Rect _searchBarRect = default;


        string _searchedBarText = String.Empty;

        //======= EVENTS ========
        event SearchBarTextChangeCallback OnSearchBarTextChange = null;
        event OnPressConfirmCallback OnPressConfirm = null;
        event UpOrDownArrowPressedCallback OnUpOrDownArrowPressed = null;

        #endregion

        #region ResultBox Fields
        Rect _resultBoxRect = default;
        Vector2 _scrollPosition = default;

        List<string> _library, _results;
        HashSet<string> _drawnCategories = default;
        int _currentlySelectedResult = -1;


        public bool IsInResultBox => _resultBoxRect.Contains(Event.current.mousePosition, true);

        #region Constants
        const string CATEGORY_IDENTIFIER = "/", CATEGORY_ARROWSYMBOL = "＞ ";
        const float BAR_CANCELICON_WIDTH = 17.5f
        ;

        static GUIStyle STYLE_RESULTS_EVEN = default
        , STYLE_RESULTS_ODD = default
        , STYLE_RESULTS_SELECTED = default
        ;
        #endregion

        #endregion

        ///<Summary>
        ///Returns the height that the entire searchbox will need to occupy
        ///</Summary>
        public virtual float Handle_OnGUI(Rect searchBarRect, float resultBoxHeight)
        {
            //========= SEARCHBAR ==============
            SearchBar_OnGUI(searchBarRect);

            //=========== RESULTBOX ===============
            //Substract the cross icon's width
            ResultBox_OnGUI(resultBoxHeight);


            return _searchBarRect.height + _resultBoxRect.height;
        }


        #region Search Bar Methods
        protected void SearchBar_OnGUI(Rect rect)
        {
            //====== UPDATE BAR RECT =========
            _searchBarRect = rect;

            //====== UPDATE THE SEARCH BAR TEXT ============
            string newSearchBarText = _searchBar.OnGUI(_searchBarRect, _searchedBarText);

            if (newSearchBarText == _searchedBarText)
            {
                return;
            }

            _searchedBarText = newSearchBarText;
            RaiseSearchBarTextChange(_searchedBarText);
        }


        #endregion

        #region Result Box Methods
        void ResultBox_OnGUI(float searchBoxHeight)
        {
            ResultBox_UpdateValues(searchBoxHeight);
            ResultBox_DrawResults();
            ResultBox_ClearValues();
        }

        void ResultBox_UpdateValues(float searchBoxHeight)
        {
            //must occur after _barRect updates itself
            //====== UPDATE BOX RECT =========
            _resultBoxRect.x = _searchBarRect.x;
            _resultBoxRect.y = _searchBarRect.y + _searchBarRect.height;
            _resultBoxRect.width = _searchBarRect.width - BAR_CANCELICON_WIDTH;
            _resultBoxRect.height = searchBoxHeight;


            // _maxNumberOfResults = Mathf.FloorToInt(_boxRect.height / EditorGUIUtility.singleLineHeight);
        }

        private void ResultBox_ClearValues()
        {
            _drawnCategories.Clear();
        }

        void ResultBox_DrawResults()
        {

            // ============ DRAW A SPACE ===============
            EditorGUILayout.Space(_searchBarRect.y + _searchBarRect.height);

            // ============ SCROLL VIEW ================
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.MinWidth(_resultBoxRect.width), GUILayout.MinHeight(_resultBoxRect.height));

            for (int i = 0; i < _results.Count; i++)
            {
                string result = _results[i];
                GUIStyle resultStyle = GetResultStyle(i);

                //If no category was found, just draw the result as it is
                if (!TryGetCategory(result, out string category))
                {
                    EditorGUILayout.LabelField(result, resultStyle);
                    ProcessResult(result);
                    continue;
                }

                //If the result is a category but it was already drawn, skip
                if (_drawnCategories.Contains(category))
                    continue;


                //=== DRAWING CATERGORY =====
                EditorGUILayout.LabelField(CATEGORY_ARROWSYMBOL + category, resultStyle);
                _drawnCategories.Add(category);
                ProcessResult(category);
            }

            GUILayout.EndScrollView();

        }

        GUIStyle GetResultStyle(int resultIndex)
        {
            if (resultIndex == _currentlySelectedResult) return STYLE_RESULTS_SELECTED;

            return resultIndex % 2 == 0 ? STYLE_RESULTS_EVEN : STYLE_RESULTS_ODD;
        }

        bool TryGetCategory(string s, out string catergory)
        {
            int slashChar = s.IndexOf(CATEGORY_IDENTIFIER);

            catergory = string.Empty;

            if (slashChar == -1) return false;

            catergory = s.Substring(0, slashChar);
            return true;
        }

        //Must only be called after the EditorGUILayout field is drawn
        void ProcessResult(string resultName)
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            Event e = Event.current;

            switch (e.type)
            {
                //==============MOUSE UP EVENT ================
                case EventType.MouseUp:
                    if (rect.Contains(e.mousePosition, true))
                    {
                        RaiseOnConfirm(resultName);
                    }
                    break;


                //============== KEY DOWN EVENT ================
                case EventType.KeyDown:
                    switch (e.keyCode)
                    {
                        case KeyCode.UpArrow:
                            RaiseDownOrUpArrowKeyPressed(true);
                            break;

                        case KeyCode.DownArrow:
                            RaiseDownOrUpArrowKeyPressed(false);
                            break;

                        //Else do nth
                        default: break;
                    }
                    break;


                //Else do nth
                default: break;

            }


        }

        #endregion



        #region Handle  Events
        //======= RAISE SEARCH BAR TEXT CHANGE ==========
        void RaiseSearchBarTextChange(string newSearchBarText)
        {
            _currentlySelectedResult = -1;

            //ensure that if there is nth in the searchbar, at least show all the results
            _results = newSearchBarText == string.Empty ? _results = _library : _library.FindAll(SearchBarSearchPredicate);

            OnSearchBarTextChange?.Invoke(newSearchBarText);
            Event.current?.Use();
        }

        bool SearchBarSearchPredicate(string s)
        {
            if (s.Contains(_searchedBarText, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }


        // ============= RAISE CONFIRM ==============
        //Handles when search bar has "enter" pressed down (when a result has been highlighted using the up & down arrow keys) or when a result has been pressed down
        //resultname can be: category name, result name
        void RaiseOnConfirm(string resultName)
        {
            Debug.Log($"Result name is {resultName}");
            //Check if the confirmed result is a category. if so, append that category name to the searchbar text
            if (_drawnCategories.Contains(resultName))
            {
                //Add only when the text doesnt already have the result name
                if (_searchedBarText.Contains(resultName))
                {
                    return;
                }

                _searchedBarText = _searchedBarText == string.Empty ? _searchedBarText + resultName : _searchedBarText + "/" + resultName;
                return;
            }

            //Else, invoke the onconfirm event
            OnPressConfirm?.Invoke(_searchedBarText);
            Event.current.Use();
        }


        //============= RAISE DOWN OR UP ARROW PRESSED ==============
        private void RaiseDownOrUpArrowKeyPressed(bool upArrowKeyWasPressed)
        {
            int addition = upArrowKeyWasPressed ? -1 : 1;
            _currentlySelectedResult = Mathf.Clamp(_currentlySelectedResult + addition, -1, _results.Count);
            OnUpOrDownArrowPressed?.Invoke(upArrowKeyWasPressed);
            Event.current.Use();
        }

        #endregion
    }

}