using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class OrderData<Holder, Data>
 where Holder : ArrayHolder<Data>
 where Data : new()
{
    protected abstract Holder RefHolder { get; }
    [SerializeField]
    int _dataElmtIndex;


    //All these functions are used during unity editor time to manage the Holder's array as well as the OrderData itself
    //none of these will be used in the actual build except for the variables stored
#if UNITY_EDITOR
    public virtual void Initialize(Holder holder, bool isInsert)
    {
        // RefHolder = holder;
        _dataElmtIndex = RefHolder.AddNewObject(isInsert);
        RefHolder.OnRemoveObject += HandleRemoveObject;
        RefHolder.OnInsertObject += HandleInsertObject;
    }

    //To be called before removing the order intsance from the list
    public virtual void OnRemove()
    {
        RefHolder.RemoveObjectAt(_dataElmtIndex);
    }


    #region Handle Event
    //Compares with the removed object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
    protected virtual void HandleRemoveObject(int removedIndex)
    {
        if (_dataElmtIndex > removedIndex)
        {
            _dataElmtIndex--;
        }
    }

    //Compares with the inserted object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
    protected virtual void HandleInsertObject(int insertedIndex)
    {
        if (_dataElmtIndex > insertedIndex)
        {
            _dataElmtIndex++;
        }
    }

    #endregion
#endif

}


