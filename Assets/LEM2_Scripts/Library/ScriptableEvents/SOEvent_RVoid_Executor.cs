namespace LinearEffects.ScriptableEvents
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using ScriptableObjectEvents;
    [System.Serializable]
    ///<Summary>Raises a scriptableobject event which returns void and have no parameters</Summary>
    public class SOEvent_RVoid_Executor : EffectExecutor<SOEvent_RVoid_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : Effect
        {
            public SOEvent_Void VoidEvent = default;
        }


        protected override bool ExecuteEffect(MyEffect effectData)
        {
            effectData.VoidEvent.RaiseEvent();
            return true;
        }


    }

}