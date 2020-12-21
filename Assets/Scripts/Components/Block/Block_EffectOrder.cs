namespace LinearEffects
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



        }
    }

}