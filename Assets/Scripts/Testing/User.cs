using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearEffects;
using System;

public class User : MonoBehaviour
{
    [System.Serializable]
    class TestingClass
    {
        public int TargetIndex = 0;
    }

    [System.Serializable]
    class OrderClass
    {
        [SerializeField]
        Holder _refHolder;
        [SerializeField]
        int _holderElementIndex;

        public OrderClass(Holder holder, bool isInsert)
        {
            _refHolder = holder;
            _holderElementIndex = _refHolder.AddNewObject(isInsert);
            _refHolder.OnRemoveObject += HandleRemoveObject;
            _refHolder.OnInsertObject += HandleInsertObject;
        }




        //To be called before removing the order intsance from the list
        public void RemoveInstance()
        {
            _refHolder.RemoveObjectAt(_holderElementIndex);
        }


        #region Handle Event
        private void HandleRemoveObject(int removedIndex)
        {
            if (_holderElementIndex > removedIndex)
            {
                _holderElementIndex--;
            }
        }

        private void HandleInsertObject(int removedIndex)
        {
            if (_holderElementIndex > removedIndex)
            {
                _holderElementIndex++;
            }
        }

        #endregion

        // //Compares a order class A with another order class B where A is the removed instance and B is being checked if it requires an update due to A's removal.
        // public void AttemptUpdateDueToRemoval(OrderClass removedInstance)
        // {
        //     //Comparing old holderelement index values 
        //     if (removedInstance._refHolder == this._refHolder && this._holderElementIndex > removedInstance._holderElementIndex)
        //     {
        //         _holderElementIndex--;
        //     }
        // }

        // //Compares a order class A with another order class B where A is the inserted instance and B is being checked if it requires an update due to A's insertion.
        // public void AttemptUpdateDueToInsertion(OrderClass insertedInstance)
        // {
        //     //Comparing old holderelement index values 
        //     if (insertedInstance._refHolder == this._refHolder && this._holderElementIndex > insertedInstance._holderElementIndex)
        //     {
        //         _holderElementIndex++;
        //     }
        // }


    }


    [SerializeField]
    TestingClass _testing = default;

    [SerializeField]
    OrderClass[] _order = default;



    #region Editor Commands

    public void Add()
    {
        ArrayExtension.Add(ref _order, GetOrderClass_ForAdd());
    }

    public void RemoveAt()
    {
        if (_testing.TargetIndex >= _order.Length) return;

        _order[_testing.TargetIndex].RemoveInstance();
        ArrayExtension.RemoveAt(ref _order, _testing.TargetIndex);

        // //We need to update all order to ensure that regardless of order,(because list is reorderable) the orderclasses which are effected gets updated
        // foreach (var item in _order)
        // {
        //     item.AttemptUpdateDueToRemoval(removedInstance);
        // }

    }

    //I assume this is for copy pasting
    public void Insert()
    {
        if (_testing.TargetIndex > _order.Length) return;

        OrderClass newOrderClass = GetOrderClass_ForInsert();
        ArrayExtension.Insert(ref _order, _testing.TargetIndex, newOrderClass);

        // //We need to update all order to ensure that regardless of order,(because list is reorderable) the orderclasses which are effected gets updated
        // foreach (var item in _order)
        // {
        //     item.AttemptUpdateDueToInsertion(newOrderClass);
        // }


    }

    //TO be called by other instances of User when one instance of User removes an order
    public void RemoveUpdate()
    {

    }


    #endregion



    #region Supporting
    OrderClass GetOrderClass_ForAdd() =>  GetOrderClass(false);
    OrderClass GetOrderClass_ForInsert() => GetOrderClass(true);

    OrderClass GetOrderClass(bool isForInsert)
    {
        Holder holder = GetComponent<Holder>();
        return new OrderClass(holder, isForInsert);
    }
    #endregion








}
