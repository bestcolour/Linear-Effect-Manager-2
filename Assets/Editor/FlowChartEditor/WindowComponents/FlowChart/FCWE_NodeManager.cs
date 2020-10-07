namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Definitions
        //Gets triggered whenever NodeManager_CreateNewBlock() gets called since genericmenu doesnt allow creation of a new class type within the function
        enum AddNewBlockFrom { None, ToolBar, ContextMenu }
        enum DragBlockState { None, HasPotential, HadDragged }
        #endregion

        #region Statics
        static GUIStyle DebugStyle;
        static GUIContent DebugGUIContent;
        #endregion



        List<BlockNode> _allBlocks;
        HashSet<BlockNode> _selectedBlocks;

        #region States
        AddNewBlockFrom _newBlockFromEnum;
        bool? _isDraggingBlocks;
        #endregion

        #region LifeCycle Method
        private void NodeManager_OnEnable()
        {
            InitializeDebugger();
            _allBlocks = new List<BlockNode>();
            _selectedBlocks = new HashSet<BlockNode>();
            _newBlockFromEnum = AddNewBlockFrom.None;
            _isDraggingBlocks = null;
            _selectedBlockIndex = -1;

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

        void InitializeDebugger()
        {
            DebugStyle = new GUIStyle();
            DebugStyle.wordWrap = true;
            DebugStyle.normal.textColor = Color.red;
            DebugGUIContent = new GUIContent();
        }

        #endregion
        #region Draw

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
            rect.y = TOOLBAR_HEIGHT;

            string debugStatement = $"Number of selected blocks: {_selectedBlocks.Count} \n ";


            DebugGUIContent.text = debugStatement;
            rect.size = DebugStyle.CalcSize(DebugGUIContent);

            GUI.Label
            (
                rect,
                DebugGUIContent,
                DebugStyle
            );
        }
        #endregion



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
                //================= HAS NO POTENTIAL OF EVER HAPPENING ===================
                case null:
                    break;

                //================= HAS POTENTIAL OF HAPPENING ===================
                case false:
                    //It has been decided that user wants to use this potential to drag blocks
                    _isDraggingBlocks = true;

                    break;

                //================= IS USING POTENTIAL TO DRAG BLOCKS ===================
                case true:
                    foreach (BlockNode node in _selectedBlocks)
                    {
                        node.ProcessMouseDrag(mouseDelta);
                    }
                    break;
            }



        }

        //used to communicate which block was selected in MouseDown to MouseUp
        int _selectedBlockIndex;

        void NodeManager_HandleLeftMouseDownInGraph()
        {
            //================= FINDING CLICKED NODE ======================
            _selectedBlockIndex = -1;

            for (int i = 0; i < _allBlocks.Count; i++)
            {
                if (_allBlocks[i].CheckIfClicked())
                {
                    _selectedBlockIndex = i;
                    break;
                }
            }

            //Dont update potential if there is no block selected.
            if (_selectedBlockIndex == -1)
                return;

            //If selected block was not alrady selected,
            if (!_selectedBlocks.Contains(_allBlocks[_selectedBlockIndex]))
                return;


            //===================== DETERMINE DRAGBLOCKS POTENTIAL ===========================
            // false = yes, there is potential
            // null = no, there is no potential
            _isDraggingBlocks = _selectedBlocks.Count > 0 ? (bool?)false : null;

        }

        void NodeManager_HandleLeftmouseUpInGraph()
        {
            //Previously had used the potential to dragg blocks
            if (_isDraggingBlocks == true)
            {
                _isDraggingBlocks = null;
                return;
            }


            Event e = Event.current;
            //================== SHIFT HELD ====================
            if (e.shift)
            {

                NodeManager_ToggleBlockSelection();
                Repaint();
                return;

                // for (int i = 0; i < _allBlocks.Count; i++)
                // {
                //     if (_allBlocks[i].UpdateIfClicked())
                //     {
                //         //Stop loop once node has been found
                //         NodeManager_ToggleBlockSelection(i);
                //         Repaint();
                //         return;
                //     }
                // }
            }

            //================== NO SHIFT HELD ==========================
            _selectedBlocks.Clear();


            //Reset all block's select state
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                _allBlocks[i].IsSelected = false;
            }

            //Select the selected block if there is one. This causes the selectedblock to be sent to the end of the list
            NodeManager_SelectBlockNode();
            Repaint();



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
        void NodeManager_SelectBlockNode()
        {
            //Return if no blocks were selected
            if (_selectedBlockIndex < 0) return;

            _selectedBlocks.Add(_allBlocks[_selectedBlockIndex]);


            //Send the selected block to the end of the list so that it will be rendered on top
            int lastIndex = _allBlocks.Count - 1;
            BlockNode selectedBlock = _allBlocks[_selectedBlockIndex], lastBlock = _allBlocks[lastIndex];

            selectedBlock.IsSelected = true;
            _allBlocks[lastIndex] = selectedBlock;
            _allBlocks[_selectedBlockIndex] = lastBlock;
        }

        void NodeManager_ToggleBlockSelection()
        {
            //Return if no blocks were selected
            if (_selectedBlockIndex < 0) return;

            //Send the selected block to the end of the list so that it will be rendered on top
            int lastIndex = _allBlocks.Count - 1;
            BlockNode selectedBlock = _allBlocks[_selectedBlockIndex];

            if (_selectedBlocks.Contains(selectedBlock))
            {
                selectedBlock.IsSelected = false;
                _selectedBlocks.Remove(selectedBlock);
            }
            else
            {
                _selectedBlocks.Add(selectedBlock);
                selectedBlock.IsSelected = true;

                BlockNode lastBlock = _allBlocks[lastIndex];
                _allBlocks[lastIndex] = selectedBlock;
                _allBlocks[_selectedBlockIndex] = lastBlock;
            }
        }

        #endregion




    }

}