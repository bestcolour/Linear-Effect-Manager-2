namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using LinearEffects;
    using System.Collections.Generic;

    //The bottom half class will render the current observed command as well as the command toolbar (add,minus coppy etc)
    public partial class BlockInspector : Editor
    {
        enum BlockCommand { None, Copy, Cut }

        GameObject BlockGameObject => _target.BlockGameObject;
        // List<Block.EffectOrder> _clipBoard = default;

        List<int> _clipBoardIndices = default;
        HashSet<int> _clipBoardUnOrderedIndices = default;

        BlockCommand _previousCommand = BlockCommand.None;

        const string DEBUG_EFFECTEXECUTOR = "TestUpdateExecutor";

        #region LifeTime Method
        void BottomHalf_OnEnable()
        {
            // _clipBoard = new List<Block.EffectOrder>();
            _clipBoardIndices = new List<int>();
            _clipBoardUnOrderedIndices = new HashSet<int>();
        }

        void BottomHalf_OnDisable()
        {

        }

        void BottomHalf_OnInspectorGUI()
        {
            BottomHalf_DrawToolBar();
            BottomHalf_DrawObservedEffect(Screen.width);

        }
        #endregion


        #region ToolBar

        const float BUTTON_SIZE = 35f;

        void BottomHalf_DrawToolBar()
        {
            EditorGUILayout.BeginHorizontal();
            // //============DRAW PARENT BOX=====================
            // EditorGUILayout.LabelField(string.Empty);

            //================DRAW NEXT/PREV COMMAND BUTTONS===============
            if (GUILayout.Button("【↑】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {

            }
            else if (GUILayout.Button("【↓】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {

            }


            //================DRAW SPACE===============
            EditorGUILayout.Space();

            //================ DRAW ADD BUTTON ===============
            if (GUILayout.Button("【＋】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_OpenEffectSearchBar();
            }
            //================ DRAW CUT BUTTON ===============
            else if (GUILayout.Button("【✂】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_CopySelectedToClipBoard();
                _previousCommand = BlockCommand.Cut;
            }
            //================ DRAW COPY BUTTON ===============
            else if (GUILayout.Button("【❏】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                //Copy will not actually copy selected element. It will only copy elements which are in the range of the firstclickedindex and currentclickedindex
                BottomHalf_CopySelectedToClipBoard();
                _previousCommand = BlockCommand.Copy;
            }
            //================ DRAW PASTE BUTTON =========================
            else if (GUILayout.Button("【≚】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_PasteClipBoardEffects();
                _previousCommand = BlockCommand.None;
            }
            //=================== DRAW DELETE BUTTON ===================
            else if (GUILayout.Button("【╳】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                if (!TopHalf_GetSelectedForLoopValues(out int diff, out int direction, out int firstClickedIndex))
                {
                    return;
                }

                //Get the bigger starting index
                int startingIndex = direction > 0 ? CurrentClickedListIndex : _firstClickedIndex;

                BottomHalf_DeleteAllSelectedEffects(startingIndex, diff);
            }


            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Observed Effect

        void BottomHalf_DrawObservedEffect(float inspectorWidth)
        {
            Color prevColor = GUIExtensions.Start_GUI_ColourChange(Color.grey);
            GUILayout.Box(string.Empty, GUILayout.Height(50f), GUILayout.MaxWidth(inspectorWidth));
            GUIExtensions.End_GUI_ColourChange(prevColor);

        }




        #endregion

        #region Commands

        #region Paste methods

        void BottomHalf_PasteClipBoardEffects()
        {
            switch (_previousCommand)
            {
                default: break;

                case BlockCommand.Copy:
                    BottomHalf_PasteFromCopyMethod();
                    break;

                case BlockCommand.Cut:
                    BottomHalf_PasteFromCutMethod();
                    break;

            }

        }

        void BottomHalf_PasteFromCopyMethod()
        {
            //Check if there is nothing selected
            int currentInsertPosition = CurrentClickedListIndex == -1 ? _list.count : CurrentClickedListIndex;

            foreach (var elementIndexWhichYouIntendToCopy in _clipBoardIndices)
            {
                if (!BottomHalf_GetCopyOfOrderObjectFromArray(elementIndexWhichYouIntendToCopy, out var effectOrder))
                {
                    Debug.Log($"Unable to copy index {elementIndexWhichYouIntendToCopy} because index is out of bounds!");
                    continue;
                }

                if (!CommandData.TryGetExecutor(effectOrder.EffectName, out Type executorType))
                {
                    Debug.Log($"The Executor {effectOrder.EffectName} doesnt exist in CommandData.cs!");
                    continue;
                }

                //Add the effectorder into the currently selected index (if there isnt any selected index on the list, add to the end)
                _target.Block.InsertOrderElement(_target.BlockGameObject, executorType, effectOrder, currentInsertPosition);
                currentInsertPosition++;
            }

            Debug.Log($"Copied the current {_clipBoardIndices[0]}th element to the {_clipBoardIndices[_clipBoardIndices.Count - 1]}th element.");

            _target.SaveModifiedProperties();
            _clipBoardIndices.Clear();
            _clipBoardUnOrderedIndices.Clear();
        }

        //More of a reorder really
        void BottomHalf_PasteFromCutMethod()
        {
            // //Check if there is nothing selected
            // int currentInsertPosition = CurrentClickedListIndex == -1 ? _list.count : CurrentClickedListIndex;

            // foreach (var elementIndexWhichYouIntendToCopy in _clipBoardIndices)
            // {
            //     if (!TopHalf_GetOrderArrayElement(elementIndexWhichYouIntendToCopy, out SerializedProperty p))
            //     {
            //         return;
            //     }

            //     //Add the effectorder into the currently selected index (if there isnt any selected index on the list, add to the end)
            //     _target.Block.InsertOrderElement(_target.BlockGameObject, executorType, effectOrder, currentInsertPosition);
            //     currentInsertPosition++;
            // }

            // Debug.Log($"Copied the current {_clipBoardIndices[0]}th element to the {_clipBoardIndices[_clipBoardIndices.Count - 1]}th element.");

            // _target.SaveModifiedProperties();


            // _clipBoardIndices.Clear();
            // _clipBoardUnOrderedIndices.Clear();
        }

        #endregion

        void BottomHalf_OpenEffectSearchBar()
        {
            if (!CommandData.TryGetExecutor(DEBUG_EFFECTEXECUTOR, out Type type))
            {
                return;
            }
            _target.Block.AddNewOrderElement(BlockGameObject, type, DEBUG_EFFECTEXECUTOR);
            _target.SaveModifiedProperties();
        }

        void BottomHalf_DeleteAllSelectedEffects(int startingIndex, int range)
        {
            //Remove elements from the biggest index to the lowest index
            for (int i = 0; i <= range; i++)
            {
                int index = startingIndex - i;
                _target.Block.RemoveOrderElementAt(index);
            }

            _selectedElements.Clear();
            TopHalf_ResetFirstClickedIndex();
            _target.SaveModifiedProperties();
        }

        void BottomHalf_CopySelectedToClipBoard()
        {
            //as hashset does not garuntee order, i will be using index from and to ensure the copied elements are in the correct order
            if (!TopHalf_GetSelectedForLoopValues(out int diff, out int direction, out _firstClickedIndex))
            {
                return;
            }

            // _clipBoard.Clear();
            _clipBoardIndices.Clear();
            _clipBoardUnOrderedIndices.Clear();

            //Always ensure that the order of the elements copied starts from the smallest index to the largest index
            int startingIndex = direction > 0 ? _firstClickedIndex : CurrentClickedListIndex;
            for (int i = 0; i <= diff; i++)
            {
                int index = startingIndex + i;
                _clipBoardIndices.Add(index);
                _clipBoardUnOrderedIndices.Add(index);
            }
        }

        bool BottomHalf_GetCopyOfOrderObjectFromArray(int index, out Block.EffectOrder orderData)
        {
            if (!TopHalf_GetOrderArrayElement(index, out SerializedProperty p))
            {
                orderData = null;
                return false;
            }


            orderData = new Block.EffectOrder();
            orderData.LoadFromSerializedProperty(p);
            return true;
        }
        #endregion



    }

}