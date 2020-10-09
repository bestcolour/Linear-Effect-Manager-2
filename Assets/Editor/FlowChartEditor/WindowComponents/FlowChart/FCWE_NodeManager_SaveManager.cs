namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

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

        void NodeManager_SaveManager_SaveAllNodes()
        {
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

    }

}