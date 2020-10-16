namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using LinearEffects;
    using System.Collections.Generic;
    using System;

    //The top half class will render the settings & command list
    public partial class BlockInspector : Editor
    {

        #region Cached Variable

        ReorderableList _list = default;
        SerializedProperty _settingsProperty = default;
        Vector2 _scrollPosition = default;
        HashSet<int> _selectedElements = default;
        int _firstClickedIndex = -1;
        #endregion

        #region LifeTime Methods

        void TopHalf_OnEnable()
        {
            SerializedProperty orderArray = serializedObject.FindProperty(BlockScriptableInstance.PROPERTYPATH_ORDERARRAY);
            _settingsProperty = serializedObject.FindProperty(BlockScriptableInstance.PROPERTYPATH_SETTINGS);
            _list = new ReorderableList(serializedObject, orderArray, displayAddButton: false, displayHeader: true, displayRemoveButton: false, draggable: true);
            _selectedElements = new HashSet<int>();
            _firstClickedIndex = -1;
            // _hasSelectedElement = false;

            _list.drawHeaderCallback = TopHalf_HandleDrawHeaderCallBack;
            _list.drawElementCallback = TopHalf_HandleDrawElementCallBack;
            _list.elementHeightCallback += TopHalf_HandleElementHeightCallBack;
            _list.onChangedCallback += TopHalf_HandleOnChange;
            _list.onSelectCallback += TopHalf_HandleOnSelect;
            _list.onMouseDragCallback += TopHalf_HandleDrag;
        }



        void TopHalf_OnDisable()
        {
            _list.elementHeightCallback -= TopHalf_HandleElementHeightCallBack;
            _list.onChangedCallback -= TopHalf_HandleOnChange;
            _list.onSelectCallback -= TopHalf_HandleOnSelect;
            _list.onMouseDragCallback -= TopHalf_HandleDrag;

            _list = null;
        }


        //Calll the reorderable list to update itself
        void TopHalf_OnInspectorGUI(Vector2 windowSize)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(windowSize.x), GUILayout.Height(windowSize.y));
            EditorGUILayout.Space();
            TopHalf_DrawSettings();
            EditorGUILayout.Space(20f);
            TopHalf_DrawReOrderableList();
            EditorGUILayout.EndScrollView();
        }

        #endregion


        #region Event Handlers
        private void TopHalf_HandleDrag(ReorderableList list)
        {
            //Drag all the selected elements by communicating 
            //DECIDE IF U WANNA CODE MUTLIPLE SELECTION AND HENCE DRAGGING
            // Debug.Log($"Dragging {list.index}");
        }

        private void TopHalf_HandleOnSelect(ReorderableList list)
        {
            // _hasSelectedElement = true;
            int clickedIndex = list.index;

            //====================== SHIFT CLICK =========================
            if (Event.current.shift && _firstClickedIndex != -1)
            {
                _selectedElements.Clear();

                int diff = clickedIndex - _firstClickedIndex;

                //If its positive it will move downards
                int direction = diff > 0 ? 1 : -1;
                diff = Mathf.Abs(diff);

                for (int i = 0; i <= diff; i++)
                {
                    _selectedElements.Add(_firstClickedIndex + i * direction);
                }

                return;
            }

            //================= NO SHIFT CLICK ======================
            _selectedElements.Clear();
            _selectedElements.Add(clickedIndex);
            _firstClickedIndex = clickedIndex;
        }

        private void TopHalf_HandleDrawHeaderCallBack(Rect rect)
        {
            EditorGUI.LabelField(rect, "Effect List");
        }

        private float TopHalf_HandleElementHeightCallBack(int index)
        {
            return EditorGUIUtility.singleLineHeight * 2f;
        }

        private void TopHalf_HandleDrawElementCallBack(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty orderElement = _list.serializedProperty.GetArrayElementAtIndex(index);

            //<================ DRAWING MAIN BG =========================>
            //Draw a bg for the entire list rect before we start modifying the rect
            Color colourOfBg = _selectedElements.Contains(index) ? Color.green : Color.blue;
            Color prevBgColour = GUIExtensions.Start_GUIBg_ColourChange(colourOfBg);
            GUI.Box(rect, string.Empty);
            GUIExtensions.End_GUIBg_ColourChange(prevBgColour);

            //<================ DRAWING COMMAND TYPE=========================>
            if (!EditorDebugExtension.TryGetProperty(orderElement, Block.EffectOrder.PROPERTYNAME_EFFECTNAME, out SerializedProperty dummyProperty)) return;

            //By calculating the size of the content, i can ensure that the error log is always rendered 10 units past the commadntype
            GUIContent content = new GUIContent(dummyProperty.stringValue);
            var style = EditorStyles.label;

            Vector2 sizeOfContent = style.CalcSize(content);

            //Modify rect
            //Shift rect 10units to avoid overlapping the stroke bullet points
            rect.width = sizeOfContent.x;
            rect.height = sizeOfContent.y;
            rect.x += 10;
            rect.y += 10;

            //Draw the Type of Command first
            EditorGUI.LabelField(rect, dummyProperty.stringValue);


            //For now this is unneeded
            // //<================ DRAWING ERROR LOG =========================>
            // //Modify rect again
            // //Shift rect 10 units to give space between errorlog and type of command
            // rect.x += rect.width + 10;
            // rect.y -= 5;

            // if (!EditorDebugExtension.TryGetProperty(orderElement, Block.EffectOrder.PROPERTYNAME_ERRORLOG, out dummyProperty)) return;

            // Color pastLabelColour = GUIExtensions.Start_StyleText_ColourChange(Color.red, EditorStyles.label);
            // style.fontStyle = FontStyle.Italic;
            // //Draw Errorlog
            // EditorGUI.LabelField(rect, dummyProperty.stringValue);
            // style.fontStyle = FontStyle.Normal;
            // GUIExtensions.End_StyleText_ColourChange(pastLabelColour, EditorStyles.label);
        }

        private void TopHalf_HandleOnChange(ReorderableList list)
        {
            //Call the recalibration of effect order here in the block
            _target.SaveModifiedProperties();
        }

        #endregion

        #region  Draw Methods
        void TopHalf_DrawSettings()
        {
            if (_settingsProperty == null)
            {
                string debug = $"The property named: {BlockScriptableInstance.PROPERTYPATH_SETTINGS} inside the Block class has been renamed to something else or it doesnt exist anymore!";
                Debug.LogWarning(debug);
                return;
            }

            EditorGUILayout.PropertyField(_settingsProperty, includeChildren: true);
        }

        void TopHalf_DrawReOrderableList()
        {
            _list.DoLayoutList();
        }
        #endregion

// #region Resets
// void TopHalf_ResetSelectedElements()
// {
//     _selectedElements.Clear();
// }
// #endregion

    }

}