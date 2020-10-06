namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using UnityEditor;

    public partial class FlowChartWindowEditor : EditorWindow
    {

        #region  Events
        delegate void PanCallback(Vector2 mouseDelta);
        static event PanCallback OnPan = null;
        #endregion


        bool _isPanning;


        void ProcessEvent_OnEnable()
        {
            _isPanning = false;
            // ProcessEvent_InitializeNodeMenu();

        }

        void ProcessEvent_OnDisable()
        {
            OnPan = null;
        }


        void ProcessEvents()
        {
            Event e = Event.current;

            switch (e.type)
            {
                case EventType.Repaint | EventType.Layout:
                    return;


                //======================== MOUSE DOWN ============================
                case EventType.MouseDown:

                    switch (e.button)
                    {
                        //========== MOUSE DOWN - LEFTCLICK =================
                        case 0:

                            if (e.alt)
                            {
                                _isPanning = true;
                                return;
                            }

                            Debug.Log($"Mouse Position is : {e.mousePosition}");
                            break;

                        //========== MOUSE DOWN - RIGHTCLICK =================
                        case 1:

                            // _nodeMenu.ShowAsContext();
                            break;

                        //No intention of calling other mouse clicks
                        default: return;
                    }
                    break;

                //======================== MOUSE UP ============================
                case EventType.MouseUp:
                    if (_isPanning)
                    {
                        _isPanning = false;
                        return;
                    }

                    break;

                //======================== MOUSE DRAG ============================
                case EventType.MouseDrag:
                    if (_isPanning)
                    {
                        OnPan?.Invoke(e.delta);
                        e.Use();
                    }
                    break;





            }



        }

        #region Node Menu

        // GenericMenu _nodeMenu = null;

        // void ProcessEvent_InitializeNodeMenu()
        // {
        //     _nodeMenu = new GenericMenu();
        //     _nodeMenu.AddItem(new GUIContent("New Block"), false, () => ProcessEvent_NewBlock());
        // }

        //================= NODE MENU FUNCTIONS =================
        void ProcessEvent_NewBlock()
        {
            BlockNode b = new BlockNode();
            Debug.Log($"Is b null? Check here:{b == null}");
        }



        #endregion



    }

}