namespace LinearEffects
{
    using DualList;

    [System.Serializable]
    public abstract class BaseEffectExecutor : ArrayHolderMono
    {
#if UNITY_EDITOR
        //This is stored here because EffectExecutor class is a generic class
        public const string PROPERTYNAME_EFFECTDATAS = "_effectDatas";

#endif

        ///<Summary>
        ///Returns true when effect has completed its execution.
        ///</Summary>
        public abstract bool ExecuteEffectAtIndex(int index);

    }

}