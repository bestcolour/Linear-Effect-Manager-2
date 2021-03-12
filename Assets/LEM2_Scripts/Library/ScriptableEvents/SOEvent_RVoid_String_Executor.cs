namespace LinearEffects.ScriptableEvents
{
    using ScriptableObjectEvents;
    [System.Serializable]
    ///<Summary>Raises a scriptableobject event which returns void and have a int parameter with the value inputed in inspector</Summary>
    public class SOEvent_RVoid_String_Executor : EffectExecutor<SOEvent_RVoid_String_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : Effect
        {
            public SOEvent_RVoid_String VoidEvent = default;
            public string Value = default;
        }

        protected override bool ExecuteEffect(MyEffect effectData)
        {
            effectData.VoidEvent.RaiseEvent(effectData.Value);
            return true;
        }

    }

}