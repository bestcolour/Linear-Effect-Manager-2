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
        public event ChangeObjectArrayCallBack OnInsertObject = null;


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
                OnInsertObject?.Invoke(elementIndex);
            }

            return elementIndex;
        }

        public void RemoveObjectAt(int index)
        {
            // if (DataArrayObject == null)
            // {
            //     DataArrayObject = new Data[0];
            // }

            object[] objectArray = DataArrayObject;

            // Data[] dataArray = (Data[])DataArrayObject;
            ArrayExtension.RemoveAt(ref objectArray, index);
            DataArrayObject = objectArray;

            OnRemoveObject?.Invoke(index);
        }



#endif
    }
}