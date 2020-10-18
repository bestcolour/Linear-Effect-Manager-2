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
                if (!CommandData.TryGetExecutor("TestUpdateExecutor", out Type type))
                {
                    return;
                }
                // _target.Editor_AddEffect(type);S

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

            //Positive works with the for loop but negative doesnt for some reason
            //TRY TO FIX THIS ISSUE OF HAVING -VE DIR BUT NOT BEING ABLE TO DELETE ALL THE EFFECTS

            //CurrentClicked = 0
            //diff = 1
            //direction = -1
            //firstclicked = 1

            //!Should not happen!
            //Loop 1
            //index = 0

            //Should be:
            //Loop 1
            //index = 1 + -1 * 0 = 1

            //Loop 2
            //index = 1 + -1 * 1  =0

            switch (direction > 0)
            {
                case true:
                    //Remove elements from the biggest index to the lowest index
                    for (int i = diff; i >= 0; i--)
                    {
                        int index = firstClickedIndex + direction * i;
                        _target.Block.OrderElement_RemoveAt(index);
                    }
                    break;

                case false:
                    //Remove elements from the biggest index to the lowest index
                    for (int i = 0; i <= diff; i++)
                    {
                        int index = firstClickedIndex + direction * i;
                        _target.Block.OrderElement_RemoveAt(index);
                    }


                    //CurrentClicked = 0
                    //diff = 1
                    //direction = -1
                    //firstclicked = 1

                    //Loop 1:
                    // index: 1

                    //Loop 2:
                    // index: 0



                    break;
            }



            _selectedElements.Clear();
            _target.SaveModifiedProperties();
        }

        void BottomHalf_CopySelectedToClipBoard()
        {
            //as hashset does not garuntee order, i will be using index from and to ensure the copied elements are in the correct order
            if (!TopHalf_GetSelectedForLoopValues(out int diff, out int direction, out _firstClickedIndex))
            {
                return;
            }

            for (int i = 0; i <= diff; i++)
            {
                SerializedProperty p = TopHalf_GetOrderArrayElement(_firstClickedIndex + i * direction);
                Block.EffectOrder orderObject = new Block.EffectOrder();
                orderObject.LoadFromSerializedProperty(p);
                _clipBoard.Add(orderObject);
            }
        }

        #endregion



    }

}