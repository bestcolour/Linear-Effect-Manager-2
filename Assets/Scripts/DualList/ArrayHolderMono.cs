namespace DualList
{
    using UnityEngine;
    using System;

    //Please ensure that T has the System.Serializable attribute
    [System.Serializable]
    public abstract class ArrayHolderMono : MonoBehaviour
    {

#if UNITY_EDITOR
        protected abstract object[] DataArrayObject { get; set; }

        public delegate void ChangeObjectArrayCallBack(int objectIndex);
        public event ChangeObjectArrayCallBack OnRemoveObject = null;
        public event ChangeObjectArrayCallBack OnInsertNewObject = null;


        //Although we do not care if DataUser class is inserting a new orderclass, we still want to call the event to update all the necessary order instances
        public int AddNewObject(bool isInsert)
        {
            object[] objectArray = DataArrayObject;
            Type dataType = objectArray.GetType().GetElementType();

            // Data[] dataArray = (Data[])DataArrayObject;
            int elementIndex = ArrayExtension.AddReturn(ref objectArray, Activator.CreateInstance(dataType));

            DataArrayObject = objectArray;
            if (isInsert)
            {
                OnInsertNewObject?.Invoke(elementIndex);
            }

            return elementIndex;
        }

        public void RemoveObjectAt(int index)
        {
            object[] objectArray = DataArrayObject;

            ArrayExtension.RemoveAt(ref objectArray, index);
            DataArrayObject = objectArray;

            OnRemoveObject?.Invoke(index);
        }

        public void DuplicateDataElement(int dataToCopyFrom)
        {
            //Duplicates a copy of the class using reflection's deep copy 
            //Adds the class to the end of the array
        }



#endif
    }
}