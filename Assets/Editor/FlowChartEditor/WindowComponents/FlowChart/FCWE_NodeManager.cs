namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        //Gets triggered whenever NodeManager_CreateNewBlock() gets called since genericmenu doesnt allow creation of a new class type within the function
        enum AddNewBlockFrom { None, ToolBar, ContextMenu }
        AddNewBlockFrom _newBlockFromEnum;

        List<BlockNode> _allBlocks;
        HashSet<BlockNode> _selectedBlocks;
        bool? _isDraggingBlocks;

        #region LifeCycle Method
        private void NodeManager_OnEnable()
        {
            _allBlocks = new List<BlockNode>();
            _selectedBlocks = new HashSet<BlockNode>();
            _newBlockFromEnum = AddNewBlockFrom.None;
            _isDraggingBlocks = null;

            OnPan += NodeManager_HandlePan;
            OnLeftMouseDownInGraph += NodeManager_HandleLeftMouseDownInGraph;
            OnMouseDrag += NodeManager_HandleMouseDrag;
            OnLeftMouseUpInGraph += NodeManager_HandleLeftmouseUpInGraph;
        }

        private void NodeManager_OnDisable()
        {
            OnLeftMouseDownInGraph -= NodeManager_HandleLeftMouseDownInGraph;
            OnPan -= NodeManager_HandlePan;
            OnMouseDrag -= NodeManager_HandleMouseDrag;
            OnLeftMouseUpInGraph -= NodeManager_HandleLeftmouseUpInGraph;
        }

        private void NodeManager_OnGUI()
        {

            //Idk why but i cant create a new instance of custom class inside of Update/InspectorUpdate/Genric Menu callbac. So im addign it here
            Event e = Event.current;

            if (_newBlockFromEnum != AddNewBlockFrom.None)
            {
                NodeManager_CreateNewBlock();
                return;
            }

            NodeManager_Draw();
            NodeManager_DrawDebugger();


        }
        #endregion

        void NodeManager_Draw()
        {
            //Draw from the bottom up
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                _allBlocks[i].Draw();
            }

        }

        void NodeManager_DrawDebugger()
        {
            Rect rect = Rect.zero;
            //Abit of border
            rect.x = 5f;
            rect.y = position.height - 5f - EditorGUIUtility.singleLineHeight;
            rect.width = position.width;
            rect.height = EditorGUIUtility.singleLineHeight;

            GUI.Label
            (
                rect,
                $"Number of selected blocks: {_selectedBlocks.Count}"
            );
        }



        #region Event Handlers
        void NodeManager_HandlePan(Vector2 mouseDelta)
        {
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                _allBlocks[i].ProcessMouseDrag(mouseDelta);
            }
        }

        void NodeManager_HandleMouseDrag(Vector2 mouseDelta)
        {
            switch (_isDraggingBlocks)
            {
                //================= IS DRAGGING HAS NOT BEEN DECIDED ===================
                case null:
                    _isDraggingBlocks = _selectedBlocks.Count > 0;
                    break;

                //================= IS DRAGGING BLOCKS ===================
                case true:
                    foreach (BlockNode node in _selectedBlocks)
                    {
                        node.ProcessMouseDrag(mouseDelta);
                    }
                    break;

                //================= IS NOT DRAGGING BLOCKS ===================
                default: break;
            }

        }

        void NodeManager_HandleLeftMouseDownInGraph()
        {


            // Event e = Event.current;

            // //================== SHIFT HELD ====================
            // if (e.shift)
            // {
            //     for (int i = 0; i < _allBlocks.Count; i++)
            //     {
            //         if (_allBlocks[i].UpdateIfClicked())
            //         {
            //             //Stop loop once node has been found
            //             NodeManager_ToggleBlockSelection(i);
            //             Repaint();
            //             return;
            //         }
            //     }
            //     return;
            // }

            // //================== NO SHIFT HELD ==========================
            // _selectedBlocks.Clear();

            // int selectedNodeIndex = -1;

            // //Since selected block will usally be at the last index of _allBlock, looping it from 0 to count will ensure that all other blocks gets a chance to consider itself as the newly selected block before the previous selected block
            // for (int i = 0; i < _allBlocks.Count; i++)
            // {
            //     if (_allBlocks[i].UpdateIsSelected(selectedNodeIndex != -1))
            //     {
            //         selectedNodeIndex = i;
            //     }
            // }

            // NodeManager_SelectBlockNode(selectedNodeIndex);
            // Repaint();
        }

        void NodeManager_HandleLeftmouseUpInGraph()
        {
            Event e = Event.current;

            //Reset IsDragging
            if (_isDraggingBlocks == true)
            {

            }
            else
            {

            }


            //================== SHIFT HELD ====================
            if (e.shift)
            {
                for (int i = 0; i < _allBlocks.Count; i++)
                {
                    if (_allBlocks[i].UpdateIfClicked())
                    {
                        //Stop loop once node has been found
                        NodeManager_ToggleBlockSelection(i);
                        Repaint();
                        return;
                    }
                }
                return;
            }

            //================== NO SHIFT HELD ==========================
            _selectedBlocks.Clear();

            int selectedNodeIndex = -1;

            //Since selected block will usally be at the last index of _allBlock, looping it from 0 to count will ensure that all other blocks gets a chance to consider itself as the newly selected block before the previous selected block
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                if (_allBlocks[i].UpdateIsSelected(selectedNodeIndex != -1))
                {
                    selectedNodeIndex = i;
                }
            }

            NodeManager_SelectBlockNode(selectedNodeIndex);
            Repaint();
        }


        #endregion


        //================================================= BLOCK FUNCTIONS ==================================================

        #region Creating Blocks
        void NodeManager_TriggerCreateNewBlock(AddNewBlockFrom from)
        {
            _newBlockFromEnum = from;
        }

        void NodeManager_CreateNewBlock()
        {
            BlockNode b;
            switch (_newBlockFromEnum)
            {
                case AddNewBlockFrom.ContextMenu:
                    b = new BlockNode(Event.current.mousePosition);
                    break;

                case AddNewBlockFrom.ToolBar:
                    b = new BlockNode(CenterScreen);
                    break;

                default: b = new BlockNode(CenterScreen); break;
            }

            _allBlocks.Add(b);
            _newBlockFromEnum = AddNewBlockFrom.None;
        }
        #endregion


        #region Selecting Block
        ///<Summary>
        /// Is called to select one and only one block node with the rest all cleared
        ///</Summary>
        void NodeManager_SelectBlockNode(int i)
        {
            //Return if no blocks were selected
            if (i < 0) return;

            _selectedBlocks.Add(_allBlocks[i]);

            //Send the selected block to the end of the list so that it will be rendered on top
            int lastIndex = _allBlocks.Count - 1;
            BlockNode selectedBlock = _allBlocks[i], lastBlock = _allBlocks[lastIndex];

            _allBlocks[lastIndex] = selectedBlock;
            _allBlocks[i] = lastBlock;
        }

        void NodeManager_ToggleBlockSelection(int i)
        {
            //Return if no blocks were selected
            if (i < 0) return;

            //Send the selected block to the end of the list so that it will be rendered on top
            int lastIndex = _allBlocks.Count - 1;
            BlockNode selectedBlock = _allBlocks[i];

            if (_selectedBlocks.Contains(selectedBlock))
            {
                _selectedBlocks.Remove(selectedBlock);
            }
            else
            {
                _selectedBlocks.Add(selectedBlock);

                BlockNode lastBlock = _allBlocks[lastIndex];
                _allBlocks[lastIndex] = selectedBlock;
                _allBlocks[i] = lastBlock;
            }
        }

        #endregion




    }

}