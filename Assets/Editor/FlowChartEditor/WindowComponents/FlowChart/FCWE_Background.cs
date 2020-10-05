namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        const int GRID_SMALL_SPACE = 10;
        const int GRID_LARGE_SPACE = 50;

        Vector3 _background_Offset;

        void Background_OnEnable()
        {
            OnPan += Background_HandlePan;
            _background_Offset = Vector3.zero;
        }

        void Background_OnDisable()
        {
            OnPan -= Background_HandlePan;
        }

        void Background_HandlePan(Vector2 mouseDelta)
        {
            mouseDelta *= 0.3f;
            _background_Offset.x += mouseDelta.x;
            _background_Offset.y += mouseDelta.y;

        }


        void Background_OnGUI()
        {
            Background_DrawGrid(GRID_SMALL_SPACE, GetGrid1Colour());
            Background_DrawGrid(GRID_LARGE_SPACE, GetGrid2Colour());
        }

        #region Draw
        void Background_DrawGrid(float gridspacing, Color lineColour)
        {
            Handles.BeginGUI();
            // Color grid1Colour = GetGrid1Colour(), grid2Colour = GetGrid2Colour();
            Color prevColour = GUIExtensions.Start_Handles_ColourChange(lineColour);

            //=======================DRAW HORIZONTAL LINES===========================
            //Divide the total amount of offset by gridspacing. if remainder is 0, that means we dont need to draw new lines cause the canvas moved the exactly the same distance as gridspacing multiplied by a factor. So it is as if we didnt move the canvas however, if remainder is not zero, we have an offset value of ranging from 0 < value < gridspacing with that offset, we can draw lines at a new position inbetween the usual grid lines positions
            Vector3 adjustedOffset = _background_Offset;
            adjustedOffset.x %= gridspacing;
            adjustedOffset.y %= gridspacing;

            //Ensure that startV & endV is at least one Gridspace behind the screen's actual starting point
            Vector3 startV = Vector3.left * gridspacing, endV = Vector3.right * (position.width + gridspacing);
            int numberOfLines = Mathf.CeilToInt((position.height / gridspacing));

            for (int i = 0; i < numberOfLines; i++)
            {
                startV.y = endV.y = gridspacing * i;
                Handles.DrawLine(startV + adjustedOffset, endV + adjustedOffset);
            }

            startV = Vector3.down * gridspacing;
            endV = Vector3.up * (position.height + gridspacing);
            numberOfLines = Mathf.CeilToInt((position.width / gridspacing));

            for (int i = 0; i < numberOfLines; i++)
            {
                startV.x = endV.x = gridspacing * i;
                Handles.DrawLine(startV + adjustedOffset, endV + adjustedOffset);
            }

            // int numberOfLines = Mathf.CeilToInt((position.height / GRID_SPACE) * 0.2f) + 1;
            // for (int i = 1, totalLinesDrawn = 0; i < numberOfLines; i++)
            // {
            //     //Draw the first 4 lines
            //     for (int g = 1; g < 5; g++)
            //     {
            //         startV.y = endV.y = GRID_SPACE * (g + totalLinesDrawn);
            //         Handles.DrawLine(startV + adjustedOffset, endV + adjustedOffset);
            //     }

            //     //Draw the fifth line
            //     startV.y = endV.y = GRID_SPACE * (5 + totalLinesDrawn);


            //     totalLinesDrawn += 5;
            //     Handles.color = grid2Colour;
            //     Handles.DrawLine(startV + adjustedOffset, endV + adjustedOffset);
            //     Handles.color = grid1Colour;
            // }

            // //Ensure that startV & endV is at least one Gridspace behind the screen's actual starting point
            // startV = Vector3.down * GRID_SPACE;
            // endV = Vector3.up * (position.height + GRID_SPACE);
            // numberOfLines = Mathf.CeilToInt((position.width / GRID_SPACE) * 0.2f) + 1;
            // for (int i = 1, totalLinesDrawn = 0; i < numberOfLines; i++)
            // {
            //     //Draw the first 4 lines
            //     for (int g = 1; g < 5; g++)
            //     {
            //         startV.x = endV.x = GRID_SPACE * (g + totalLinesDrawn);
            //         Handles.DrawLine(startV + adjustedOffset, endV + adjustedOffset);
            //     }

            //     //Draw the fifth line
            //     startV.x = endV.x = GRID_SPACE * (5 + totalLinesDrawn);


            //     totalLinesDrawn += 5;
            //     Handles.color = grid2Colour;
            //     Handles.DrawLine(startV + adjustedOffset, endV + adjustedOffset);
            //     Handles.color = grid1Colour;
            // }

            GUIExtensions.End_GUI_ColourChange(prevColour);
            Handles.EndGUI();
        }
        #endregion


        #region Colours
        //https://www.rapidtables.com/web/color/gray-color.html
        static readonly Color LIGHT_GRIDLINE1_COLOUR = new Color(0.66f, 0.66f, 0.66f, 0.5f);
        static readonly Color LIGHT_GRIDLINE2_COLOUR = new Color(0.41f, 0.41f, 0.41f, 0.5f);

        static readonly Color DARK_GRIDLINE1_COLOUR = new Color(0.41f, 0.41f, 0.41f, 0.5f);
        static readonly Color DARK_GRIDLINE2_COLOUR = new Color(0.75f, 0.75f, 0.75f, 0.5f);

        Color GetGrid1Colour()
        {
            return !EditorGUIUtility.isProSkin ? LIGHT_GRIDLINE1_COLOUR : DARK_GRIDLINE1_COLOUR;
        }

        Color GetGrid2Colour()
        {
            return !EditorGUIUtility.isProSkin ? LIGHT_GRIDLINE2_COLOUR : DARK_GRIDLINE2_COLOUR;
        }
        #endregion


    }

}