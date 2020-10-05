namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Current Cached Variable
        static FlowChart _target = default;


        #endregion

        #region Events

        static System.Action _onGUI = null;

        #endregion

        #region  LifeTime
        public static void OpenWindow(FlowChart flowChart)
        {
            var window = GetWindow<FlowChartWindowEditor>();

            window.titleContent = new GUIContent("FlowChartEditor");

            _target = flowChart;
        }

        void OnEnable()
        {
            _onGUI = General_Init_OnGUI;
        }

        void OnDisable()
        {
            _onGUI = null;
        }

        void OnGUI()
        {
            _onGUI?.Invoke();
        }

        #endregion

        void General_Update_OnGUI()
        {
            Background_Draw();
        }

        void General_Init_OnGUI()
        {
            //Initialize code

            _onGUI = General_Update_OnGUI;
        }

     

    }

}