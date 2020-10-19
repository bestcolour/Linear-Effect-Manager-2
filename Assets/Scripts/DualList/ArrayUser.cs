namespace DualList
{
    using UnityEngine;
    using System;
    //This is the non-monobehaviour version of the ArrayUser class. 
    [System.Serializable]
    public abstract class ArrayUser<OData, BaseHolderClass>
    where OData : OrderData<BaseHolderClass>, new()
    where BaseHolderClass : ArrayHolderMono
    {

        ///<Summary>
        ///This array is the order in which you get your Data. For eg, let Data be a monobehaviour that stores cakes. By looping through OData, you are retrieving the cakes from the Holder class which is where the CakeData[] is being stored & serialized.
        ///</Summary>
        [SerializeField]
        protected OData[] _orderArray = new OData[0];


#if UNITY_EDITOR
        #region Editor Commands

        public virtual void OrderElement_Add(GameObject gameObject, Type type)
        {
            if (!type.IsSubclassOf(typeof(BaseHolderClass)))
            {
                Debug.Log($"Type {type} does not inherit from {typeof(BaseHolderClass)} and therefore adding this type to the OrderData is not possible!");
                return;
            }

            ArrayExtension.Add(ref _orderArray, GetOrderData_ForAdd(gameObject, type));
        }

        public virtual void OrderElement_RemoveAt(int index)
        {
            if (index >= _orderArray.Length) return;

            _orderArray[index].OnRemove();
            ArrayExtension.RemoveAt(ref _orderArray, index);
        }

        //I assume this is for copy pasting
        public virtual void OrderElement_Insert(GameObject gameObject, Type type, int index)
        {
            if (!type.IsSubclassOf(typeof(BaseHolderClass)))
            {
                Debug.Log($"Type {type} does not inherit from {typeof(BaseHolderClass)} and therefore adding this type to the OrderData is not possible!");
                return;
            }

            if (index > _orderArray.Length) return;

            OData newOrderClass = GetOrderData_ForInsert(gameObject, type);
            ArrayExtension.Insert(ref _orderArray, index, newOrderClass);
        }


        #region Get OrderData
        protected OData GetOrderData_ForAdd(GameObject gameObject, Type typeOfHolder) => GetOrderData(gameObject, typeOfHolder, false);
        protected OData GetOrderData_ForInsert(GameObject gameObject, Type typeOfHolder) => GetOrderData(gameObject, typeOfHolder, true);

        //Since this class is not deriving from a monobehaviour, we need to pass in the reference of the gameobject this class is being serialized on
        protected OData GetOrderData(GameObject gameObject, Type typeOfHolder, bool isForInsert)
        {
            if (!gameObject.TryGetComponent(typeOfHolder, out Component component))
            {
                component = gameObject.AddComponent(typeOfHolder);
            }

            BaseHolderClass holder = component.GetComponent<BaseHolderClass>();
            OData o = new OData();
            o.Initialize(holder, isForInsert);
            return o;
        }


        #endregion
        #endregion
#endif
    }
}

