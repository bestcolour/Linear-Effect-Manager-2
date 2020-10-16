namespace LinearEffects
{
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using System.Linq;
#endif

    //The EffectExecutor is by default assumed to have an ExecuteEffect function which can be completed
    //in a single frame call
    // public abstract class EffectExecutor<T> : MonoBehaviour where T : Effect, new()
    public abstract class EffectExecutor<T> : BaseEffectExecutor
    where T : Effect, new()
    {

#if UNITY_EDITOR
        protected override object[] DataArrayObject
        {
            get
            {
                return _effectDatas;
            }
            set
            {
                _effectDatas = value.Select(x => (T)x).ToArray();
            }
        }
#endif

        //=============================FOR RUN TIME==============================
        [SerializeField]
        protected T[] _effectDatas = new T[0];

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