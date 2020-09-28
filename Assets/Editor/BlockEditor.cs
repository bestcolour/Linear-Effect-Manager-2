using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinearCommandsEditor
{
    using UnityEditor;
    using UnityEditorInternal;
    using LinearCommands;
    using System.Text;

    [CustomEditor(typeof(Block))]
    public class BlockEditor : Editor
    {
        #region CONSTANT VALUES
        const string SETTINGS_PROPERTY = "_settings",
        COMMANDLABEL_TYPE_PROPERTY = "_commandType",
        COMMANDLABEL_ERRORLOG_PROPERTY = "_errorLog"
        ;
        #endregion


        ReorderableList _list = default;
        SerializedProperty _commandLabelsProperty = default;
        SerializedProperty _settingsProperty = default;



        private void OnEnable()
        {
            _commandLabelsProperty = serializedObject.FindProperty("_commandLabels");
            _settingsProperty = serializedObject.FindProperty(SETTINGS_PROPERTY);

            _list = new ReorderableList(serializedObject, _commandLabelsProperty, displayAddButton: true, displayHeader: true, displayRemoveButton: true, draggable: true);


            _list.drawHeaderCallback = DrawHeaderCallBack;
            _list.drawElementCallback = DrawElementCallBack;
            // _list.elementHeightCallback += ElementHeightCallBack;



        }


        private void OnDisable()
        {
            _commandLabelsProperty = null;
            _list = null;
        }



        #region Event Handlers
        private void DrawHeaderCallBack(Rect rect)
        {
            EditorGUI.LabelField(rect, "Command List");
        }


        // private float ElementHeightCallBack(int index)
        // {

        // }

        private void DrawElementCallBack(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty currentElement = _list.serializedProperty.GetArrayElementAtIndex(index);


            //<================ DRAWING MAIN BG =========================>
            //Draw a bg for the entire list rect before we start modifying the rect
            Color colourOfBg = isActive ? Color.green : Color.blue;
            Color prevBgColour = GUIExtensions.Start_GUIBg_ColourChange(colourOfBg);
            GUI.Box(rect, string.Empty);
            GUIExtensions.End_GUIBg_ColourChange(prevBgColour);

            //<================ DRAWING COMMAND TYPE=========================>
            if (!TryGetProperty(currentElement, COMMANDLABEL_TYPE_PROPERTY, out SerializedProperty dummyProperty)) return;

            //By calculating the size of the content, i can ensure that the error log is always rendered 10 units past the commadntype
            GUIContent content = new GUIContent(dummyProperty.stringValue);
            var style = GUI.skin.label;
            Vector2 sizeOfContent = style.CalcSize(content);

            //Modify rect
            //Shift rect 10units to avoid overlapping the stroke bullet points
            rect.width = sizeOfContent.x;
            rect.height = sizeOfContent.y;
            rect.x += 10;

            //Draw the Type of Command first
            EditorGUI.LabelField(rect, dummyProperty.stringValue);


            //<================ DRAWING ERROR LOG =========================>
            //Modify rect again
            //Shift rect 10 units to give space between errorlog and type of command
            rect.x += rect.width + 10;

            if (!TryGetProperty(currentElement, COMMANDLABEL_ERRORLOG_PROPERTY, out dummyProperty)) return;

            Color pastLabelColour = GUIExtensions.Start_StyleText_ColourChange(Color.red, EditorStyles.label);
            //Draw Errorlog
            EditorGUI.LabelField(rect, dummyProperty.stringValue);
            GUIExtensions.End_StyleText_ColourChange(pastLabelColour, EditorStyles.label);
        }


        #endregion



        //Calll the reorderable list to update itself
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // DrawDebugButton();
            EditorGUILayout.Space();
            DrawSettings();
            EditorGUILayout.Space(20f);
            DrawReOrderableList();

            serializedObject.ApplyModifiedProperties();
        }


        #region  Draw Methods
        void DrawSettings()
        {
            if (_settingsProperty == null)
            {
                string debug = $"The property named: {SETTINGS_PROPERTY} inside the Block class has been renamed to something else or it doesnt exist anymore!";
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



        #region  Debug
        //Just a debug method
        void PrintAllProperties()
        {
            SerializedProperty firstP = serializedObject.GetIterator();
            while (firstP.NextVisible(true))
            {
                Debug.Log(firstP.name);
            };
        }


        void DrawDebugButton()
        {
            if (GUILayout.Button("Debug"))
            {
                PrintAllProperties();
            }
        }

        bool TryGetProperty(SerializedProperty p, string propertyName, out SerializedProperty propertyFound)
        {
            propertyFound = p.FindPropertyRelative(propertyName);
            if (propertyFound != null)
            {
                return true;
            }

            Debug.LogWarning($"The property named: {propertyName} inside the Block class has been renamed to something else or it doesnt exist anymore!");
            return false;
        }
        #endregion



    }
}