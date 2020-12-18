#if UNITY_EDITOR
namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public abstract partial class BaseEffectExecutor
    {

        //This is stored here because EffectExecutor class is a generic class
        public const string PROPERTYNAME_EFFECTDATAS = "_effectDatas";



        public delegate void ChangeObjectArrayCallBack(int objectIndex, string effectName);

        #region Subscribing to EventList
        protected ChangeObjectArrayCallBack OnRemoveObject = null, OnInsertObject = null;

        public virtual void InitializeSubs(ChangeObjectArrayCallBack onRemove, ChangeObjectArrayCallBack onInsert)
        {
            OnRemoveObject = onRemove;
            OnInsertObject = onInsert;
        }
        #endregion

        public abstract int AddNewObject(bool isInsert);

        public abstract void RemoveObjectAt(int index);

        public abstract int DuplicateDataElement(int index);


        // public override int AddNewObject(bool isInsert)
        // {
        //     object[] objectArray = DataArrayObject;
        //     Type dataType = objectArray.GetType().GetElementType();


        //     int elementIndex = ArrayExtension.AddReturn(ref objectArray, Activator.CreateInstance(dataType));

        //     DataArrayObject = objectArray;
        //     if (isInsert)
        //     {
        //         //Getting baseEffector's effectname
        //         Type thisType = GetType();
        //         string effectName = thisType.Name;
        //         OnInsertObject?.Invoke(elementIndex, effectName);
        //     }

        //     return elementIndex;
        // }

        // public override void RemoveObjectAt(int index)
        // {
        //     object[] objectArray = DataArrayObject;

        //     ArrayExtension.RemoveAt(ref objectArray, index);
        //     DataArrayObject = objectArray;
        //     Type dataType = objectArray.GetType().GetElementType();

        //     //Getting baseEffector's effectname
        //     Type thisType = GetType();
        //     string effectName = thisType.Name;
        //     OnRemoveObject?.Invoke(index, effectName);
        // }

    }

}
#endif