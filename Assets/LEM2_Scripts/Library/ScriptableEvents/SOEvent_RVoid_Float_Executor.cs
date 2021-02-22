namespace LinearEffects.ScriptableEvents
{
    using ScriptableObjectEvents;
    [System.Serializable]
    ///<Summary>Raises a scriptableobject event which returns void and have a float parameter with the value inputed in inspector</Summary>
    public class SOEvent_RVoid_Float_Executor : EffectExecutor<SOEvent_RVoid_Float_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : Effect
        {
            public SOEvent_RVoid_Float VoidEvent = default;
            public float Value = default;
        }

        protected override bool ExecuteEffect(MyEffect effectData)
        {
            effectData.VoidEvent.RaiseEvent(effectData.Value);
            return true;
        }

    }

}