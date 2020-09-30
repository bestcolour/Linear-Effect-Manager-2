namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;

    public class BlockEditor_CenterDivision
    {

        #region Definitions


        public delegate void DragCenterDivisionCallback(float mouseDeltaY);
        public event DragCenterDivisionCallback OnDrag = null;
        #endregion

        bool _isDragging = false;


        public void OnEnable()
        {
            _isDragging = false;
        }



        public void OnDisable()
        {
        }

        public void OnInspectorGUI(float inspectorWidth)
        {
            //==================DRAW AN EMPTY DUMMY LAYOUT BOX====================
            EditorGUILayout.LabelField(string.Empty);

            //============DRAW THE REAL SEPARATOR USING THE LAYOUT BOX'S RECT====================
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.x = 0;
            lastRect.width = inspectorWidth;
            EditorGUI.LabelField(lastRect, string.Empty, GUI.skin.horizontalSlider);


            //===================== PROCESS EVENTS ===========================
            Event e = Event.current;

            switch (_isDragging)
            {
                case false:
                    //Run logic if mouse is over the division
                    if (!lastRect.Contains(e.mousePosition))
                    {
                        return;
                    }

                    //  Change cursor
                    EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.SplitResizeUpDown);

                    //If mouse is clicked on the left click,
                    if (e.type != EventType.MouseDown || e.button != 0)
                    {
                        return;
                    }

                    _isDragging = true;
                    break;

                case true:

                    //If mouse button goes up 
                    if (e.type == EventType.MouseUp)
                    {
                        _isDragging = false;
                        return;
                    }

                    //We only care about mouse events now
                    if (!e.isMouse) return;

                    //Else if mouse is continuing the drag
                    OnDrag?.Invoke(e.delta.y);
                    break;
            }


        }
    }

}