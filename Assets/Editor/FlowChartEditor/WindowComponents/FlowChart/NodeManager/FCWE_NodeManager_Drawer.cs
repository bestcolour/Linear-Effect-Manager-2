namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Statics
        static GUIStyle DebugStyle;
        static GUIContent DebugGUIContent;

        public static GUIStyle BlockNodeConnectButtonStyle { get; private set; }


        #endregion


        #region Constants
        static readonly Color SELECTIONBOX_COLOUR = new Color(.75f, .93f, .93f, 0.5f);
        #endregion



        #region Initialization
        void NodeManager_Drawer_OnEnable()
        {
            NodeManager_Drawer_InitializeMain();
            NodeManager_Drawer_InitializeDebugger();
        }

        private void NodeManager_Drawer_InitializeMain()
        {
            BlockNodeConnectButtonStyle = new GUIStyle(GUI.skin.button);
            BlockNodeConnectButtonStyle.wordWrap = true;
            BlockNodeConnectButtonStyle.alignment = TextAnchor.MiddleCenter;
        }

        void NodeManager_Drawer_InitializeDebugger()
        {
            DebugStyle = new GUIStyle();
            DebugStyle.wordWrap = true;
            DebugStyle.normal.textColor = Color.red;
            DebugGUIContent = new GUIContent();
        }
        #endregion


        void NodeManager_Drawer_OnGUI()
        {
            NodeManager_Drawer_DrawMain();
            NodeManager_Drawer_DrawDebugger();
        }


        #region Draw Methods
        void NodeManager_Drawer_DrawMain()
        {

            switch (_toolBarState)
            {
                case ToolBarState.NORMAL:
                    NodeManager_Drawer_DrawMain_ToolBarState_NORMAL();
                    break;

                case ToolBarState.ARROW:
                    NodeManager_Drawer_DrawMain_ToolBarState_ARROW();
                    break;
            }


        }

        ///<Summary>Draw whatever is supposed to be drawn during arrow mode. This includes: (block nodes with a box, their label and a button which says "Connect" which when pressed, will connect the current selected node towards that node) </Summary>
        private void NodeManager_Drawer_DrawMain_ToolBarState_ARROW()
        {
            //========== DRAW NODE BLOCKS ===========
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].DrawNodeArrowLines();
            }

            //Draw from the bottom up
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].Draw_ToolBarState_ARROW();
            }
        }

        ///<Summary>Draw whatever is supposed to be drawn during normal mode. This includes: (block nodes with just a box with their label), (arrow lines) and (selection box) </Summary>
        private void NodeManager_Drawer_DrawMain_ToolBarState_NORMAL()
        {
            //========== DRAW NODE BLOCKS ===========
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].DrawNodeArrowLines();
            }

            //Draw from the bottom up
            for (int i = 0; i < _allBlockNodes.Count; i++)
            {
                _allBlockNodes[i].Draw_ToolBarState_NORMAL();
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

        #endregion

        #region Dubugger

        void NodeManager_Drawer_DrawDebugger()
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

        #region Colour
        // static Color GetTextColour()
        // {
        // return !EditorGUIUtility.isProSkin ? LIGHT_THEME_CONNECTIONLINE_COLOUR : DARK_THEME_CONNECTIONLINE_COLOUR;
        // }
        #endregion

    }

}