namespace DualList
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    //Please ensure that T has the System.Serializable attribute
    [System.Serializable]
    public abstract class ArrayHolderMono : MonoBehaviour
    {

#if UNITY_EDITOR
        protected abstract object[] DataArrayObject { get; set; }

        public delegate void ChangeObjectArrayCallBack(int objectIndex, string effectName);

        #region Subscribing to EventList
        protected ChangeObjectArrayCallBack OnRemoveObject = null, OnInsertObject = null;

        public virtual void InitializeSubs(ChangeObjectArrayCallBack onRemove, ChangeObjectArrayCallBack onInsert)
        {
            OnRemoveObject = onRemove;
            OnInsertObject = onInsert;
        }

        #endregion

        //Although we do not care if DataUser class is inserting a new orderclass, we still want to call the event to update all the necessary order instances
        public virtual int AddNewObject(bool isInsert)
        {
            object[] objectArray = DataArrayObject;
            Type dataType = objectArray.GetType().GetElementType();

            int elementIndex = ArrayExtension.AddReturn(ref objectArray, Activator.CreateInstance(dataType));

            DataArrayObject = objectArray;
            if (isInsert)
            {
                // OnInsertObject?.Invoke(elementIndex);
                // InvokeEventHashset(OnInsertObjectHashset, elementIndex);
            }

            return elementIndex;
        }

        public virtual void RemoveObjectAt(int index)
        {

            object[] objectArray = DataArrayObject;

            ArrayExtension.RemoveAt(ref objectArray, index);
            DataArrayObject = objectArray;

            // OnRemoveObject?.Invoke(index);

            // InvokeEventHashset(OnRemoveObject, index);
        }

        public int DuplicateDataElement(int index)
        {
            //Duplicates a copy of the class using reflection's deep copy 
            object[] objectArray = DataArrayObject;

            object objectToDuplicate = objectArray[index];

            object copy = ReflectionExtensions.DeepCopy(objectToDuplicate);

            int elementIndex = ArrayExtension.AddReturn(ref objectArray, copy);
            DataArrayObject = objectArray;
            return elementIndex;
        }



#endif
    }
}