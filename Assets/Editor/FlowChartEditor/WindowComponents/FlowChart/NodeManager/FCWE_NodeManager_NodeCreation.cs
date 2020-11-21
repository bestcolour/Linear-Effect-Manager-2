
namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

//This code handles all the creation of new block nodes in the window editor 
    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Lifetime Methods

        void NodeManager_NodeCreation_OnGUI()
        {
            if (_newBlockFromEnum != AddNewBlockFrom.None)
            {
                NodeManager_NodeCreation_AddNewNode();
            }
        }

        #endregion


        #region Creating NodeBlocks
        void NodeManager_NodeCreation_TriggerCreateNewNode(AddNewBlockFrom from)
        {
            _newBlockFromEnum = from;
        }

        BlockNode NodeManager_NodeCreation_AddNewNode()
        {
            BlockNode node;
            switch (_newBlockFromEnum)
            {
                case AddNewBlockFrom.ContextMenu:
                    node = NodeManager_NodeCreation_AddNewNode(Event.current.mousePosition);
                    break;

                case AddNewBlockFrom.ToolBar:
                    node = NodeManager_NodeCreation_AddNewNode(CenterScreen);
                    break;

                //Fall back code
                default:
                    node = NodeManager_NodeCreation_AddNewNode(Vector2.zero);
                    break;
            }

            return node;
        }

        BlockNode NodeManager_NodeCreation_AddNewNode(Vector2 position)
        {
            //Ensure that block gets a unique label name every time a new node is added

            Block b = new Block(position, NodeManager_NodeCreation_GetUniqueBlockName());

            SerializedProperty newBlockProperty = _allBlocksArrayProperty.AddToSerializedPropertyArray(b);
            BlockNode node = new BlockNode(newBlockProperty);

            _allBlockNodes.Add(node);
            _allBlockNodesDictionary.Add(node.Label, node);
            _newBlockFromEnum = AddNewBlockFrom.None;
            return node;
        }

        private string NodeManager_NodeCreation_GetUniqueBlockName()
        {
            string s = "New Block ";

            for (int i = 0; i < int.MaxValue; i++)
            {
                string unqName = s + i;
                //Keep looping if there is an entry called "New Block 1", "New Block 2", ....
                if (_allBlockNodesDictionary.ContainsKey(unqName))
                    continue;

                //Else if the dict doesnt hv this name,
                return unqName;
            }

            Debug.LogError("There is no way you create 2147483647 blocks with all their names as New Block <number>.... you monster why are you like this....");
            return null;
        }

        #endregion
    }

}

