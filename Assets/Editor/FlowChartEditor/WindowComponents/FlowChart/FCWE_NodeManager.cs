namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        //Gets triggered whenever NodeManager_CreateNewBlock() gets called since genericmenu doesnt allow creation of a new class type within the function
        bool _newBlockAdded;

        List<BlockNode> _allBlocks;
        HashSet<BlockNode> _selectedBlocks;


        private void NodeManager_OnEnable()
        {
            _allBlocks = new List<BlockNode>();
            _selectedBlocks = new HashSet<BlockNode>();
            _newBlockAdded = false;
        }

        private void NodeManager_OnDisable()
        {

        }


        private void NodeManager_OnGUI()
        {

            //Idk why but i cant create a new instance of custom class inside of Update/InspectorUpdate/Genric Menu callbac. So im addign it here
            Event e = Event.current;

            if (_newBlockAdded)
            {
                _newBlockAdded = false;
                BlockNode b = new BlockNode(e.mousePosition);
                _allBlocks.Add(b);
            }

            NodeManager_Draw();

            NodeManager_ProcessEvent();


        }

        void NodeManager_Draw()
        {
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                _allBlocks[i].Draw();
            }
        }

        void NodeManager_ProcessEvent()
        {

            switch (Event.current.type)
            {
                case EventType.MouseUp:



                    break;

                case EventType.MouseDown:
                    for (int i = 0; i < _allBlocks.Count; i++)
                    {
                        if (_allBlocks[i].ProcessMouseDown())
                        {
                            NodeManager_ToggleBlockSelection(_allBlocks[i]);
                            break;
                        }
                    }
                    break;

            }

        }




        //================= BLOCK FUNCTIONS =================
        void NodeManager_CreateNewBlock()
        {
            _newBlockAdded = true;
        }

        void NodeManager_ToggleBlockSelection(BlockNode block)
        {
            if (_selectedBlocks.Contains(block))
            {
                _selectedBlocks.Remove(block);
            Debug.Log($"Total number of selected blocks {_selectedBlocks.Count}");
                return;
            }

            _selectedBlocks.Add(block);
            Debug.Log($"Total number of selected blocks {_selectedBlocks.Count}");
        }


    }

}