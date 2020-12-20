namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;

    [System.Serializable]
    public class ArrowConnectionLine
    {
        public delegate void RemoveArrowConnectionLineCallback(string from, string to);

        #region Constants
        const float LINE_WIDTH = 5f
        , LINE_TRIANGLE_HALF_WIDTH = 5
        , LINE_TRAINGLE_HALF_HEIGHT = 5
        , LINE_REMOVEBUTTON_SIZE = 5f
        , LINE_REMOVEBUTTON_PICKSIZE = 7f
        , LINE_TRAINGLE_THICKNESS = 7.5f
        ;

        static readonly Color LIGHT_THEME_CONNECTIONLINE_COLOUR = Color.black;
        static readonly Color DARK_THEME_CONNECTIONLINE_COLOUR = Color.white;
        static readonly Vector2 REMOVEBUTTON_SIZE = new Vector2(50f, 50f);
        #endregion


        #region Cached Variables

        public BlockNode StartNode { get; private set; }
        public BlockNode EndNode { get; private set; }

        Color _lineColour = default;

        ///<Summary>The delegate assigned wil be called when the Remove button is pressed to remove a arrow connection line which is connecting from this block towards the block with the ConnectedTowardsBlockName</Summary>
        RemoveArrowConnectionLineCallback onRemove;

        #endregion

        public ArrowConnectionLine(BlockNode start, BlockNode end, RemoveArrowConnectionLineCallback onRemove)
        {
            this.onRemove = onRemove;
            StartNode = start;
            EndNode = end;
            UpdateConnectionLineColour(EditorGUIUtility.isProSkin);
            FlowChartWindowEditor.OnEditorSkinChange += UpdateConnectionLineColour;
        }

        ~ArrowConnectionLine()
        {
            FlowChartWindowEditor.OnEditorSkinChange -= UpdateConnectionLineColour;
        }

        public void Draw()
        {
            Color prevColour;
            prevColour = Handles.color;
            Handles.color = _lineColour;

            Handles.BeginGUI();

            //=========== DRAW LINE =============
            Handles.DrawAAPolyLine(LINE_WIDTH, StartNode.OutConnectionPoint, EndNode.InConnectionPoint);
            // Handles.DrawDottedLine(StartNode.Center, EndNode.Center,LINE_WIDTH);
            DrawTriangleArrow();
            // DrawRemoveButton();

            Handles.EndGUI();
            Handles.color = prevColour;
        }

        //         private void DrawRemoveButton()
        //         {
        //             Vector2 centerPoint = (StartNode.Center + EndNode.Center) * 0.5f;
        //             Rect rect = new Rect(centerPoint, REMOVEBUTTON_SIZE);

        // Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap)
        //             if (GUI.Button(rect, "Remove"))
        //             {

        //             }
        //         }

        void DrawTriangleArrow()
        {
            //Instruction to visualise: Draw a unit circle and draw a line to any direction inside that circle.
            //The centerPoint will be the center of that line 
            //Imagine a triangle being drawn at that center pointing towards the circle's edges from the origin of the unit circle
            //=========== DRAW TRIANGLE =============

            //------- Top Point ---------
            Vector2 centerPoint = (StartNode.Center + EndNode.Center) * 0.5f;
            // Vector2 dir = (EndNode.Center - StartNode.Center).normalized;
            // Vector2 triangleTopPoint = centerPoint + dir * LINE_TRAINGLE_HALF_HEIGHT;


            // Vector2 orthonganalDir = Vector2.Perpendicular(dir);

            // Vector2 triangleBaseCenter = centerPoint + (-dir * LINE_TRAINGLE_HALF_HEIGHT);
            // Vector2 triangleBottomLeftPoint = triangleBaseCenter + (orthonganalDir * LINE_TRIANGLE_HALF_WIDTH);
            // Vector2 triangleBottomrightPoint = triangleBaseCenter + (-orthonganalDir * LINE_TRIANGLE_HALF_WIDTH);

            if (Handles.Button(centerPoint, Quaternion.identity, LINE_REMOVEBUTTON_SIZE, LINE_REMOVEBUTTON_PICKSIZE, Handles.RectangleHandleCap))
            {
                onRemove?.Invoke(StartNode.Label, EndNode.Label);
            }
            // Handles.DrawAAPolyLine(LINE_TRAINGLE_THICKNESS, triangleTopPoint, triangleBottomLeftPoint, triangleBottomrightPoint, triangleTopPoint);
        }


        void UpdateConnectionLineColour(bool isDark)
        {
            _lineColour = !isDark ? LIGHT_THEME_CONNECTIONLINE_COLOUR : DARK_THEME_CONNECTIONLINE_COLOUR;
            // _lineColour = !EditorGUIUtility.isProSkin ? LIGHT_THEME_CONNECTIONLINE_COLOUR : DARK_THEME_CONNECTIONLINE_COLOUR;
        }

    }

}