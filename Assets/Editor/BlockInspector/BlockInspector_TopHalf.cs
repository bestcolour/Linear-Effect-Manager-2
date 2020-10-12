﻿namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using LinearEffects;

    //The top half class will render the settings & command list
    public partial class BlockInspector : Editor
    {

        #region Cached Variable

        ReorderableList _list = default;
        SerializedProperty _settingsProperty = default;
        Vector2 _scrollPosition = default;

        #endregion

        #region LifeTime Methods

        void TopHalf_OnEnable()
        {
            SerializedProperty orderArray = serializedObject.FindProperty(BlockScriptableInstance.PROPERTYPATH_ORDERARRAY);
            _settingsProperty = serializedObject.FindProperty(BlockScriptableInstance.PROPERTYPATH_SETTINGS);
            _list = new ReorderableList(serializedObject, orderArray, displayAddButton: false, displayHeader: true, displayRemoveButton: false, draggable: true);


            _list.drawHeaderCallback = HandleDrawHeaderCallBack;
            _list.drawElementCallback = HandleDrawElementCallBack;
            _list.elementHeightCallback += HandleElementHeightCallBack;
            _list.onChangedCallback += HandleOnChange;

        }

        void TopHalf_OnDisable()
        {
            _list.elementHeightCallback -= HandleElementHeightCallBack;
            _list.onChangedCallback -= HandleOnChange;

            _list = null;
        }


        //Calll the reorderable list to update itself
        void TopHalf_OnInspectorGUI(Vector2 windowSize)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(windowSize.x), GUILayout.Height(windowSize.y));
            EditorGUILayout.Space();
            DrawSettings();
            EditorGUILayout.Space(20f);
            DrawReOrderableList();
            EditorGUILayout.EndScrollView();
        }

        #endregion


        #region Event Handlers
        private void HandleDrawHeaderCallBack(Rect rect)
        {
            EditorGUI.LabelField(rect, "Effect List");
        }

        private float HandleElementHeightCallBack(int index)
        {
            return EditorGUIUtility.singleLineHeight * 2f;
        }

        private void HandleDrawElementCallBack(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty orderElement = _list.serializedProperty.GetArrayElementAtIndex(index);


            //<================ DRAWING MAIN BG =========================>
            //Draw a bg for the entire list rect before we start modifying the rect
            Color colourOfBg = isActive ? Color.green : Color.blue;
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


            //<================ DRAWING ERROR LOG =========================>
            //Modify rect again
            //Shift rect 10 units to give space between errorlog and type of command
            rect.x += rect.width + 10;
            rect.y -= 5;

            if (!EditorDebugExtension.TryGetProperty(orderElement, Block.EffectOrder.PROPERTYNAME_ERRORLOG, out dummyProperty)) return;

            Color pastLabelColour = GUIExtensions.Start_StyleText_ColourChange(Color.red, EditorStyles.label);
            style.fontStyle = FontStyle.Italic;
            //Draw Errorlog
            EditorGUI.LabelField(rect, dummyProperty.stringValue);
            style.fontStyle = FontStyle.Normal;
            GUIExtensions.End_StyleText_ColourChange(pastLabelColour, EditorStyles.label);
        }

        private void HandleOnChange(ReorderableList list)
        {
            //Call the recalibration of effect order here in the block
        }

        #endregion

        #region  Draw Methods
        void DrawSettings()
        {
            if (_settingsProperty == null)
            {
                string debug = $"The property named: {BlockScriptableInstance.PROPERTYPATH_SETTINGS} inside the Block class has been renamed to something else or it doesnt exist anymore!";
                Debug.LogWarning(debug);
                return;
            }

            EditorGUILayout.PropertyField(_settingsProperty, includeChildren: true);
        }

        void DrawReOrderableList()
        {
            _list.DoLayoutList();
        }
        #endregion

    }

}