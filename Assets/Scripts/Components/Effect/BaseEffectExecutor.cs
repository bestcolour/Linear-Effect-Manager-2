namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class BaseEffectExecutor : MonoBehaviour
    {
        public abstract void ExecuteEffectAtIndex(int index);


        //===================FOR EDITOR TIME=======================
#if UNITY_EDITOR
        public abstract void EditorUse_AddEffect();

        //needs to update the block due to list changing
        public abstract void EditorUse_RemoveEffectAt(int i, Block caller);
#endif
    }

}