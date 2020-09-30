namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class BaseEffectExecutor<T> : MonoBehaviour where T : BaseEffect
    {
        // public abstract List<T> Effects { get; }

        [SerializeField]
        List<T> _effects = default;
    }

}