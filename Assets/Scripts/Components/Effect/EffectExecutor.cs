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

        // [SerializeField]
        // protected List<T> _list = new List<T>();

        [SerializeField]
        protected T[] _effects = new T[0];

        public override void ExecuteEffectAtIndex(int index)
        {
#if UNITY_EDITOR
            Debug.Assert(index >= 0, $"Name of EffectExecutor is {this.GetType().ToString()} Index passed in is {index}");
#endif
            ExecuteEffect(_effects[index]);
        }


        //===================FOR EDITOR TIME=======================
#if UNITY_EDITOR
        public override int EditorUse_AddNewEffectEntry()
        {
            T newEffectData = new T();
            return ArrayExtension.AddReturn(ref _effects, newEffectData);
        }

        //needs to update the block due to list changing
        public override void EditorUse_RemoveEffectAt(int i)
        {
            ArrayExtension.RemoveAt(ref _effects, i);
        }
#endif


    }

}