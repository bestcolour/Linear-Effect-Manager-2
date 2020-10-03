namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DualList;

    public abstract class BaseEffectExecutor<T> : ArrayHolderMono<T>
    where T : new()
    {
        ///<Summary>
        ///Returns true when effect has completed its execution.
        ///</Summary>
        public abstract bool ExecuteEffectAtIndex(int index);


//         //===================FOR EDITOR TIME=======================
// #if UNITY_EDITOR
//         ///<Summary>
//         ///Returns the element index of the newly added effect element
//         ///</Summary>
//         public abstract int EditorUse_AddNewEffectEntry();

//         ///<Summary>
//         ///Inserted a new effect element at the given index
//         ///</Summary>
//         public abstract void EditorUse_InsertNewEffectEntry(int index);

//         ///<Summary>
//         ///Removes the element represented by the given index from the effect list.
//         ///</Summary>
//         public abstract void EditorUse_RemoveEffectAt(int i);
// #endif
    }

}