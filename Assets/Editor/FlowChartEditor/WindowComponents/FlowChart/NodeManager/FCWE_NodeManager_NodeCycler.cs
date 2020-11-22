
namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

    //This code handles all the creation and deletion of block nodes in the window editor 
    public partial class FlowChartWindowEditor : EditorWindow
    {

        #region Lifetime Methods

        void NodeManager_NodeCycler_OnGUI()
        {
            if (_newBlockFromEnum != AddNewBlockFrom.None)
            {
                NodeManager_NodeCycler_AddNewNode();
            }
        }

        #endregion


        #region Creating NodeBlocks
        void NodeManager_NodeCycler_TriggerCreateNewNode(AddNewBlockFrom from)
        {
            _newBlockFromEnum = from;
        }

        BlockNode NodeManager_NodeCycler_AddNewNode()
        {
            BlockNode node;
            switch (_newBlockFromEnum)
            {
                case AddNewBlockFrom.ContextMenu:
                    node = NodeManager_NodeCycler_AddNewNode(Event.current.mousePosition);
                    break;

                case AddNewBlockFrom.ToolBar:
                    node = NodeManager_NodeCycler_AddNewNode(CenterScreen);
                    break;

                //Fall back code
                default:
                    node = NodeManager_NodeCycler_AddNewNode(Vector2.zero);
                    break;
            }

            return node;
        }

        BlockNode NodeManager_NodeCycler_AddNewNode(Vector2 position)
        {
            //Ensure that block gets a unique label name every time a new node is added

            Block b = new Block(position, NodeManager_NodeCycler_GetUniqueBlockName(Block.DEFAULT_BLOCK_NAME));

            SerializedProperty newBlockProperty = _allBlocksArrayProperty.AddToSerializedPropertyArray(b);
            BlockNode node = new BlockNode(newBlockProperty);

            _allBlockNodes.Add(node);
            _allBlockNodesDictionary.Add(node.Label, node);
            _newBlockFromEnum = AddNewBlockFrom.None;
            return node;
        }

        private string NodeManager_NodeCycler_GetUniqueBlockName(string defaultName)
        {
            //Add a space to ensure that there is a whitespace
            string s = defaultName + " ";

            for (int i = 0; i < int.MaxValue; i++)
            {
                string unqName = s + i;
                //Keep looping if there is an entry called "New Block 1", "New Block 2", ....
                if (_allBlockNodesDictionary.ContainsKey(unqName))
                    continue;

                //Else if the dict doesnt hv this name,
                return unqName;
            }

            Debug.LogError($"There is no way you create 2147483647 blocks with all their names as {defaultName} <number>.... you monster why are you like this....");
            return null;
        }

        #endregion

        #region Deleting NodeBlocks
        void NodeManager_NodeCycler_DeleteNode()
        {
            //Get the indices of all the selected blocks

            //Check if the block or anyone of the block has more than 1 effects inside of it then
            //add a pop up to say that there is no way of getting it back

            //if player accepts, yeet the entire selected blocks
            //remove them from the list and the dictionary



        }


        #endregion


    }

}

