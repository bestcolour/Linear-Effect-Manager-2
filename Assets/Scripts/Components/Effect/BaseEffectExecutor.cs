namespace LinearEffects
{
    using DualList;

    [System.Serializable]
    public abstract class BaseEffectExecutor : ArrayHolderMono
    {
        ///<Summary>
        ///Returns true when effect has completed its execution.
        ///</Summary>
        public abstract bool ExecuteEffectAtIndex(int index);

    }

}