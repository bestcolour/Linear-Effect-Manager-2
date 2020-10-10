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
        // static FlowChart _target = default;
        static SerializedObject _target = default;
        static FlowChart _flowChart = default;
        static int _state;

        #endregion



        #region Constants
        struct FLOWCHART_EDITOR_STATES
        {
            public const int INITIALIZATION = -1, UNLOADED = 0, LOADED = 1, RUNTIME_DEBUG = 2;
        }
        #endregion

        #region Events
        // static UpdateGUICallback _onGUI = null;
        static Action _onEnterPlayMode = null;
        #endregion


        #region  Properties
        Vector2 CenterScreen => new Vector2(Screen.width, Screen.height) * 0.35f;
        #endregion


        #region Unity LifeTime
        [MenuItem(itemName: "Window/FlowChart Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<FlowChartWindowEditor>();
            window.titleContent = new GUIContent("FlowChartEditor");
        }


        public static void OpenWindow(FlowChart flowChart)
        {
            _flowChart = flowChart;
            var window = GetWindow<FlowChartWindowEditor>();
            window.titleContent = new GUIContent("FlowChartEditor");
        }

        private void OnEnable()
        {
            _state = FLOWCHART_EDITOR_STATES.INITIALIZATION;
        }

        //I dont put the code in OnEnable because if i want to intialize styles, i have to do in during OnGUI calls
        void Initialize()
        {
            //============================== WINDOW OPEN VIA BUTTON PRESS ==============================
            if (_flowChart != null)
            {
                _state = FLOWCHART_EDITOR_STATES.LOADED;
                LOADED_OnEnable();
                return;
            }

            //======================= WINDOW OPEN VIA PLAYMODE CHANGE ===========================
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _state = FLOWCHART_EDITOR_STATES.RUNTIME_DEBUG;
                RUNTIME_DEBUG_OnEnable();
                return;
            }

            //======================= WINDOW OPEN VIA MENU ===========================
            _state = FLOWCHART_EDITOR_STATES.UNLOADED;
            UNLOADED_OnEnable();
        }

        void OnDisable()
        {
            switch (_state)
            {
                case FLOWCHART_EDITOR_STATES.UNLOADED:
                    UNLOADED_OnDisable();
                    break;
                case FLOWCHART_EDITOR_STATES.LOADED:
                    LOADED_OnDisable();
                    break;
                case FLOWCHART_EDITOR_STATES.RUNTIME_DEBUG:
                    RUNTIME_DEBUG_OnDisable();
                    break;
            }
        }

        void OnGUI()
        {
            switch (_state)
            {
                case FLOWCHART_EDITOR_STATES.INITIALIZATION:
                    Initialize();
                    break;
                case FLOWCHART_EDITOR_STATES.UNLOADED:
                    UNLOADED_OnGUI();
                    break;
                case FLOWCHART_EDITOR_STATES.LOADED:
                    LOADED_OnGUI();
                    break;
                case FLOWCHART_EDITOR_STATES.RUNTIME_DEBUG:
                    RUNTIME_DEBUG_OnGUI();
                    break;
            }


        }
        #endregion

        #region Enable Disable Proxy Calls
        void UNLOADED_OnEnable()
        {

        }

        void UNLOADED_OnDisable()
        {

        }

        void LOADED_OnEnable()
        {
            _target = new SerializedObject(_flowChart);
            Background_OnEnable();
            NodeManager_OnEnable();
            ToolBar_OnEnable();
            ProcessEvent_OnEnable();
            BlockEditor_OnEnable();
        }

        void LOADED_OnDisable()
        {
            Background_OnDisable();
            NodeManager_OnDisable();
            ToolBar_OnDisable();
            ProcessEvent_OnDisable();
            BlockEditor_OnDisable();
        }

        void RUNTIME_DEBUG_OnEnable()
        {
            Debug.Log("Entering playmode");
        }

        void RUNTIME_DEBUG_OnDisable()
        {
            
        }
        #endregion

        #region GUI Proxy Calls
        void LOADED_OnGUI()
        {
            //=========== DRAW ORDER===============
            Background_OnGUI();
            ToolBar_OnGUI();
            NodeManager_OnGUI();
            ProcessEvent_OnGUI();
        }

        void UNLOADED_OnGUI()
        {
            EmptyBackground_OnGUI();
        }

        void RUNTIME_DEBUG_OnGUI()
        {

        }
        #endregion




    }
}