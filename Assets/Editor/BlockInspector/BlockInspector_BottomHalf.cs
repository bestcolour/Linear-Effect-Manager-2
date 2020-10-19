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
                BottomHalf_CopySelectedToClipBoard();
                BottomHalf_DeleteAllSelectedEffects();
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
            foreach (var item in _clipBoard)
            {
                //1) Convert effect name to type using CommandData
                //2) Call target.Block.OrderElement_Insert()
                Debug.Log(item.EffectName);
            }


        }

        void BottomHalf_OpenEffectSearchBar()
        {
            if (!CommandData.TryGetExecutor("DebuggerExecutor", out Type type))
            {
                return;
            }
            _target.Block.OrderElement_Add(BlockGameObject, type);
            _target.SaveModifiedProperties();
        }

        void BottomHalf_DeleteAllSelectedEffects()
        {
            //1 -1 1
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

        #endregion



    }

}