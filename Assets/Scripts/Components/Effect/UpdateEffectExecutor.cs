namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //The UpdateEffectExecutor is assumed to have an ExecuteEffect function which needs multiple frame to complete
    ///<Summary>A base effectexecutor which will finish its effect in a more than single frame</Summary>
    public abstract class UpdateEffectExecutor<T> : EffectExecutor<T>
    where T : UpdateEffect, new()
    {
        public override bool ExecuteEffectAtIndex(int index, out bool haltCodeFlow)
        {
            UpdateEffect effect = _effectDatas[index];
            haltCodeFlow = effect.HaltUntilFinished;
            return ExecuteEffect(_effectDatas[index]);
        }
    }

}