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
        List<Block.EffectOrder> _clipBoard = default;

        #region LifeTime Method
        void BottomHalf_OnEnable()
        {
            _clipBoard = new List<Block.EffectOrder>();
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
                //If there had been previous clipboard effects and you are cutting agn,
                if (_clipBoard.Count > 0)
                {
                    BottomHalf_DeleteAllClipBoardEffects();
                }

                BottomHalf_CopySelectedToClipBoard();
            }
            //================ DRAW COPY BUTTON ===============
            else if (GUILayout.Button("【❏】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_CopySelectedToClipBoard();
            }
            //================ DRAW PASTE BUTTON =========================
            else if (GUILayout.Button("【≚】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_PasteClipBoardEffects();


            }
            //=================== DRAW DELETE BUTTON ===================
            else if (GUILayout.Button("【╳】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                // _target.Editor_RemoveEffectOrder(0);
                BottomHalf_DeleteAllSelectedEffects();
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
        void BottomHalf_PasteClipBoardEffects()
        {
            for (int i = 0; i < _clipBoard.Count; i++)
            {
                var effectOrder = _clipBoard[i];
                //1) Convert effect name to type using CommandData
                if (!CommandData.TryGetExecutor(effectOrder.EffectName, out Type effectType))
                {
                    Debug.Log($"There is no such effect executor called {effectOrder.EffectName} in CommandData.cs!");
                    continue;
                }

                //2) Call target.Block.OrderElement_Insert()
                // _target.Block.OrderElement_Insert(_target.BlockGameObject, effectType, effectOrder, CurrentClickedListIndex + 1);
                Debug.Log(effectOrder.EffectName);
            }


        }

        void BottomHalf_OpenEffectSearchBar()
        {
            if (!CommandData.TryGetExecutor("TestUpdateExecutor", out Type type))
            {
                return;
            }
            _target.Block.OrderElement_AddNew(BlockGameObject, type);
            _target.SaveModifiedProperties();
        }

        void BottomHalf_DeleteAllSelectedEffects()
        {
            if (!TopHalf_GetSelectedForLoopValues(out int diff, out int direction, out int firstClickedIndex))
            {
                return;
            }
            //Remove elements from the biggest index to the lowest index
            int startingIndex = direction > 0 ? CurrentClickedListIndex : _firstClickedIndex;

            //Remove elements from the biggest index to the lowest index
            for (int i = 0; i <= diff; i++)
            {
                int index = startingIndex - i;
                _target.Block.OrderElement_RemoveAt(index);
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

            _clipBoard.Clear();

            //Always ensure that the order of the elements copied starts from the smallest index to the largest index
            int startingIndex = direction > 0 ? _firstClickedIndex : CurrentClickedListIndex;
            for (int i = 0; i <= diff; i++)
            {
                SerializedProperty p = TopHalf_GetOrderArrayElement(startingIndex + i);
                Block.EffectOrder orderObject = new Block.EffectOrder();
                orderObject.LoadFromSerializedProperty(p);
                _clipBoard.Add(orderObject);
            }

        }

        void BottomHalf_DeleteAllClipBoardEffects()
        {
            // for (int i = 0; i < _clipBoard.Count; i++)
            // {
            //     _clipBoard[i].OnRemove();
            // }
            // _clipBoard.Clear();
        }
        #endregion



    }

}