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

        ///<Summary>The method which will be called to execute whatever code you want. Set the returns to true when effect has completed its execution.</Summary>
        protected abstract bool ExecuteEffect(T effectData, out bool haltCodeFlow);

        ///<Summary>Called by Block's EffectOrder during a block's execute effects call. Returns true when the effect being executed is complete</Summary>
        public override bool ExecuteEffectAtIndex(int index, out bool haltCodeFlow)
        {
#if UNITY_EDITOR
            Debug.Assert(index >= 0, $"Name of EffectExecutor is {this.GetType().ToString()} Index passed in is {index}");
#endif
            return ExecuteEffect(_effectDatas[index],out haltCodeFlow);
        }


    }

}