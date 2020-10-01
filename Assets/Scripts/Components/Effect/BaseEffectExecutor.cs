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
        ///Returns the element index of the newly added effect element
        ///</Summary>
        public abstract int EditorUse_AddNewEffectEntry();

        ///<Summary>
        ///Removes the element represented by the given index from the effect list.
        ///</Summary>
        public abstract void EditorUse_RemoveEffectAt(int i);
#endif
    }

}