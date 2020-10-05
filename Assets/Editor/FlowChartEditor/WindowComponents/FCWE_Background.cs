namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public partial class FlowChartWindowEditor : EditorWindow
    {
        //https://www.rapidtables.com/web/color/gray-color.html
        static readonly Color GRIDLINE1_LIGHT_COLOUR = new Color(0.66f, 0.66f, 0.66f);
        static readonly Color GRIDLINE2_LIGHT_COLOUR = new Color(.41f, .41f, .41f);

        static readonly Color GRIDLINE1_DARK_COLOUR = new Color(0.66f, 0.66f, 0.66f);
        static readonly Color GRIDLINE2_DARK_COLOUR = new Color(0.66f, 0.66f, 0.66f);


        Color GetGrid1Colour()
        {
            return !EditorGUIUtility.isProSkin ? GRIDLINE1_LIGHT_COLOUR : GRIDLINE1_LIGHT_COLOUR;
        }

        Color GetGrid2Colour()
        {
            return !EditorGUIUtility.isProSkin ? GRIDLINE2_LIGHT_COLOUR : GRIDLINE2_LIGHT_COLOUR;
        }

        #region Draw
        void Background_Draw()
        {
            Vector3 startV = Vector3.zero, endV = Vector3.right * Screen.width;
            int numberOfLines = Screen.height / 10;

            //=======================DRAW HORIZONTAL LINES===========================
            Color grid1Colour = GetGrid1Colour(), grid2Colour = GetGrid2Colour();
            Color prevColour = GUIExtensions.Start_Handles_ColourChange(grid1Colour);
            for (int i = 0; i < numberOfLines / 5; i++)
            {
                for (int g = 0; g < 4; g++)
                {
                    startV.y += 10;
                    endV.y += 10;
                    Handles.DrawLine(startV, endV);

                }

                startV.y += 10;
                endV.y += 10;

                Handles.color = grid2Colour;
                Handles.DrawLine(startV, endV);
                Handles.color = grid1Colour;

            }

            // //=======================DRAW VERTICAL LINES===========================
            // startV = Vector3.zero;
            // endV = Screen.height * Vector3.up;
            // numberOfLines = Screen.width / 10;
            // for (int i = 0; i < numberOfLines; i++)
            // {
            //     for (int g = 0; g < 4; g++)
            //     {
            //         startV.x += 10;
            //         endV.x += 10;
            //         Handles.DrawLine(startV, endV);

            //     }

            //     startV.x += 10;
            //     endV.x += 10;

            //     Handles.color = grid2Colour;
            //     Handles.DrawLine(startV, endV);
            //     Handles.color = grid1Colour;
            // }

            GUIExtensions.End_GUI_ColourChange(prevColour);


        }
        #endregion
    }

}