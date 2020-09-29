﻿namespace LinearCommandsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearCommands;

    //The bottom half class will render the current observed command as well as the command toolbar (add,minus coppy etc)
    public class BlockEditor_BottomHalf
    {
        #region Cached Variables
        SerializedObject serializedObject = default;

        Vector2 _scrollPosition = default;
        #endregion


        #region LifeTime Method
        public void OnEnable(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
        }


        public void OnDisable()
        {
            serializedObject = null;
        }


        public void OnInspectorGUI(float inspectorWidth)
        {
            DrawToolBar();
            DrawObservedCommand(inspectorWidth);

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

            //================DRAW ADD COPY & DELETE BUTTONS===============
            // EditorGUILayout.LabelField(string.Empty);


            if (GUILayout.Button("【＋】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {

            }
            else if (GUILayout.Button("【❏】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {

            }
            else if (GUILayout.Button("【╳】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {

            }


            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Observed Command

        void DrawObservedCommand(float inspectorWidth)
        {
            Color prevColor = GUIExtensions.Start_GUI_ColourChange(Color.grey);
            GUILayout.Box(string.Empty, GUILayout.Height(50f),GUILayout.MaxWidth(inspectorWidth));
            GUIExtensions.End_GUI_ColourChange(prevColor);

        }

        #endregion

    }

}