﻿namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Serializable]
    ///<Summary>Sets a gameobject's active state</Summary>
    public class SetActive_Executor : EffectExecutor<SetActive_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : Effect
        {
            public GameObject Target = default;
            public bool State = false;
        }

        protected override bool ExecuteEffect(MyEffect effectData)
        {
            effectData.Target.SetActive(effectData.State);
            return true;
        }
    }

}