namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    public class FlowChartWindowEditor : EditorWindow
    {
        public static void OpenWindow(FlowChart flowChart)
        {
            var window = GetWindow<FlowChartWindowEditor>();

            window.titleContent = new GUIContent("FlowChartEditor");
        }
    }

}