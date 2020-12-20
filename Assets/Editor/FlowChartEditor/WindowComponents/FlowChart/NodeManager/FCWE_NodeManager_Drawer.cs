namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;


    public partial class FlowChartWindowEditor : EditorWindow
    {
  #region Statics
        static GUIStyle DebugStyle;
        static GUIContent DebugGUIContent;
        #endregion


        #region Constants
        static readonly Color SELECTIONBOX_COLOUR = new Color(.75f, .93f, .93f, 0.5f);
        #endregion




        void NodeManager_Drawer_OnEnable()
        {
            NodeManager_InitializeDebugger();
        }

        void NodeManager_InitializeDebugger()
        {
            DebugStyle = new GUIStyle();
            DebugStyle.wordWrap = true;
            DebugStyle.normal.textColor = Color.red;
            DebugGUIContent = new GUIContent();
        }

        void NodeManager_Drawer_OnGUI()
        {
            NodeManager_Draw();
            NodeManager_DrawDebugger();
        }


        #region Draw Methods
        void NodeManager_Draw()
        {
            //========== DRAW NODE BLOCKS ===========
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].DrawNodeArrowLines();
            }

            //Draw from the bottom up
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].DrawNodeBlocks();
            }

            //Draw Selection box
            if (_dragState == DragState.DrawSelection_HadDragged)
            {
                Event e = Event.current;
                _selectionBox.width = e.mousePosition.x - _selectionBox.x;
                _selectionBox.height = e.mousePosition.y - _selectionBox.y;

                Color prevColour = GUIExtensions.Start_GUI_ColourChange(SELECTIONBOX_COLOUR);
                GUI.Box(_selectionBox, string.Empty);
                GUIExtensions.End_GUI_ColourChange(prevColour);
            }

        }

        void NodeManager_DrawDebugger()
        {
            Rect rect = Rect.zero;
            //Abit of border
            rect.x = 5f;
            rect.y = TOOLBAR_HEIGHT;

            string debugStatement = $"Number of selected blocks: {_selectedBlocks.Count} \n Drag State: {_dragState} ";


            DebugGUIContent.text = debugStatement;
            rect.size = DebugStyle.CalcSize(DebugGUIContent);

            GUI.Label
            (
                rect,
                DebugGUIContent,
                DebugStyle
            );
        }
        #endregion


    }

}