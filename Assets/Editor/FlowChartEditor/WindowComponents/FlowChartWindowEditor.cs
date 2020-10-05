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

        #region Definition
        delegate void UpdateGUICallback();
        #endregion


        #region Events
        static UpdateGUICallback _onGUI = null;
        #endregion

        #region Unity LifeTime
        public static void OpenWindow(FlowChart flowChart)
        {
            var window = GetWindow<FlowChartWindowEditor>();

            window.titleContent = new GUIContent("FlowChartEditor");

            _target = flowChart;
        }

        void OnEnable()
        {
            _onGUI = General_Init_OnGUI;
            ProcessEvent_OnEnable();
        }

        void OnDisable()
        {
            _onGUI = null;
            ProcessEvent_OnDisable();
        }

        void OnGUI()
        {
            _onGUI?.Invoke();
        }

        #endregion

        #region GUI Call
        void General_Update_OnGUI()
        {
            ProcessEvents();

            //=========== DRAW ORDER===============
            Background_Draw();
        }

        void General_Init_OnGUI()
        {
            //Initialize code

            //End of Init
            _onGUI = General_Update_OnGUI;
        }

      

        #endregion




    }
}