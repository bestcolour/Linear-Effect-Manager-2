using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearEffects;

public class Holder : MonoBehaviour
{
    [SerializeField]
    HolderObject[] _array = default;


    public delegate void ChangeObjectArrayCallBack(int objectIndex);
    public event ChangeObjectArrayCallBack OnRemoveObject = null;
    public event ChangeObjectArrayCallBack OnInsertObject = null;


    //Although we do not care if User class is inserting a new orderclass, we still want to call the event to update all the necessary order instances
    public int AddNewObject(bool isInsert)
    {
        int elementIndex = ArrayExtension.AddReturn(ref _array, new HolderObject());

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

}
