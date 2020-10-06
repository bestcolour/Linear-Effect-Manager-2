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


        private void NodeManager_OnEnable()
        {
            _allBlocks = new List<BlockNode>();
            _selectedBlocks = new HashSet<BlockNode>();
            _newBlockFromEnum = AddNewBlockFrom.None;
            OnPan += NodeManager_HandlePan;
            OnLeftMouseDownInGraph += NodeManager_HandleLeftMouseDownInGraph;

        }



        private void NodeManager_OnDisable()
        {
            OnLeftMouseDownInGraph -= NodeManager_HandleLeftMouseDownInGraph;
            OnPan -= NodeManager_HandlePan;
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
            NodeManager_ProcessEvent();


        }

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

        void NodeManager_ProcessEvent()
        {
            // Event e = Event.current;

            // switch (Event.current.type)
            // {
            //     //================== MOUSE UP ======================
            //     case EventType.MouseUp:



            //         break;


            //     //================== MOUSE DOWN ======================
            //     case EventType.MouseDown:


            //         break;

            // }

        }

        #region Event Handlers
        private void NodeManager_HandlePan(Vector2 mouseDelta)
        {
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                _allBlocks[i].ProcessMouseDrag(mouseDelta);
            }
        }

        private void NodeManager_HandleLeftMouseDownInGraph()
        {
            Event e = Event.current;
            //Return if panning
            if (e.alt) return;

            //Else if no shift key is pressed,
            _selectedBlocks.Clear();

            //Since selected block will usally be at the last index of _allBlock, looping it from 0 to count will ensure that all other blocks gets a chance to consider itself as the newly selected block before the previous selected block
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                if (_allBlocks[i].ProcessMouseDown())
                {
                    NodeManager_SelectBlock(i);
                }
            }


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

        void NodeManager_SelectBlock(int i)
        {
            _selectedBlocks.Add(_allBlocks[i]);

            //Send the selected block to the end of the list so that it will be rendered on top
            int lastIndex = _allBlocks.Count - 1;
            BlockNode selectedBlock = _allBlocks[i], lastBlock = _allBlocks[lastIndex];

            _allBlocks[lastIndex] = selectedBlock;
            _allBlocks[i] = lastBlock;
        }

        void NodeManager_ToggleBlockSelection(BlockNode block)
        {
            if (_selectedBlocks.Contains(block))
            {
                _selectedBlocks.Remove(block);
                return;
            }

            _selectedBlocks.Add(block);
        }





    }

}