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
        ///<Summary>
        ///Returns the index of the newly added effect element
        ///</Summary>
        public abstract int EditorUse_AddNewEffectEntry();

        ///<Summary>
        ///Removes the index from the effect list. Returns the ints
        ///</Summary>
        public abstract void EditorUse_RemoveEffectAt(int i);
#endif
    }

}