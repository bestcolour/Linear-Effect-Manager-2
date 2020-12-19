namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    [System.Serializable]
    public class ConnectionLine
    {
        #region Constants
        const float LINE_WIDTH = 5f;
        static readonly Color LIGHT_THEME_CONNECTIONLINE_COLOUR = Color.black;
        static readonly Color DARK_THEME_CONNECTIONLINE_COLOUR = Color.white;
        #endregion


        public void Draw(Vector2 startPoint, Vector2 endPoint)
        {
            Color prevColour;
            prevColour = Handles.color;
            Handles.color = GetConnectionLineColour();
            Handles.DrawAAPolyLine(LINE_WIDTH, startPoint, endPoint);
            Handles.color = prevColour;
        }


        static Color GetConnectionLineColour()
        {
            return !EditorGUIUtility.isProSkin ? LIGHT_THEME_CONNECTIONLINE_COLOUR : DARK_THEME_CONNECTIONLINE_COLOUR;
        }

    }

}