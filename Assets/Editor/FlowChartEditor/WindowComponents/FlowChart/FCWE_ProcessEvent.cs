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

            if (e.type == (EventType.Repaint | EventType.Layout)) return;

            //=============== PROCESS PANNING ==================
            if (_isPanning)
            {
                OnPan?.Invoke(e.delta);

                if (e.type == EventType.MouseUp)
                {
                    _isPanning = false;
                }

                Repaint();
                return;
            }

            if (e.alt)
            {
                if (e.type == EventType.MouseDown)
                {
                    _isPanning = true;
                }

            }


            //===================== COMMANDS ==========================





        }




    }

}