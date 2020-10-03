using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Please ensure that T has the System.Serializable attribute
[System.Serializable]
public abstract class ArrayHolder<Data> where Data : new()
{
    [SerializeField]
    Data[] _array = new Data[0];

#if UNITY_EDITOR
    public delegate void ChangeObjectArrayCallBack(int objectIndex);
    public event ChangeObjectArrayCallBack OnRemoveObject = null;
    public event ChangeObjectArrayCallBack OnInsertObject = null;

    //Although we do not care if DataUser class is inserting a new orderclass, we still want to call the event to update all the necessary order instances
    public int AddNewObject(bool isInsert)
    {
        int elementIndex = ArrayExtension.AddReturn(ref _array, new Data());

        if (isInsert)
        {
            OnInsertObject?.Invoke(elementIndex);
        }

        return elementIndex;
    }

    public void RemoveObjectAt(int index)
    {
        ArrayExtension.RemoveAt(ref _array, index);
        OnRemoveObject?.Invoke(index);
    }
#endif
}
