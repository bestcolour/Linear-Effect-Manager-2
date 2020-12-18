namespace LinearEffects
{
    using UnityEngine;

    [System.Serializable]
    public abstract partial class BaseEffectExecutor : MonoBehaviour
    {
        ///<Summary>
        ///Returns true when effect has completed its execution.
        ///</Summary>
        public abstract bool ExecuteEffectAtIndex(int index);



    }

}