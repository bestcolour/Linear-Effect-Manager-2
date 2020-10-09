﻿namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Definitions
        //Gets triggered whenever NodeManager_CreateNewBlock() gets called since genericmenu doesnt allow creation of a new class type within the function
        enum AddNewBlockFrom { None, ToolBar, ContextMenu }
        enum DragState { Default, DrawSelection_HasPotential, DrawSelection_HadDragged, DragBlocks_HasPotential, DragBlocks_HadDraggedBlock }
        public delegate void ClickOnBlockNodeCallback(BlockNode block);
        #endregion

        #region Statics
        static GUIStyle DebugStyle;
        static GUIContent DebugGUIContent;
        #endregion

        #region Constants
        static readonly Color SELECTIONBOX_COLOUR = new Color(.75f, .93f, .93f, 0.5f);
        #endregion

        #region Events
        public event ClickOnBlockNodeCallback OnSelectBlockNode = null;
        #endregion


        #region States
        AddNewBlockFrom _newBlockFromEnum;
        DragState _dragState;
        #endregion

        #region Var
        Rect _selectionBox;


        //Optimise drawcalls later by doing occulsion culling
        //because this script isnt gunna get compiledi into the final build, ill use list instead of array
        List<BlockNode> _allBlockNodes;
        SerializedProperty _allBlocksArrayProperty;

        HashSet<BlockNode> _selectedBlocks;
        #endregion



        #region LifeCycle Method
        private void NodeManager_OnEnable()
        {
            _selectedBlocks = new HashSet<BlockNode>();
            InitializeDebugger();
            NodeManager_LoadCachedBlockNodes();
            _selectedBlockIndex = -1;
            _dragState = DragState.Default;
            _selectionBox = Rect.zero;

            OnPan += NodeManager_HandlePan;
            OnLeftMouseDownInGraph += NodeManager_HandleLeftMouseDownInGraph;
            OnMouseDrag += NodeManager_HandleMouseDrag;
            OnLeftMouseUpInGraph += NodeManager_HandleLeftmouseUpInGraph;
            EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;

        }


        private void NodeManager_OnDisable()
        {
            OnLeftMouseDownInGraph -= NodeManager_HandleLeftMouseDownInGraph;
            OnPan -= NodeManager_HandlePan;
            OnMouseDrag -= NodeManager_HandleMouseDrag;
            OnLeftMouseUpInGraph -= NodeManager_HandleLeftmouseUpInGraph;
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;

        }

        private void NodeManager_OnGUI()
        {

            //Idk why but i cant create a new instance of custom class inside of Update/InspectorUpdate/Genric Menu callbac. So im addign it here
            Event e = Event.current;

            if (_newBlockFromEnum != AddNewBlockFrom.None)
            {
                NodeManager_GetNewNode();
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
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].Draw();
            }

            //Draw Selection box
            if (_dragState == DragState.DrawSelection_HadDragged)
            {
                Event e = Event.current;
                _selectionBox.width = e.mousePosition.x - _selectionBox.x;
                _selectionBox.height = e.mousePosition.y - _selectionBox.y;

                Color prevColour = GUIExtensions.Start_GUI_ColourChange(SELECTIONBOX_COLOUR);
                GUI.Box(_selectionBox, string.Empty);
                GUIExtensions.End_GUI_ColourChange(prevColour);
            }

        }

        void NodeManager_DrawDebugger()
        {
            Rect rect = Rect.zero;
            //Abit of border
            rect.x = 5f;
            rect.y = TOOLBAR_HEIGHT;

            string debugStatement = $"Number of selected blocks: {_selectedBlocks.Count} \n Drag State: {_dragState} ";


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
        //Handles all of the window node's current cache whenever recompilation occurs
        private void HandlePlayModeStateChanged(PlayModeStateChange obj)
        {
            Debug.Log(obj);

        }


        void NodeManager_HandlePan(Vector2 mouseDelta)
        {
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].ProcessMouseDrag(mouseDelta);
            }
        }

        void NodeManager_HandleMouseDrag(Vector2 mouseDelta)
        {
            switch (_dragState)
            {
                //============== HAS NO POTENTIAL OF EITHER DRAGBLOCK OR SELECTIONBOX EVER HAPPENING ===================
                case DragState.Default:
                    break;

                //======================= HANDLE SELECTION BOX LOGIC ==========================
                case DragState.DrawSelection_HasPotential:
                    //If code flows here, _selectedBlockIndex == -1
                    _dragState = DragState.DrawSelection_HadDragged;
                    break;

                case DragState.DrawSelection_HadDragged:

                    for (int i = 0; i < _allBlockNodes.Count; i++)
                    {
                        if (_allBlockNodes[i].CheckRectOverlap(_selectionBox))
                        {
                            _allBlockNodes[i].IsSelected = true;
                            _selectedBlocks.Add(_allBlockNodes[i]);
                        }
                        else
                        {
                            _allBlockNodes[i].IsSelected = false;
                            _selectedBlocks.Remove(_allBlockNodes[i]);
                        }
                    }
                    break;

                //================= HAS POTENTIAL OF HAPPENING ===================
                case DragState.DragBlocks_HasPotential:
                    //It has been decided that user wants to use this potential to drag blocks
                    _dragState = DragState.DragBlocks_HadDraggedBlock;

                    break;

                //================= IS USING POTENTIAL TO DRAG BLOCKS ===================
                case DragState.DragBlocks_HadDraggedBlock:
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

            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                if (_allBlockNodes[i].CheckIfClicked())
                {
                    _selectedBlockIndex = i;
                    break;
                }
            }

            //==================== NO CLICKED NODE FOUND ======================

            if (_selectedBlockIndex == -1)
            {
                //Start Drawing Selection box
                _selectionBox.position = Event.current.mousePosition;
                _dragState = DragState.DrawSelection_HasPotential;

                return;
            }


            //==================== CLICKED NODE FOUND ======================
            Event e = Event.current;
            _dragState = DragState.DragBlocks_HasPotential;

            //==================== DRAGGING MULTIPLE NODES ======================
            if (_selectedBlocks.Contains(_allBlockNodes[_selectedBlockIndex]))
            {
                return;
            }

            //================== SHIFT HELD ====================
            if (e.shift)
            {
                NodeManager_ToggleNodeSelection();
                Repaint();
                return;
            }

            //================== NO SHIFT HELD =================
            //Reset all block's select state
            NodeManager_ClearAllSelectedNodes();

            //Select the selected block if there is one. This causes the selectedblock to be sent to the end of the list
            NodeManager_SelectNode();
            Repaint();
        }

        void NodeManager_HandleLeftmouseUpInGraph()
        {

            switch (_dragState)
            {
                //Previously had used the potential to dragg blocks
                case DragState.DragBlocks_HadDraggedBlock:
                    _dragState = DragState.Default;
                    break;

                case DragState.DrawSelection_HadDragged:
                    _dragState = DragState.Default;
                    break;

                case DragState.DragBlocks_HasPotential:
                    _dragState = DragState.Default;
                    break;


                default:
                    _dragState = DragState.Default;
                    NodeManager_ClearAllSelectedNodes();
                    break;
            }

            Repaint();
        }


        #endregion


        //================================================= SUPPORTING FUNCTIONS ==================================================
        #region Loading Blocks
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
        #endregion
        #region Creating Blocks
        void NodeManager_TriggerCreateNewNode(AddNewBlockFrom from)
        {
            _newBlockFromEnum = from;
        }

        BlockNode NodeManager_GetNewNode()
        {
            BlockNode node;
            switch (_newBlockFromEnum)
            {
                case AddNewBlockFrom.ContextMenu:
                    node = NodeManager_CreateNewNode(Event.current.mousePosition);
                    break;

                case AddNewBlockFrom.ToolBar:
                    node = NodeManager_CreateNewNode(CenterScreen);
                    break;

                //Code runs thru here when loading nodes
                default:
                    node = new BlockNode(CenterScreen);
                    break;
            }

            return node;
        }

        BlockNode NodeManager_CreateNewNode(Vector2 position)
        {
            BlockNode node = new BlockNode();
            Block b = new Block(position);

            //Newly created node should load from a newly created block to get all the default values
            node.LoadFrom(b);

            _allBlocksArrayProperty.AddToBlockPropertyArray(b);
            _allBlockNodes.Add(node);
            _newBlockFromEnum = AddNewBlockFrom.None;
            return node;
        }

        #endregion
        #region Selecting Block
        ///<Summary>
        /// Is called to select one and only one block node with the rest all cleared
        ///</Summary>
        void NodeManager_SelectNode()
        {
            //Return if no blocks were selected
            if (_selectedBlockIndex < 0) return;

            _selectedBlocks.Add(_allBlockNodes[_selectedBlockIndex]);


            //Send the selected block to the end of the list so that it will be rendered on top
            int lastIndex = _allBlockNodes.Count - 1;
            BlockNode selectedBlock = _allBlockNodes[_selectedBlockIndex], lastBlock = _allBlockNodes[lastIndex];

            selectedBlock.IsSelected = true;
            _allBlockNodes[lastIndex] = selectedBlock;
            _allBlockNodes[_selectedBlockIndex] = lastBlock;

            OnSelectBlockNode?.Invoke(selectedBlock);
        }

        void NodeManager_ToggleNodeSelection()
        {
            //Return if no blocks were selected
            if (_selectedBlockIndex < 0) return;

            //Send the selected block to the end of the list so that it will be rendered on top
            int lastIndex = _allBlockNodes.Count - 1;
            BlockNode selectedBlock = _allBlockNodes[_selectedBlockIndex];

            if (_selectedBlocks.Contains(selectedBlock))
            {
                selectedBlock.IsSelected = false;
                _selectedBlocks.Remove(selectedBlock);
            }
            else
            {
                _selectedBlocks.Add(selectedBlock);
                selectedBlock.IsSelected = true;

                BlockNode lastBlock = _allBlockNodes[lastIndex];
                _allBlockNodes[lastIndex] = selectedBlock;
                _allBlockNodes[_selectedBlockIndex] = lastBlock;
            }
        }

        void NodeManager_ClearAllSelectedNodes()
        {
            foreach (var item in _selectedBlocks)
            {
                item.IsSelected = false;
            }
            _selectedBlocks.Clear();
        }
        #endregion
    }

}