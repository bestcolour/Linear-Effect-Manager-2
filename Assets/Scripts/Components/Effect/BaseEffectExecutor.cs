namespace LinearEffects
{
    using System.Collections.Generic;
    using System;
    using DualList;
using UnityEngine;

    [System.Serializable]
    public abstract class BaseEffectExecutor : ArrayHolderMono
    {


        ///<Summary>
        ///Returns true when effect has completed its execution.
        ///</Summary>
        public abstract bool ExecuteEffectAtIndex(int index);


#if UNITY_EDITOR
        //This is stored here because EffectExecutor class is a generic class
        public const string PROPERTYNAME_EFFECTDATAS = "_effectDatas";


        public override int AddNewObject(bool isInsert)
        {
            object[] objectArray = DataArrayObject;
            Type dataType = objectArray.GetType().GetElementType();
            string fullName = dataType.FullName;
            Debug.Log(fullName);

            int elementIndex = ArrayExtension.AddReturn(ref objectArray, Activator.CreateInstance(dataType));

            DataArrayObject = objectArray;
            if (isInsert)
            {
                OnInsertObject?.Invoke(elementIndex, fullName);
            }

            return elementIndex;
        }

        public override void RemoveObjectAt(int index)
        {
            object[] objectArray = DataArrayObject;

            ArrayExtension.RemoveAt(ref objectArray, index);
            DataArrayObject = objectArray;
            Type dataType = objectArray.GetType().GetElementType();
            string fullName = dataType.ToString();


            OnRemoveObject?.Invoke(index,fullName);
        }

#endif

    }

}