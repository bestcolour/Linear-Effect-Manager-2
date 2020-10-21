namespace DualList
{
    using UnityEngine;
    using System;
    //This is the monobehaviour version of the ArrayUser class. There is also basic class version where ArrayUser does not inherit from anything except for new()
    [Serializable]
    public abstract class ArrayUserMono<OData, BaseHolderClass> : MonoBehaviour
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

        #region Add New Order Data
        public virtual void AddNewOrderElement(Type type)
        {
            if (!type.IsSubclassOf(typeof(BaseHolderClass)))
            {
                Debug.Log($"Type {type} does not inherit from {typeof(BaseHolderClass)} and therefore adding this type to the OrderData is not possible!");
                return;
            }

            ArrayExtension.Add(ref _orderArray, GetNewOrderData(type, false));
        }

        public virtual void InsertNewOrderElement(Type type, int index)
        {
            if (!type.IsSubclassOf(typeof(BaseHolderClass)))
            {
                Debug.Log($"Type {type} does not inherit from {typeof(BaseHolderClass)} and therefore adding this type to the OrderData is not possible!");
                return;
            }

            if (index > _orderArray.Length) return;

            OData newOrderClass = GetNewOrderData(type, true);
            ArrayExtension.Insert(ref _orderArray, index, newOrderClass);
        }

        //Since this class is not deriving from a monobehaviour, we need to pass in the reference of the gameobject this class is being serialized on
        protected virtual OData GetNewOrderData(Type typeOfHolder, bool isForInsert)
        {
            if (!gameObject.TryGetComponent(typeOfHolder, out Component component))
            {
                component = gameObject.AddComponent(typeOfHolder);
            }

            BaseHolderClass holder = component.GetComponent<BaseHolderClass>();
            OData o = new OData();
            o.OnAddNew(holder, isForInsert);
            return o;
        }
        #endregion

        public virtual void RemoveOrderElementAt(int index)
        {
            if (index >= _orderArray.Length) return;

            _orderArray[index].OnRemove();
            ArrayExtension.RemoveAt(ref _orderArray, index);
        }

        //I assume this is for copy/cut pasting
        public virtual void InsertOrderElement(Type holderType, OData orderData, int index)
        {
            if (index > _orderArray.Length) return;

            if (!holderType.IsSubclassOf(typeof(BaseHolderClass)))
            {
                Debug.Log($"Type {holderType} does not inherit from {typeof(BaseHolderClass)} and therefore adding this type to the OrderData is not possible!");
                return;
            }

            if (!gameObject.TryGetComponent(holderType, out Component component))
            {
                component = gameObject.AddComponent(holderType);
                BaseHolderClass holder = component.GetComponent<BaseHolderClass>();
                orderData.OnInsertCopy(holder);
                ArrayExtension.Insert(ref _orderArray, index, orderData);
                return;
            }

            orderData.OnInsertCopy();
            ArrayExtension.Insert(ref _orderArray, index, orderData);
        }

        #endregion
#endif

    }



}