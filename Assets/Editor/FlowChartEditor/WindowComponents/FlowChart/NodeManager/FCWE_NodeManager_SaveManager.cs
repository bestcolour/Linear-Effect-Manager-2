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
            _allBlockNodes = new List<BlockNode>();
            _allBlockNodesDictionary = new Dictionary<string, BlockNode>();
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
            //Clear all of the events subscriptions on all of the baseexecutors
            BaseEffectExecutor[] effectExecutors = _flowChart.GetComponents<BaseEffectExecutor>();
            foreach (var item in effectExecutors)
            {
                item.ClearAllSubs();
            }

            //======================== LOADING BLOCK NODES FROM BLOCKS ARRAY =============================
            _newBlockFromEnum = AddNewBlockFrom.None;
            _allBlocksArrayProperty = _targetObject.FindProperty(FlowChart.PROPERTYNAME_BLOCKARRAY);
            _allBlockNodes.Clear();
            _allBlockNodesDictionary.Clear();

            for (int i = 0; i < _allBlocksArrayProperty.arraySize; i++)
            {
                SerializedProperty blockProperty = _allBlocksArrayProperty.GetArrayElementAtIndex(i);
                BlockNode node = new BlockNode(blockProperty);

                //Add subscriptio to the respective holders
                Block block = _flowChart.Editor_GetBlock(node.Label);
                block.Editor_AddSubscription();

                //Record all the nodes
                _allBlockNodes.Add(node);
                _allBlockNodesDictionary.Add(node.Label, node);
            }
        }
        #endregion
    }

}