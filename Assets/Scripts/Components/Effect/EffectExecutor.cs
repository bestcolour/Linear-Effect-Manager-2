namespace LinearEffects
{
    using System.Collections.Generic;
    using UnityEngine;

    //The EffectExecutor is by default assumed to have an ExecuteEffect function which can be completed
    //in a single frame call
    ///<Summary>A base effectexecutor which will finish its effect in a single frame</Summary>
    public abstract partial class EffectExecutor<T> : BaseEffectExecutor
    where T : Effect, new()
    {
        //=============================FOR RUN TIME==============================
        [SerializeField]
        protected T[] _effectDatas = new T[0];

        ///<Summary>Returns true when effect has completed its execution.</Summary>
        protected abstract bool ExecuteEffect(T effectData);

        public override bool ExecuteEffectAtIndex(int index)
        {
#if UNITY_EDITOR
            Debug.Assert(index >= 0, $"Name of EffectExecutor is {this.GetType().ToString()} Index passed in is {index}");
#endif
            return ExecuteEffect(_effectDatas[index]);
        }


    }

}