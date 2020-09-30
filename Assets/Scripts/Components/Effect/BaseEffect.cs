namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum BaseEffectType { Instant, OverTime }

    [System.Serializable]
    public abstract class BaseEffect
    {
        public abstract BaseEffectType Type { get; }

        public abstract bool ExecuteEffect();
    }
   

}