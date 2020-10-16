namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using LinearEffects;

    //The bottom half class will render the current observed command as well as the command toolbar (add,minus coppy etc)
    public partial class BlockInspector : Editor
    {
        GameObject BlockGameObject => _target.BlockGameObject;

        #region LifeTime Method
        void BottomHalf_OnEnable()
        {

        }

        void BottomHalf_OnDisable()
        {

        }

        void BottomHalf_OnInspectorGUI()
        {
            DrawToolBar();
            DrawObservedEffect(Screen.width);

        }
        #endregion


        #region ToolBar

        const float BUTTON_SIZE = 35f;

        void DrawToolBar()
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

            //================ DRAW ADD ===============
            if (GUILayout.Button("【＋】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                OpenEffectSearchBar();
            }
            //================ DRAW COPY=========================
            else if (GUILayout.Button("【❏】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                if (!CommandData.TryGetExecutor("TestUpdateExecutor", out Type type))
                {
                    return;
                }
                // _target.Editor_AddEffect(type);S

            }
            //=================== DRAW DELETE ===================
            else if (GUILayout.Button("【╳】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                // _target.Editor_RemoveEffectOrder(0);
                DeleteAllSelectedEffects();
            }


            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Commands
        void OpenEffectSearchBar()
        {
            if (!CommandData.TryGetExecutor("DebuggerExecutor", out Type type))
            {
                return;
            }
            _target.Block.OrderElement_Add(BlockGameObject, type);
            _target.SaveModifiedProperties();
        }

        void DeleteAllSelectedEffects()
        {
            foreach (var index in _selectedElements)
            {
                _target.Block.OrderElement_RemoveAt(index);
            }
            _selectedElements.Clear();
            
            _target.SaveModifiedProperties();
        }

        #endregion

        #region Observed Effect

        void DrawObservedEffect(float inspectorWidth)
        {
            Color prevColor = GUIExtensions.Start_GUI_ColourChange(Color.grey);
            GUILayout.Box(string.Empty, GUILayout.Height(50f), GUILayout.MaxWidth(inspectorWidth));
            GUIExtensions.End_GUI_ColourChange(prevColor);

        }




        #endregion

    }

}