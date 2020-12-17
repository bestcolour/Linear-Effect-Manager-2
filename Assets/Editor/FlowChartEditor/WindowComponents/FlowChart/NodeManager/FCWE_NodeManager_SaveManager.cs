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
                item.InitializeSubs(NodeManager_HandleOnRemoveEvent, NodeManager_HandleOnInsertEvent);
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
                // block.Editor_AddSubscription();

                //Record all the nodes
                _allBlockNodes.Add(node);
                _allBlockNodesDictionary.Add(node.Label, node);
            }
        }
        #endregion

        #region Handles
        private void NodeManager_HandleOnRemoveEvent(int removedIndex, string fullEffectName)
        {
            //Search through every block
            for (int blockIndex = 0; blockIndex < _allBlocksArrayProperty.arraySize; blockIndex++)
            {
                SerializedProperty blockProperty = _allBlocksArrayProperty.GetArrayElementAtIndex(blockIndex);
                SerializedProperty orderArray = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_ORDERARRAY);

                for (int orderIndex = 0; orderIndex < orderArray.arraySize; orderIndex++)
                {
                    //Check if block's effect order name is the same as the fullEffectname
                    SerializedProperty orderElement = orderArray.GetArrayElementAtIndex(orderIndex);
                    string fullName = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_FULLEFFECTNAME).stringValue;

Debug.Log($"FullEffect Name {fullEffectName} FullName: {fullName}");

                    if (fullName != fullEffectName)
                    {
                        continue;
                    }

                    //Check if the removed index is smaller than this order element's index
                    SerializedProperty dataElementProperty = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_DATAELEMENTINDEX);
                    int dataElmtIndex = dataElementProperty.intValue;
                    if (removedIndex < dataElmtIndex)
                    {
                        //set the data element index to something decremented
                        dataElmtIndex--;
                        dataElementProperty.serializedObject.Update();
                        dataElementProperty.intValue = dataElmtIndex;
                        dataElementProperty.serializedObject.ApplyModifiedProperties();
                    }

                }


            }



        }


        private void NodeManager_HandleOnInsertEvent(int insertedIndex, string fullEffectName)
        {
            //Search through every block
            for (int blockIndex = 0; blockIndex < _allBlocksArrayProperty.arraySize; blockIndex++)
            {
                SerializedProperty blockProperty = _allBlocksArrayProperty.GetArrayElementAtIndex(blockIndex);
                SerializedProperty orderArray = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_ORDERARRAY);

                for (int orderIndex = 0; orderIndex < orderArray.arraySize; orderIndex++)
                {
                    //Check if block's effect order name is the same as the fullEffectname
                    SerializedProperty orderElement = orderArray.GetArrayElementAtIndex(orderIndex);
                    string fullName = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_FULLEFFECTNAME).stringValue;

                    if (fullName != fullEffectName)
                    {
                        continue;
                    }

                    //Check if the removed index is smaller than this order element's index
                    SerializedProperty dataElementProperty = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_DATAELEMENTINDEX);
                    int dataElmtIndex = dataElementProperty.intValue;
                    if (insertedIndex > dataElmtIndex)
                    {
                        //set the data element index to something decremented
                        dataElmtIndex++;
                        dataElementProperty.serializedObject.Update();
                        dataElementProperty.intValue = dataElmtIndex;
                        dataElementProperty.serializedObject.ApplyModifiedProperties();
                    }

                }


            }

        }
        #endregion

    }

}