namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Constants
        const string PREVIOUS_FLOWCHART_SCENEPATH_EDITORPREFS = "FlowChartPath";
       
        #endregion

        SerializedProperty _allBlocksArrayProperty;

        #region LifeTime
        void NodeManager_SaveManager_OnEnable()
        {
            NodeManager_LoadCachedBlockNodes();
            AssemblyReloadEvents.beforeAssemblyReload += HandleBeforeAssemblyReload;
            NodeManager_SaveManager_SaveFlowChartPath();
        }



        void NodeManager_SaveManager_OnDisable()
        {
            Debug.Log("Disable");
            NodeManager_SaveManager_SaveAllNodes();
            AssemblyReloadEvents.beforeAssemblyReload -= HandleBeforeAssemblyReload;
        }
        #endregion
        #region Handle Events

        // void NodeManager_SaveManager_HandlePlayModeStateChange(PlayModeStateChange obj)
        // {
        //     //For now runtime debugging will not be shown.
        //     Debug.Log(obj);
        //     // switch (obj)
        //     // {
        //     //     case PlayModeStateChange.EnteredEditMode:
        //     //         break;
        //     //     case PlayModeStateChange.EnteredPlayMode:
        //     //         break;
        //     //     case PlayModeStateChange.ExitingEditMode:
        //     //         NodeManager_SaveManager_SaveFlowChartPath();
        //     //         NodeManager_SaveManager_SaveAllNodes();
        //     //         break;
        //     //     case PlayModeStateChange.ExitingPlayMode:
        //     //         NodeManager_SaveManager_LoadFlowChartPath();
        //     //         break;
        //     // }
        // }

        private void HandleBeforeAssemblyReload()
        {
            Debug.Log("Before Assembly");
        }

        private void HandleAfterAssemblyReload()
        {
            Debug.Log("After Assembly");

        }

        #endregion

        #region Save Load
        void NodeManager_SaveManager_SaveFlowChartPath()
        {
            Transform t = _flowChart.transform;
            string path = t.name;

            while (t.parent != null)
            {
                path = path.Insert(0, $"{t.parent.name}/");
                t = t.parent;
            }

            //Append scene name
            path = path.Insert(0, $"{t.root.gameObject.scene.name}/");
            Debug.Log(path);


            EditorPrefs.SetString(PREVIOUS_FLOWCHART_SCENEPATH_EDITORPREFS, path);
        }

        void NodeManager_SaveManager_LoadFlowChartPath()
        {

        }

        void NodeManager_SaveManager_SaveAllNodes()
        {
            if (_allBlockNodes == null)
            {
                Debug.Log($"For some reason _allBlockNodes was null. This is probably due to the window being open during recompilation");
                return;
            }

            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                //Save to their 
                _allBlockNodes[i].SaveTo(_allBlocksArrayProperty.GetArrayElementAtIndex(i));
            }
        }

        void NodeManager_LoadCachedBlockNodes()
        {
            //======================== LOADING BLOCK NODES FROM BLOCKS ARRAY =============================
            _newBlockFromEnum = AddNewBlockFrom.None;
            _allBlocksArrayProperty = _target.FindProperty(FlowChart.BLOCKARRAY_PROPERTYNAME);
            _allBlockNodes = new List<BlockNode>();

            for (int i = 0; i < _allBlocksArrayProperty.arraySize; i++)
            {
                BlockNode b = NodeManager_GetNewNode();
                SerializedProperty e = _allBlocksArrayProperty.GetArrayElementAtIndex(i);
                b.LoadFrom(e);
                _allBlockNodes.Add(b);
            }
        }
        #endregion
    }

}