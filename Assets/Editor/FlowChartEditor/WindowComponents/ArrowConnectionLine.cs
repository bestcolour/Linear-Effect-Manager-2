namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    [System.Serializable]
    public class ArrowConnectionLine
    {
        #region Constants
        const float LINE_WIDTH = 5f
        , LINE_TRIANGLE_HALF_WIDTH = 5
        , LINE_TRAINGLE_HALF_HEIGHT = 5
        , LINE_TRAINGLE_THICKNESS = 7.5f
        ;
        static readonly Color LIGHT_THEME_CONNECTIONLINE_COLOUR = Color.black;
        static readonly Color DARK_THEME_CONNECTIONLINE_COLOUR = Color.white;
        #endregion


        public void Draw(Vector2 startPoint, Vector2 endPoint)
        {
            Color prevColour;
            prevColour = Handles.color;
            Handles.color = GetConnectionLineColour();

            Handles.BeginGUI();

            //=========== DRAW LINE =============
            Handles.DrawAAPolyLine(LINE_WIDTH, startPoint, endPoint);


            //=========== DRAW TRIANGLE =============
            Vector2 centerPoint = (startPoint + endPoint) * 0.5f;
            Vector2 dir = (endPoint - startPoint).normalized;
            Vector2 triangleTopPoint = centerPoint + dir * LINE_TRAINGLE_HALF_HEIGHT;



            Vector2 orthonganalDir = Vector2.Perpendicular(dir);

            Vector2 triangleBaseCenter = centerPoint + (-dir * LINE_TRAINGLE_HALF_HEIGHT);
            Vector2 triangleBottomLeftPoint = triangleBaseCenter + (orthonganalDir * LINE_TRIANGLE_HALF_WIDTH);
            Vector2 triangleBottomrightPoint = triangleBaseCenter + (-orthonganalDir * LINE_TRIANGLE_HALF_WIDTH);
            Handles.DrawAAPolyLine(LINE_TRAINGLE_THICKNESS, triangleTopPoint, triangleBottomLeftPoint, triangleBottomrightPoint, triangleTopPoint);


            // #region Debug
            // Handles.color = Color.red;

            // Handles.DrawAAPolyLine(LINE_WIDTH, triangleTopPoint, centerPoint);

            // #endregion


            Handles.EndGUI();
            Handles.color = prevColour;
        }


        static Color GetConnectionLineColour()
        {
            return !EditorGUIUtility.isProSkin ? LIGHT_THEME_CONNECTIONLINE_COLOUR : DARK_THEME_CONNECTIONLINE_COLOUR;
        }

    }

}