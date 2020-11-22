
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

        #region Constants
        const string DELETEWARNING_TITLE = "Are you sure you want to delete?"
        , DELETEWARNING_MESSAGE = "Deletion cannot be undoned once executed"
        , DELETEWARNING_OK = "Continue"
        , DELETEWARNING_CANCEL = "Cancel"
        ;
        #endregion

        bool _deleteWarningBox = default;


        #region Lifetime Methods

        void NodeManager_NodeCycler_OnGUI()
        {

            if (_newBlockFromEnum != AddNewBlockFrom.None)
            {
                NodeManager_NodeCycler_AddNewNode();
            }

            //=========== DRAW DELETE BOX ==============
            NodeManager_NodeCycler_DrawDeleteWarning();
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
        void NodeManager_NodeCycler_DeleteButton()
        {
            //Check if the block or anyone of the selected block has more than 1 effects inside of it then
            foreach (var blockNode in _selectedBlocks)
            {
                int effectCount = blockNode.GetEffectCount;
                //If effect count is more than 0 exit loop and open up warning window
                if (effectCount > 0)
                {
                    // Debug.Log($"There are {effectCount} effects! Open warning window");
                    //add a pop up to say that there is no way of getting it back
                    _deleteWarningBox = true;
                    return;
                }
            }

            //Delete nodes if all of the nodes are empty of effect orders
            NodeManager_NodeCycler_DeleteSelectedNodes();
        }

        void NodeManager_NodeCycler_DrawDeleteWarning()
        {
            if (!_deleteWarningBox)
            {
                return;
            }

            switch (EditorUtility.DisplayDialogComplex(DELETEWARNING_TITLE, DELETEWARNING_MESSAGE, DELETEWARNING_OK, DELETEWARNING_CANCEL, string.Empty))
            {
                //============= OK BUTTON PRESSED ==============
                case 0:
                    NodeManager_NodeCycler_DeleteSelectedNodes();
                    //Close the warning box
                    _deleteWarningBox = false;
                    break;


                //=============== CANCEL BUTTON PRESSED ===============
                case 1:
                    _deleteWarningBox = false;
                    break;
                // ==================== ALT BUTTON PRESSED ================
                default:
                    _deleteWarningBox = false;
                    break;
            }

        }

        void NodeManager_NodeCycler_DeleteSelectedNodes()
        {
            //if player accepts, yeet the entire selected blocks
            //remove them from the list and the dictionary
            foreach (var blockNode in _selectedBlocks)
            {
                //Close blocknode editor if it is being yeeted
                if (_blockEditor.Block.BlockName == blockNode.Label)
                {
                    BlockEditor_HandleOnNoBlockNodeFound();
                }

                //Remove from list
                int index = _allBlockNodes.IndexOf(blockNode);
                _allBlockNodes.RemoveAt(index);
                _allBlockNodesDictionary.Remove(blockNode.Label);

                //Get block from flow chart and then remove all order data
                Block block = _flowChart.GetBlock(index);
                block.RemoveAllOrderData();

                // blockNode.OnDelete();

                //Save the array property
                _allBlocksArrayProperty.serializedObject.Update();
                _allBlocksArrayProperty.DeleteArrayElement
                (
                    (x) =>
                    {
                        string a = x.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue;
                        string b = blockNode.BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue;
                        return a == b;
                    }
                );

                _allBlocksArrayProperty.serializedObject.ApplyModifiedProperties();

            }

        }

        #endregion


    }

}

