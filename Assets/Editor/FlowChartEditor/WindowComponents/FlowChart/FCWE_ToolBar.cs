namespace LinearEffectsEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        const string TOOLBAR_BUTTONSYMBOL_ADD = "＋"
        , TOOLBAR_BUTTONSYMBOL_COPY = "❏"
        , TOOLBAR_BUTTONSYMBOL_DELETE = "X"
        ;

        const float TOOLBAR_HEIGHT = 30f;
        static readonly Vector2 BUTTONSIZE = new Vector2(20f, 20f);

        Rect _toolBarRect;
        // bool _toolBarHidden;

        public void ToolBar_OnEnable()
        {
            _toolBarRect = new Rect();
            _toolBarRect.height = TOOLBAR_HEIGHT;

            // _toolBarHidden = false;
        }

        public void ToolBar_OnDisable()
        {

        }


        public void ToolBar_OnGUI()
        {
            Color prevColor = GUIExtensions.Start_GUI_ColourChange(Color.white);
            Rect rect = _toolBarRect;


            //======================= DRAW TOOLBAR ========================
            _toolBarRect.width = position.width;
            GUI.Box(_toolBarRect, string.Empty);

            rect.y += 5f;

            //================== DRAW ADD BUTTON ======================
            rect.size = BUTTONSIZE;
            rect.x += 5f;
            if (GUI.Button(rect, TOOLBAR_BUTTONSYMBOL_ADD))
            {
                NodeManager_NodeCycler_TriggerCreateNewNode(AddNewBlockFrom.ToolBar);
            }

            //================== DRAW COPY BUTTON ======================
            rect.size = BUTTONSIZE;
            rect.x += BUTTONSIZE.x + 5f;
            if (GUI.Button(rect, TOOLBAR_BUTTONSYMBOL_COPY))
            {

            }

            //================== DRAW DELETE BUTTON ======================
            rect.size = BUTTONSIZE;
            rect.x += BUTTONSIZE.x + 5f;
            if (GUI.Button(rect, TOOLBAR_BUTTONSYMBOL_DELETE))
            {
                NodeManager_NodeCycler_DeleteNode();
            }

            GUIExtensions.End_GUI_ColourChange(prevColor);
        }





    }

}