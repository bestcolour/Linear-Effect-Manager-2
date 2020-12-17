﻿namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    //Repsonsible for displaying empty flowchart editor
    public partial class FlowChartWindowEditor : EditorWindow
    {
        void UnloadedBackground_OnGUI()
        {
            EditorGUILayout.BeginVertical();EditorGUILayout.LabelField(string.Empty);EditorGUILayout.EndVertical();
            Rect rect = GUILayoutUtility.GetLastRect();
            _flowChart = (FlowChart)EditorGUI.ObjectField(rect, "Target FlowChart", _flowChart, typeof(FlowChart), true);
            if (_flowChart != null)
            {
                REINITIALIZE();
            }
        }
    }

}