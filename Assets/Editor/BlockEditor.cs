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
        const string SETTINGS_PROPERTY_NAME = "_settings";


        ReorderableList _list = default;
        SerializedProperty _commandLabelsProperty = default;
        SerializedProperty _settingsProperty = default;



        private void OnEnable()
        {
            _commandLabelsProperty = serializedObject.FindProperty("_commandLabels");
            _settingsProperty = serializedObject.FindProperty(SETTINGS_PROPERTY_NAME);

            _list = new ReorderableList(serializedObject, _commandLabelsProperty, displayAddButton: true, displayHeader: true, displayRemoveButton: true, draggable: true);


            _list.drawHeaderCallback = DrawHeaderCallBack;
            // _list.drawElementCallback = DrawElementCallBack;
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

        // private void DrawElementCallBack(Rect rect, int index, bool isActive, bool isFocused)
        // {

        // }


        #endregion



        //Calll the reorderable list to update itself
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDebugButton();

            DrawSettings();

            DrawReOrderableList();

            serializedObject.ApplyModifiedProperties();
        }


        #region  Draw Methods
        void DrawSettings()
        {
            if (_settingsProperty == null)
            {
                StringBuilder debug = new StringBuilder
                                        (
                                            string.Format
                                            (
                                                "The property named: {0} inside the Block class has been renamed to something else or it doesnt exist anymore!", SETTINGS_PROPERTY_NAME
                                            )
                                        );
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
        #endregion



    }
}