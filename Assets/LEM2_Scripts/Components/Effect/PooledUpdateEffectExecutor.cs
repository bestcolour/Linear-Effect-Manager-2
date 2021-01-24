﻿namespace LinearEffects
{
    using System.Collections.Generic;
    using UnityEngine;

    ///<Summary>This class takes in a T generic type which inherits from UpdateEffectWithRuntimeData class and a RuntimeData type which will be pooled during runtime so that effect calls can be more optimised by reusing RuntimeData objects</Summary>
    public abstract class PooledUpdateEffectExecutor<Effect, RuntimeData> : EffectExecutor<Effect>
    where Effect : UpdateEffectWithRuntimeData<RuntimeData>, new()
    where RuntimeData : class, new()
    {
        List<RuntimeData> _runtimePool = new List<RuntimeData>();

        public override bool ExecuteEffectAtIndex(int index, out bool haltCodeFlow)
        {
            UpdateEffect effect = _effectDatas[index];
            haltCodeFlow = effect.HaltUntilFinished;

            if (!effect.FirstFrameCall)
            {
                effect.FirstFrameCall = true;
                BeginExecuteEffect(_effectDatas[index]);
            }

            if (ExecuteEffect(_effectDatas[index]))
            {
                //When effect has finally over
                effect.FirstFrameCall = false;
                EndExecuteEffect(_effectDatas[index]);
                return true;
            }

            return false;
        }


        ///<Summary>Removes the runtime data reference from the UpdateEffect and returns it to the pool so that other effects can reuse the runtime data.</Summary>
        protected virtual void EndExecuteEffect(Effect t)
        {
            ReturnRuntimeData(t.RuntimeData);
            t.RuntimeData = null;
        }

        ///<Summary>Adds a runtime data instance to an UpdateEffect instance. Always call this method's base before writing your overrides</Summary>
        protected virtual void BeginExecuteEffect(Effect t)
        {
            t.RuntimeData = GetRuntimeData();
        }

        #region Pool Functions
        RuntimeData GetRuntimeData()
        {
            RuntimeData runtime;
// #if UNITY_EDITOR
            // Debug.Log($"Runtime pool count: {_runtimePool.Count} ", this);
// #endif
            if (_runtimePool.Count > 0)
            {
                int lastIndex = _runtimePool.Count - 1;
                runtime = _runtimePool[lastIndex];
                _runtimePool.RemoveAt(lastIndex);
                return runtime;
            }

            runtime = new RuntimeData();
            return runtime;
        }

        void ReturnRuntimeData(RuntimeData runtime)
        {
            _runtimePool.Add(runtime);
            // #if UNITY_EDITOR
            // Debug.Log($"Runtime pool count: {_runtimePool.Count} ", this);
// #endif
        }
        #endregion

    }

}