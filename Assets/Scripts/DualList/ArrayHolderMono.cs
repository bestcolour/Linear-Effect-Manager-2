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

        public delegate void ChangeObjectArrayCallBack(int objectIndex);
        // public event ChangeObjectArrayCallBack OnRemoveObject = null;
        // public event ChangeObjectArrayCallBack OnInsertNewObject = null;

        #region Subscribing to EventList
        HashSet<ChangeObjectArrayCallBack> OnRemoveObjectHashset = new HashSet<ChangeObjectArrayCallBack>();
        HashSet<ChangeObjectArrayCallBack> OnInsertObjectHashset = new HashSet<ChangeObjectArrayCallBack>();

        ///<Summary>Adds event to a Hashset of delegates to be called when an element of the holder array is removed</Summary>
        public void SubToOnRemove(ChangeObjectArrayCallBack arrayCallBack) => SubToEventHashset(OnRemoveObjectHashset, arrayCallBack);
        ///<Summary>Adds event to a Hashset of delegates to be called when a new element inserted into the holder array</Summary>
        public void SubToOnInsert(ChangeObjectArrayCallBack arrayCallBack) => SubToEventHashset(OnInsertObjectHashset, arrayCallBack);

        public void UnSubFromOnRemove(ChangeObjectArrayCallBack arrayCallBack) => UnSubFromEventHashset(OnRemoveObjectHashset, arrayCallBack);
        public void UnSubFromOnInsert(ChangeObjectArrayCallBack arrayCallBack) => UnSubFromEventHashset(OnInsertObjectHashset, arrayCallBack);

        public void ClearAllSubs()
        {
            OnRemoveObjectHashset.Clear();
            OnInsertObjectHashset.Clear();
        }

        protected virtual void SubToEventHashset(HashSet<ChangeObjectArrayCallBack> eventHashset, ChangeObjectArrayCallBack callback)
        {
            //Dont allow to sub if array callback is null or hashset alrdy contains it
            if (callback == null)
            {
                return;
            }
            if (eventHashset.Contains(callback))
            {
                return;
            }
            eventHashset.Add(callback);
        }

        protected virtual void UnSubFromEventHashset(HashSet<ChangeObjectArrayCallBack> eventHashset, ChangeObjectArrayCallBack callback)
        {
            //Dont allow to sub if array callback is null or hashset alrdy contains it
            if (callback == null)
            {
                return;
            }
            if (!eventHashset.Contains(callback))
            {
                Debug.Log("Failed to unsub");
                return;
            }
            eventHashset.Remove(callback);
        }

        protected virtual void InvokeEventHashset(HashSet<ChangeObjectArrayCallBack> eventHashset, int index)
        {
            foreach (var item in eventHashset)
            {
                item.Invoke(index);
            }
        }



        #endregion

        //Although we do not care if DataUser class is inserting a new orderclass, we still want to call the event to update all the necessary order instances
        public int AddNewObject(bool isInsert)
        {
            object[] objectArray = DataArrayObject;
            Type dataType = objectArray.GetType().GetElementType();

            int elementIndex = ArrayExtension.AddReturn(ref objectArray, Activator.CreateInstance(dataType));

            DataArrayObject = objectArray;
            if (isInsert)
            {
                // OnInsertNewObject?.Invoke(elementIndex);
                InvokeEventHashset(OnInsertObjectHashset, elementIndex);
            }

            return elementIndex;
        }

        public void RemoveObjectAt(int index)
        {

            object[] objectArray = DataArrayObject;

            ArrayExtension.RemoveAt(ref objectArray, index);
            DataArrayObject = objectArray;

            // OnRemoveObject?.Invoke(index);

            InvokeEventHashset(OnRemoveObjectHashset, index);
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