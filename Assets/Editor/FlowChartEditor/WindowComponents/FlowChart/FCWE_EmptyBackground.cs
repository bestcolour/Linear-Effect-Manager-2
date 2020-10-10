namespace LinearEffectsEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    //Repsonsible for displaying empty flowchart editor
    public partial class FlowChartWindowEditor : EditorWindow
    {
        void EmptyBackground_OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField(string.Empty);
            Rect rect = GUILayoutUtility.GetLastRect();
            _flowChart = (FlowChart)EditorGUI.ObjectField(rect, "Target FlowChart", _flowChart, typeof(FlowChart), true);

            if (_flowChart != null)
            {
                REINITIALIZE();
            }

            EditorGUILayout.EndVertical();

        }
    }

}