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
        GameObject BlockGameObject => _target.BlockGameObject;

        #region ClipBoard Fields
        List<int> _clipBoardIndices = default;
        HashSet<int> _clipBoardUnOrderedIndices = default;

        bool _prevlyCopied = false;
        #endregion

        SerializedProperty _currObservedProperty = default;

        #region Constants
        const string DEBUG_EFFECTEXECUTOR = "TestUpdateExecutor";
        const float OBSERVED_EFFECTBG_BORDER = 50f,
        OBSERVED_EFFECT_YOFFSET = 20f
        ;
        // static readonly Color OBSERVED_EFFECT_BOXCOLOUR = new Color(73, 112, 177) / 255;
        #endregion

        #region LifeTime Method
        void BottomHalf_OnEnable()
        {
            _prevlyCopied = false;
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
            // //================ DRAW CUT BUTTON ===============
            // else if (GUILayout.Button("【✂】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            // {
            //     BottomHalf_CopySelectedToClipBoard();
            //     _previousCommand = BlockCommand.Cut;
            // }
            //================ DRAW COPY BUTTON ===============
            else if (GUILayout.Button("【❏】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                //Copy will not actually copy selected element. It will only copy elements which are in the range of the firstclickedindex and currentclickedindex
                BottomHalf_CopySelectedToClipBoard();
                _prevlyCopied = true;
            }
            //================ DRAW PASTE BUTTON =========================
            else if (GUILayout.Button("【≚】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_PasteClipBoardEffects();
                _prevlyCopied = false;
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
            //Get currently selected order element
            if (!TopHalf_GetOrderArrayElement(CurrentClickedListIndex, out SerializedProperty orderElement))
            {
                return;
            }

            //===== GETTING OBSERVED EFFECT =====
            //convert holder to serializedobject
            BaseEffectExecutor holder = (BaseEffectExecutor)orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_REFHOLDER).objectReferenceValue;
            SerializedObject holderObject = new SerializedObject(holder);

            //Get the effectDatas array as serializedProperty
            SerializedProperty effectDataArray = holderObject.FindProperty(BaseEffectExecutor.PROPERTYNAME_EFFECTDATAS);

            //Get dataelementindex from orderElement in block
            int dataElementIndex = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_DATAELEMENTINDEX).intValue;

            //Get current selected effect through the use of the EffectData array and dataelementindex
            _currObservedProperty = effectDataArray.GetArrayElementAtIndex(dataElementIndex);

            //======== DRAWING EFFECT ==========
            float height = EditorGUI.GetPropertyHeight(_currObservedProperty);

            // ========== DRAWING BG BOX =============
            // Color prevColor = GUIExtensions.Start_GUI_ColourChange(OBSERVED_EFFECT_BOXCOLOUR);
            GUILayout.Box(string.Empty, GUILayout.Height(height + OBSERVED_EFFECTBG_BORDER), GUILayout.MaxWidth(inspectorWidth));
            // GUIExtensions.End_GUI_ColourChange(prevColor);

            //========== DRAWING EFFECT =============
            Rect prevRect = GUILayoutUtility.GetLastRect();
            prevRect.y += OBSERVED_EFFECT_YOFFSET;
            EditorGUI.PropertyField(prevRect, _currObservedProperty, true);

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