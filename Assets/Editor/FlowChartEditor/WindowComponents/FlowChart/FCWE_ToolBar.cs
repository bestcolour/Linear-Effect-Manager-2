namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public partial class FlowChartWindowEditor : EditorWindow
    {

        const float TOOLBAR_HEIGHT = 30f;
        static readonly Vector2 BUTTONSIZE = new Vector2(20f, 20f);

        Rect _toolBarRect;

        public void ToolBar_OnEnable()
        {
            _toolBarRect = new Rect();
            _toolBarRect.height = TOOLBAR_HEIGHT;

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
            if (GUI.Button(rect, "＋"))
            {
                ProcessEvent_NewBlock();
            }

            //================== DRAW COPY BUTTON ======================
            rect.size = BUTTONSIZE;
            rect.x += BUTTONSIZE.x + 5f;
            if (GUI.Button(rect, "❏"))
            {

            }

            //================== DRAW DELETE BUTTON ======================
            rect.size = BUTTONSIZE;
            rect.x += BUTTONSIZE.x + 5f;
            if (GUI.Button(rect, "X"))
            {

            }


            rect.size = BUTTONSIZE;
            rect.x = position.width - BUTTONSIZE.x - 5f;
            if (GUI.Button(rect, "˄"))
            {
                //Hide toolbar
            }


            GUIExtensions.End_GUI_ColourChange(prevColor);
        }





    }

}