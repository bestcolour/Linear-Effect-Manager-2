using System.Collections.Generic;
using UnityEngine;
using LinearEffects;
using System;

public class User : MonoBehaviour
{
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
        public void OnRemove()
        {
            _refHolder.RemoveObjectAt(_holderElementIndex);
        }


        #region Handle Event
        //Compares with the removed object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
        private void HandleRemoveObject(int removedIndex)
        {
            if (_holderElementIndex > removedIndex)
            {
                _holderElementIndex--;
            }
        }

        //Compares with the inserted object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
        private void HandleInsertObject(int insertedIndex)
        {
            if (_holderElementIndex > insertedIndex)
            {
                _holderElementIndex++;
            }
        }

        #endregion
    }


    [SerializeField]
    int _targetIndex = 0;

    [SerializeField]
    OrderClass[] _order = default;



    #region Editor Commands

    public void Add()
    {
        ArrayExtension.Add(ref _order, GetOrderClass_ForAdd());
    }

    public void RemoveAt()
    {
        if (_targetIndex >= _order.Length) return;

        _order[_targetIndex].OnRemove();
        ArrayExtension.RemoveAt(ref _order, _targetIndex);
    }

    //I assume this is for copy pasting
    public void Insert()
    {
        if (_targetIndex > _order.Length) return;

        OrderClass newOrderClass = GetOrderClass_ForInsert();
        ArrayExtension.Insert(ref _order, _targetIndex, newOrderClass);
    }


    #endregion



    #region Supporting
    OrderClass GetOrderClass_ForAdd() => GetOrderClass(false);
    OrderClass GetOrderClass_ForInsert() => GetOrderClass(true);

    OrderClass GetOrderClass(bool isForInsert)
    {
        Holder holder = GetComponent<Holder>();
        return new OrderClass(holder, isForInsert);
    }
    #endregion








}
