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
        static SerializedObject _targetObject = default;
        static FlowChart _flowChart = default;
        static EditorState _state;

        #endregion

        #region Defintition
        enum EditorState
        {
            ///<Summary>State where the window editor has just been opened and needs to load its values</Summary>
            INITIALIZE = -1
            ,
            ///<Summary>State where the window editor has just been opened and needs to load its values</Summary>
            UNLOADED = 0
            ,
            LOADED = 1
            ,
            RUNTIME_DEBUG = 2
        }
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
            _state = EditorState.INITIALIZE;
        }

        #region Initializations
        ///<Summary>
        ///To be called when changing states to ensure that the current editor state's components have their OnDisables called
        ///</Summary>
        void REINITIALIZE()
        {
            if (_state != EditorState.INITIALIZE)
            {
                //Disable the previous state's components
                OnDisable();
            }
            AssignNewState();
        }

        //I dont put the code in OnEnable because if i want to intialize styles, i have to do in during OnGUI calls
        void INITIALIZE()
        {
            //Initalize whatever styles here 

            AssignNewState();
        }

        void AssignNewState()
        {
            switch (EditorApplication.isPlaying)
            {
                //============================ RUNTIME =======================
                case true:
                    Debug.Log("Debug mode for LEM 2 has not been implemented");
                    // //Get the flow chart during runtime then
                    // _flowChart = SaveManager_TryLoadFlowChartPath_Runtime();

                    // //This is for future development if i want to add a runtime debug mode
                    // _state = EditorState.RUNTIME_DEBUG;
                    // RUNTIME_DEBUG_OnEnable();

                    break;
                //============================ EDITOR TIME ==========================
                case false:
                    //Try get previous flowchart
                    if (_flowChart == null)
                    {
                        _flowChart = SaveManager_TryLoadFlowChartPath();
                    }

                    //This means user has opened flowchart editor from menu context
                    //Which for now..... is empty!
                    if (_flowChart == null)
                    {
                        _state = EditorState.UNLOADED;
                        UNLOADED_OnEnable();
                        return;
                    }

                    //This means user has opened flowchart editor via button press 
                    _state = EditorState.LOADED;
                    LOADED_OnEnable();

                    break;
            }

        }
        #endregion

        void OnDisable()
        {
            switch (_state)
            {
                case EditorState.UNLOADED:
                    UNLOADED_OnDisable();
                    break;
                case EditorState.LOADED:
                    LOADED_OnDisable();
                    break;
                case EditorState.RUNTIME_DEBUG:
                    RUNTIME_DEBUG_OnDisable();
                    break;
            }
            SaveManager_SaveFlowChartPath();
        }

        void OnGUI()
        {
            switch (_state)
            {
                case EditorState.INITIALIZE:
                    INITIALIZE();
                    break;
                case EditorState.UNLOADED:
                    UNLOADED_OnGUI();
                    break;
                case EditorState.LOADED:
                    LOADED_OnGUI();
                    break;
                case EditorState.RUNTIME_DEBUG:
                    RUNTIME_DEBUG_OnGUI();
                    break;
            }


        }
        #endregion

        #region Enable Disable Proxy Calls
        void UNLOADED_OnEnable() { }

        void UNLOADED_OnDisable() { }

        ///<Summary>Is called when there is a FlowChart.cs instance found. Calls all the various components to call their OnEnables</Summary>
        void LOADED_OnEnable()
        {
            _targetObject = new SerializedObject(_flowChart);
            LoadedBackground_OnEnable();
            NodeManager_OnEnable();
            ToolBar_OnEnable();
            ProcessEvent_OnEnable();
            BlockEditor_OnEnable();
        }

        ///<Summary>Is called on Disable if the window editor is LOADED</Summary>
        void LOADED_OnDisable()
        {
            LoadedBackground_OnDisable();
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
            LoadedBackground_OnGUI();
            ToolBar_OnGUI();
            NodeManager_OnGUI();
            ProcessEvent_OnGUI();
        }

        void UNLOADED_OnGUI()
        {
            UnloadedBackground_OnGUI();
        }

        void RUNTIME_DEBUG_OnGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                REINITIALIZE();
                return;
            }
        }
        #endregion




    }
}