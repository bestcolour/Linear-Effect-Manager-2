namespace LinearEffectsEditor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    [CustomEditor(typeof(FlowChart))]
    public class FlowChartEditor : Editor
    {
        const string SETTINGS_PROPERTY_NAME = "_settings";


        #region  Runtime Vars
        FlowChart _target = default;
        SerializedProperty _settingsProperty = default;
        #endregion

        private void OnEnable()
        {
            _target = (FlowChart)target;
            _settingsProperty = serializedObject.FindProperty(SETTINGS_PROPERTY_NAME);
        }

        private void OnDisable()
        {
            _target = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.BeginVertical();
            DrawProperties();
            DrawButtons();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        #region Draw 
        void DrawButtons()
        {
            if (GUILayout.Button("Open FlowChart"))
            {
                FlowChartWindowEditor.OpenWindow(_target);
            }
        }


        void DrawProperties()
        {
            if (_settingsProperty == null)
            {
                string debug = $"The property named: {SETTINGS_PROPERTY_NAME} inside the Block class has been renamed to something else or it doesnt exist anymore!";
                Debug.LogWarning(debug);
                return;
            }

            EditorGUILayout.PropertyField(_settingsProperty, includeChildren: true);
        }

        #endregion


    }

}