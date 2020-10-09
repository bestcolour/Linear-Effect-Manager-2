namespace LinearEffectsEditor
{
    using System;
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


        #region  Properties
        Vector2 CenterScreen => new Vector2(Screen.width, Screen.height) * 0.35f;
        #endregion


        #region Unity LifeTime
        public static void OpenWindow(FlowChart flowChart)
        {
            var window = GetWindow<FlowChartWindowEditor>();

            window.titleContent = new GUIContent("FlowChartEditor");

            _target = flowChart;
            window.ProxyOnEnable();
        }

        void ProxyOnEnable()
        {
            _onGUI = General_Init_OnGUI;
            Background_OnEnable();
            NodeManager_OnEnable();
            ToolBar_OnEnable();
            ProcessEvent_OnEnable();
            BlockEditor_OnEnable();
        }



        void OnDisable()
        {
            _onGUI = null;
            Background_OnDisable();
            NodeManager_OnDisable();
            ToolBar_OnDisable();
            ProcessEvent_OnDisable();
            BlockEditor_OnDisable();
        }

        void OnGUI()
        {
            _onGUI?.Invoke();
        }



        #endregion

        #region GUI Call
        void General_Update_OnGUI()
        {
            //=========== DRAW ORDER===============
            Background_OnGUI();
            ToolBar_OnGUI();


            NodeManager_OnGUI();
            ProcessEvent_OnGUI();


        }

        void General_Init_OnGUI()
        {
            //Initialize code
            //init styles here

            //End of Init
            _onGUI = General_Update_OnGUI;
        }



        #endregion




    }
}