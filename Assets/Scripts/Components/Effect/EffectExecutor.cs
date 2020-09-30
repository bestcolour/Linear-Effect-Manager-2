namespace LinearEffects
{
    using System.Collections.Generic;
    using UnityEngine;

    //The EffectExecutor is by default assumed to have an ExecuteEffect function which can be completed
    //in a single frame call
    // public abstract class EffectExecutor<T> : MonoBehaviour where T : Effect, new()
    public abstract class EffectExecutor<T> : BaseEffectExecutor
     where T : Effect, new()
    {

        //=============================FOR RUN TIME==============================
        protected abstract bool ExecuteEffect(T effectData);

        [SerializeField]
        protected List<T> _list = new List<T>();

        public override void ExecuteEffectAtIndex(int index)
        {
#if UNITY_EDITOR
            Debug.Assert(index >= 0, $"Name of EffectExecutor is {this.GetType().ToString()} Index passed in is {index}");
#endif
            ExecuteEffect(_list[index]);
        }


        //===================FOR EDITOR TIME=======================
#if UNITY_EDITOR
        public override void EditorUse_AddEffect()
        {
            T newEffectData = new T();
            _list.Add(newEffectData);
        }

        //needs to update the block due to list changing
        public override void EditorUse_RemoveEffectAt(int i, Block caller)
        {
            int lastIndex = _list.Count - 1;
            T lastEffect = _list[lastIndex];
            _list[i] = lastEffect;
            _list.RemoveAt(lastIndex);
        }
#endif


    }

}