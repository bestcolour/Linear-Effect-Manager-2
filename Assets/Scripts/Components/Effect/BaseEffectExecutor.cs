namespace LinearEffects
{
    using DualList;

    [System.Serializable]
    public abstract class BaseEffectExecutor<T> : ArrayHolderMono<T>
        where T : new()
    {
        ///<Summary>
        ///Returns true when effect has completed its execution.
        ///</Summary>
        public abstract bool ExecuteEffectAtIndex(int index);
      
    }

}