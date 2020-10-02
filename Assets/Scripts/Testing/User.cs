using System.Collections.Generic;
using UnityEngine;
using System;

public class User : ArrayUser<User.OrderClassExample, Holder, HolderObject>
{
    [System.Serializable]
    public class OrderClassExample : OrderData<Holder, HolderObject>
    {

    }

    [SerializeField]
    int _targetIndex = 0;

    public void RemoveAt()
    {
        base.RemoveAt(_targetIndex);
    }

    public void Insert()
    {
        base.Insert(_targetIndex);
    }
}
