﻿namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public partial class Block
    {
        [Serializable]
        public partial class EffectOrder
#if UNITY_EDITOR
        : ISavableData
#endif
        {
            [SerializeField]
            protected BaseEffectExecutor _refHolder;
            [SerializeField]
            protected int _dataElmtIndex;

            ///<summary>Calls the Executor to execute the effect code. Returns true when effect is finished. Outs a bool which determines whether or not this effect will halt the code flow at this effect when the Block is being executed in an update loop and only continue when this effect is finished</summary>
            public bool CallEffect(out bool haltCodeFlow)
            {
                return _refHolder.ExecuteEffectAtIndex(_dataElmtIndex, out haltCodeFlow);
            }

        }
    }

}