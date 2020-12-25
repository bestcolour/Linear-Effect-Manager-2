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
        ///<Summary>The method called before calling ExecuteEffect method. Then number of times this method is called depends on the number of times you decide to play the block this effect is on. Add your reset methods here for your effect classes.</Summary>
        protected abstract void BeginExecuteEffect(T effectData);

        ///<Summary>The method called when ExecuteEffect method is finally finished updating. (The frame in which ExecuteEffect returns true)</Summary>
        protected abstract void EndExecuteEffect(T effectData);

        public override bool ExecuteEffectAtIndex(int index, out bool haltCodeFlow)
        {
            UpdateEffect effect = _effectDatas[index];
            haltCodeFlow = effect.HaltUntilFinished;

            if (!effect.FirstFrameCall)
            {
                effect.FirstFrameCall = true;
                BeginExecuteEffect(_effectDatas[index]);
            }

            //When effect has finally over
            if (ExecuteEffect(_effectDatas[index]))
            {
                effect.FirstFrameCall = false;
                EndExecuteEffect(_effectDatas[index]);
                return true;
            }

            return false;
        }
    }

}