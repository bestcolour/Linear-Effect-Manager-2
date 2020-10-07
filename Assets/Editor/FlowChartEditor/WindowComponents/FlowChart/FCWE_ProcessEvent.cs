namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using UnityEditor;

    public partial class FlowChartWindowEditor : EditorWindow
    {

        #region  Events
        delegate void DragCallback(Vector2 mouseDelta);
        static event DragCallback OnPan = null;

        //Is called when mouse is clicked in a area which is not covered by: Toolbar
        static event Action OnLeftMouseDownInGraph = null;
        static event Action OnLeftMouseUpInGraph = null;
        static event DragCallback OnMouseDrag = null;


        #endregion


        bool _isPanning;


        void ProcessEvent_OnEnable()
        {
            _isPanning = false;
            ProcessEvent_InitializeNodeMenu();

        }

        void ProcessEvent_OnDisable()
        {
            OnPan = null;
            OnMouseDrag = null;
            OnLeftMouseUpInGraph = null;
        }


        void ProcessEvent_OnGUI()
        {
            Event e = Event.current;

            switch (e.type)
            {
                case EventType.Repaint | EventType.Layout:
                    return;


                //======================== MOUSE DOWN ============================
                case EventType.MouseDown:

                    if (_toolBarRect.Contains(e.mousePosition, true))
                    {
                        return;
                    }

                    switch (e.button)
                    {
                        //========== MOUSE DOWN - LEFTCLICK =================
                        case 0:
                            if (e.alt)
                            {
                                _isPanning = true;
                                return;
                            }

                            OnLeftMouseDownInGraph?.Invoke();
                            break;

                        //========== MOUSE DOWN - RIGHTCLICK =================
                        case 1:

                            _nodeMenu.ShowAsContext();
                            break;

                        //No intention of calling other mouse clicks
                        default: return;
                    }
                    break;

                //======================== MOUSE UP ============================
                case EventType.MouseUp:
                    if (_toolBarRect.Contains(e.mousePosition, true))
                    {
                        return;
                    }

                    if (_isPanning)
                    {
                        _isPanning = false;
                        return;
                    }

                    //Else invoke the event of mouseup inside of the graph
                    OnLeftMouseUpInGraph?.Invoke();
                    break;

                //======================== MOUSE DRAG ============================
                case EventType.MouseDrag:
                    if (_isPanning)
                    {
                        OnPan?.Invoke(e.delta * 0.5f);
                        e.Use();
                        return;
                    }

                    //Else it is likely that user is attempting to drag
                    if (OnMouseDrag != null)
                    {
                        OnMouseDrag.Invoke(e.delta);
                        e.Use();
                    }
                    break;


            }



        }


        #region  Node Menu

        GenericMenu _nodeMenu = null;

        void ProcessEvent_InitializeNodeMenu()
        {
            _nodeMenu = new GenericMenu();
            _nodeMenu.AddItem(new GUIContent("New Block"), false, () => NodeManager_TriggerCreateNewBlock(AddNewBlockFrom.ContextMenu));
        }
        #endregion
    }

}