namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using LinearEffects;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class TimerExecutor : UpdateEffectExecutor<TimerExecutor.DebuggerEffect>
    {
        [System.Serializable]
        public class DebuggerEffect : UpdateEffect
        {
            [SerializeField]
            float _duration = default;

            float _timer = -1;


            public void Reset()
            {
                _timer = _duration;
            }

            public bool TickDown()
            {
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                    return false;
                }

                return true;
            }

        }

        protected override bool ExecuteEffect(DebuggerEffect effectData)
        {
            return effectData.TickDown();
        }

        protected override void BeginExecuteEffect(DebuggerEffect effectData)
        {
            effectData.Reset();
        }

        protected override void EndExecuteEffect(DebuggerEffect effectData)
        {
        }
    }



}