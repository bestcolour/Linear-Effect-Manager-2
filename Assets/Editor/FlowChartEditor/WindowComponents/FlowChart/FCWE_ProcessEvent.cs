namespace LinearEffectsEditor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    public partial class FlowChartWindowEditor : EditorWindow
    {

        delegate void PanCallback(Vector2 mouseDelta);
        static event PanCallback OnPan = null;


        bool _isPanning = default;

        void ProcessEvent_OnEnable()
        {
            _isPanning = false;
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
                        case 0:

                            if (e.alt)
                            {
                                _isPanning = true;
                                return;
                            }

                            break;

                        case 1:


                            break;



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
                        Repaint();
                    }
                    break;





            }



        }



    }

}