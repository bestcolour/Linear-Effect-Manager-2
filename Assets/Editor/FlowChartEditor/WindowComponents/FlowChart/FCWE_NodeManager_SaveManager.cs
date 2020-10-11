namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

    //Hanldes the saving and loading of the nodes 
    public partial class FlowChartWindowEditor : EditorWindow
    {
        SerializedProperty _allBlocksArrayProperty;

        #region LifeTime
        void NodeManager_SaveManager_OnEnable()
        {
            NodeManager_LoadCachedBlockNodes();
        }

        void NodeManager_SaveManager_OnDisable()
        {
            NodeManager_SaveManager_SaveAllNodes();
        }
        #endregion
      
        #region Saving Loading Nodes

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
            _allBlocksArrayProperty = _target.FindProperty(FlowChart.PROPERTYNAME_BLOCKARRAY);
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