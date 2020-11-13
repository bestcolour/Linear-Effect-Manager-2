namespace CategorizedSearchBox
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.IMGUI.Controls;

    public class CategorizedSearchBox : SearchField
    {
        #region SearchBar Fields
        Rect _barRect = default;


        string _searchedBarText = String.Empty;

        //======= EVENTS ========
        event Action OnSearchBarTextChange = null;

        #endregion

        #region SearchBox Fields
        Rect _boxRect = default;
        Vector2 _scrollPosition = default;

        //======== RESULTS ==========
        int _maxNumberOfResults = default
       , _numOfResultsFound = default
        ;

        List<string> _library, _results;
        HashSet<string> _drawnCategories = default;


        public bool IsInSearchBox => _boxRect.Contains(Event.current.mousePosition, true);

        #region Constants
        const string CATEGORY_IDENTIFIER = "/", CATEGORY_ARROWSYMBOL = "＞ ";
        const float BAR_CANCELICON_WIDTH = 17.5f
        ;

        static GUIStyle STYLE_RESULTS_EVEN = default
        , STYLE_RESULTS_ODD = default
        ;
        #endregion

        #endregion
        public void Initialize(string[] resultsToPopulate)
        {
            STYLE_RESULTS_EVEN = new GUIStyle("CN EntryBackOdd");
            STYLE_RESULTS_ODD = new GUIStyle("CN EntryBackEven");
            _library = new List<string>();
            _drawnCategories = new HashSet<string>();
            _library.AddRange(resultsToPopulate);
            HandleSearchBarTextChange();
        }

        #region Enables & Disables
        public virtual void EnableSearchBox()
        {
            OnSearchBarTextChange += HandleSearchBarTextChange;
        }



        public virtual void EnableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
        {
            EnableSearchBox();
            downOrUpArrowKeyPressed += handleDownOrUpArrowKeyPressed;
        }

        public virtual void EnableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed, Action handleSearchBarTextChange)
        {
            EnableSearchBox(handleDownOrUpArrowKeyPressed);
            OnSearchBarTextChange += handleSearchBarTextChange;
        }

        public virtual void DisableSearchBox()
        {
            OnSearchBarTextChange -= HandleSearchBarTextChange;
        }

        public virtual void DisableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
        {
            DisableSearchBox();
            downOrUpArrowKeyPressed -= handleDownOrUpArrowKeyPressed;
        }

        public virtual void DisableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed, Action handleSearchBarChange)
        {
            DisableSearchBox(handleDownOrUpArrowKeyPressed);
            OnSearchBarTextChange -= handleSearchBarChange;
        }


        #endregion

        ///<Summary>
        ///Returns the height that the entire searchbox will need to occupy
        ///</Summary>
        public virtual float Handle_OnGUI(Rect searchBarRect, float searchBoxHeight)
        {
            //========= SEARCHBAR ==============
            SearchBar_OnGUI(searchBarRect);

            //=========== SEARCHBOX ===============
            //Substract the cross icon's width
            SearchBox_OnGUI(searchBoxHeight);


            return _barRect.height + _boxRect.height;
        }


        #region Search Bar Methods
        protected void SearchBar_OnGUI(Rect rect)
        {
            //====== UPDATE BAR RECT =========
            _barRect = rect;

            string newSearchBarText = OnGUI(_barRect, _searchedBarText);

            if (newSearchBarText == _searchedBarText)
            {
                return;
            }

            _searchedBarText = newSearchBarText;
            OnSearchBarTextChange?.Invoke();

        }


        #endregion

        #region Search Box Methods
        void SearchBox_OnGUI(float searchBoxHeight)
        {
            SearchBox_UpdateValues(searchBoxHeight);
            SearchBox_HandleResults();
        }

        void SearchBox_UpdateValues(float searchBoxHeight)
        {
            //must occur after _barRect updates itself
            //====== UPDATE BOX RECT =========
            _boxRect.x = _barRect.x;
            _boxRect.y = _barRect.y + _barRect.height;
            _boxRect.width = _barRect.width - BAR_CANCELICON_WIDTH;
            _boxRect.height = searchBoxHeight;


            _maxNumberOfResults = Mathf.FloorToInt(_boxRect.height / EditorGUIUtility.singleLineHeight);
        }

        void SearchBox_HandleResults()
        {

            // ============ DRAW A SPACE ===============
            EditorGUILayout.Space(_barRect.y + _barRect.height);

            // ============ SCROLL VIEW ================
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.MinWidth(_boxRect.width), GUILayout.MinHeight(_boxRect.height));

            for (int i = 0; i < _results.Count; i++)
            {
                string result = _results[i];
                GUIStyle resultStyle = i % 2 == 0 ? STYLE_RESULTS_EVEN : STYLE_RESULTS_ODD;

                if (!TryGetCategory(result, out string category))
                {
                    EditorGUILayout.LabelField(result, resultStyle);
                    continue;
                }

                // if (_drawnCategories.Contains(category))
                //     continue;


                //=== DRAWING CATERGORY
                EditorGUILayout.LabelField(CATEGORY_ARROWSYMBOL + category, resultStyle);
                _drawnCategories.Add(category);

            }

            GUILayout.EndScrollView();

        }

        void HandleSearchBarTextChange()
        {
            if (_searchedBarText == string.Empty)
            {
                _results = _library;
                return;
            }

            _results = _library.FindAll(SearchBarSearchPredicate);
        }

        bool SearchBarSearchPredicate(string s)
        {
            if (s.Contains(_searchedBarText, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        bool TryGetCategory(string s, out string catergory)
        {
            int slashChar = s.IndexOf(CATEGORY_IDENTIFIER);

            catergory = string.Empty;

            if (slashChar == -1) return false;

            catergory = s.Substring(0, slashChar);
            return true;
        }

        #endregion



    }

}