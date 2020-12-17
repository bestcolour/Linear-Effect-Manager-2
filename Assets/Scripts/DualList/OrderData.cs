namespace DualList
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
        public Holder RefHolder => _refHolder;

        //All these functions are used during unity editor time to manage the Holder's array as well as the OrderData itself
        //none of these will be used in the actual build except for the variables stored
        public virtual void OnAddNew(Holder holder, bool isInsert)
        {
            _refHolder = holder;
            _dataElmtIndex = _refHolder.AddNewObject(isInsert);
            // SubscribeToEvents();
        }

        //To be called before removing the order intsance from the list
        public virtual void OnRemove()
        {
            // _refHolder.UnSubFromOnInsert(HandleInsertObject);
            // _refHolder.UnSubFromOnRemove(HandleRemoveObject);
            // _refHolder.OnRemoveObject -= HandleRemoveObject;
            // _refHolder.OnInsertNewObject -= HandleInsertObject;
            _refHolder.RemoveObjectAt(_dataElmtIndex);
        }

        //For when the holder is not null
        public virtual void OnInsertCopy()
        {
            //Tell the holder to do a copy of my current data index details and add it to the end of the array
            _dataElmtIndex = _refHolder.DuplicateDataElement(_dataElmtIndex);
            // SubscribeToEvents();
        }

        public virtual void OnInsertCopy(Holder holder)
        {
            _refHolder = holder;
            OnInsertCopy();
        }

        // public virtual void SubscribeToEvents()
        // {
        //     // _refHolder.SubToOnRemove(HandleRemoveObject);
        //     // _refHolder.SubToOnInsert(HandleInsertObject);
        //     // _refHolder.OnRemoveObject += HandleRemoveObject;
        //     // _refHolder.OnInsertNewObject += HandleInsertObject;
        // }



        // #region Handle Event
        // //Compares with the removed object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
        // protected virtual void HandleRemoveObject(int removedIndex)
        // {
        //     Debug.Log($"I am an orderdata and my elmt index is {_dataElmtIndex} on {_refHolder} and the removedIndex is {removedIndex}");
        //     if (_dataElmtIndex > removedIndex)
        //     {
        //         _dataElmtIndex--;
        //     }
        //     Debug.Log($"After doing checks, my dataElmtIndex is {_dataElmtIndex}");
        // }

        // //Compares with the inserted object's element index this.instance's element index and determine if this.instance's elemnt idnex needs updating
        // protected virtual void HandleInsertObject(int insertedIndex)
        // {
        //     if (_dataElmtIndex > insertedIndex)
        //     {
        //         _dataElmtIndex++;
        //     }
        // }

        // #endregion
#endif

    }



}