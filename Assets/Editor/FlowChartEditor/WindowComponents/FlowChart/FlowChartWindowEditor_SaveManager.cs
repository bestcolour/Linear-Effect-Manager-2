namespace LinearEffectsEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    //Handles the saving and loading of FlowChartWindow level data ie. FlowChart _target variable
    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Constants
        const string EDITORPREFS_PREV_FLOWCHART_SCENEPATH = "FlowChartPath";

        #endregion

        #region  Lifetime
        void SaveManager_OnEnable()
        {

        }

        void SaveManager_OnDisable()
        {

        }
        #endregion


        #region Handle Events
        private void HandleBeforeAssemblyReload()
        {
            Debug.Log("Before Assembly");
        }

        private void HandleAfterAssemblyReload()
        {
            Debug.Log("After Assembly");

        }

        #endregion

        void NodeManager_SaveManager_SaveFlowChartPath()
        {
            EditorPrefs.SetString(EDITORPREFS_PREV_FLOWCHART_SCENEPATH, _flowChart.transform.GetFullPath());
        }

        void NodeManager_SaveManager_LoadFlowChartPath()
        {
            string s = EditorPrefs.GetString(EDITORPREFS_PREV_FLOWCHART_SCENEPATH);

        }
    }

}