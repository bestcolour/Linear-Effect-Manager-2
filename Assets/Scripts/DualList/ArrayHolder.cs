namespace DualList
{
    using UnityEngine;

    //Please ensure that T has the System.Serializable attribute
    //To get the array you want, call _array
    [System.Serializable]
    public abstract class ArrayHolder<Data> : IArrayHolder
    where Data : new()
    {
        [SerializeField]
        protected Data[] _array = new Data[0];

#if UNITY_EDITOR
        event ChangeObjectArrayCallBack _onRemoveObject = null;
        event ChangeObjectArrayCallBack _onInsertObject = null;

        public event ChangeObjectArrayCallBack OnRemoveObject
        {
            add { _onRemoveObject += new ChangeObjectArrayCallBack(value); }
            remove { _onRemoveObject += new ChangeObjectArrayCallBack(value); }
        }
        public event ChangeObjectArrayCallBack OnInsertObject
        {
            add { _onInsertObject += new ChangeObjectArrayCallBack(value); }
            remove { _onInsertObject += new ChangeObjectArrayCallBack(value); }
        }

        //Although we do not care if DataUser class is inserting a new orderclass, we still want to call the event to update all the necessary order instances
        public int AddNewObject(bool isInsert)
        {
            int elementIndex = ArrayExtension.AddReturn(ref _array, new Data());

            if (isInsert)
            {
                _onInsertObject?.Invoke(elementIndex);
            }

            return elementIndex;
        }

        public void RemoveObjectAt(int index)
        {
            ArrayExtension.RemoveAt(ref _array, index);
            _onRemoveObject?.Invoke(index);
        }
#endif
    }
}