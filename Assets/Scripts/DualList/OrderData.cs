﻿namespace DualList
{
    using UnityEngine;
    [System.Serializable]
    public abstract class OrderData<Holder>
    where Holder : ArrayHolderMono
    {
        [SerializeField]
        protected Holder _refHolder;
        [SerializeField]
        protected int _dataElmtIndex;

#if UNITY_EDITOR
        //All these functions are used during unity editor time to manage the Holder's array as well as the OrderData itself
        //none of these will be used in the actual build except for the variables stored
        public virtual void Initialize(Holder holder, bool isInsert)
        {
            _refHolder = holder;
            _dataElmtIndex = _refHolder.AddNewObject(isInsert);
            _refHolder.OnRemoveObject += HandleRemoveObject;
            _refHolder.OnInsertObject += HandleInsertObject;
        }

        //To be called before removing the order intsance from the list
        public virtual void OnRemove()
        {
            _refHolder.RemoveObjectAt(_dataElmtIndex);
        }

        //To be called before removing the order intsance from the list
        public virtual void OnInsert()
        {
            _refHolder.CopyArrayElementsTo();
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



}