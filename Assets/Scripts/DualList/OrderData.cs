﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class OrderData<Holder, Data>
 where Holder : ArrayHolder<Data>
 where Data : new()
{
    [SerializeField]
    Holder _refHolder;
    [SerializeField]
    int _dataElementIndex;


    //All these functions are used during unity editor time to manage the Holder's array as well as the OrderData itself
    //none of these will be used in the actual build except for the variables stored
#if UNITY_EDITOR
    public virtual void Initialize(Holder holder, bool isInsert)
    {
        _refHolder = holder;
        _dataElementIndex = _refHolder.AddNewObject(isInsert);
        _refHolder.OnRemoveObject += HandleRemoveObject;
        _refHolder.OnInsertObject += HandleInsertObject;
    }

    //To be called before removing the order intsance from the list
    public virtual void OnRemove()
    {
        _refHolder.RemoveObjectAt(_dataElementIndex);
    }


    #region Handle Event
    //Compares with the removed object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
    protected virtual void HandleRemoveObject(int removedIndex)
    {
        if (_dataElementIndex > removedIndex)
        {
            _dataElementIndex--;
        }
    }

    //Compares with the inserted object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
    protected virtual void HandleInsertObject(int insertedIndex)
    {
        if (_dataElementIndex > insertedIndex)
        {
            _dataElementIndex++;
        }
    }

    #endregion
#endif

}


