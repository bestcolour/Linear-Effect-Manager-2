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
                _allBlockNodes[i].Save();
            }
        }

        void NodeManager_LoadCachedBlockNodes()
        {
            //======================== LOADING BLOCK NODES FROM BLOCKS ARRAY =============================
            _newBlockFromEnum = AddNewBlockFrom.None;
            _allBlocksArrayProperty = _targetObject.FindProperty(FlowChart.PROPERTYNAME_BLOCKARRAY);
            _allBlockNodes = new List<BlockNode>();
            _allBlockNodesDictionary = new Dictionary<string, BlockNode>();

            for (int i = 0; i < _allBlocksArrayProperty.arraySize; i++)
            {
                SerializedProperty e = _allBlocksArrayProperty.GetArrayElementAtIndex(i);
                BlockNode node = new BlockNode(e);

                //Record all the nodes
                _allBlockNodes.Add(node);
                _allBlockNodesDictionary.Add(node.Label, node);
            }
        }

        void NodeManager_ReloadCachedBlockNodes()
        {
            //======================== LOADING BLOCK NODES FROM BLOCKS ARRAY =============================
            _newBlockFromEnum = AddNewBlockFrom.None;
            _allBlocksArrayProperty = _targetObject.FindProperty(FlowChart.PROPERTYNAME_BLOCKARRAY);
            _allBlockNodes.Clear();
            // _allBlockNodesDictionary.Clear();

            for (int i = 0; i < _allBlocksArrayProperty.arraySize; i++)
            {
                SerializedProperty e = _allBlocksArrayProperty.GetArrayElementAtIndex(i);
                string label = e.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue;

                //Get blocknode ref which still exists in the dictionary
                BlockNode blockNode = _allBlockNodesDictionary[label];
                blockNode.Initialize(e);

                _allBlockNodes.Add(blockNode);
            }
        }

        #endregion
    }

}