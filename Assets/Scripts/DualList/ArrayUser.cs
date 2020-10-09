namespace DualList
{
    using UnityEngine;
#if UNITY_EDITOR
    //This is the non-monobehaviour version of the ArrayUser class. 
    [System.Serializable]
    public class ArrayUser<OData, Holder, Data>
    where OData : OrderData<Holder>, new()
    where Holder : IArrayHolder
    where Data : new()
    {

        ///<Summary>
        ///This array is the order in which you get your Data. For eg, let Data be a monobehaviour that stores cakes. By looping through OData, you are retrieving the cakes from the Holder class which is where the CakeData[] is being stored & serialized.
        ///</Summary>
        [SerializeField]
        OData[] _orderArray = new OData[0];

        #region Editor Commands

        public void Add(GameObject gameObject)
        {
            ArrayExtension.Add(ref _orderArray, GetOrderData_ForAdd(gameObject));
        }

        public void RemoveAt(int index)
        {
            if (index >= _orderArray.Length) return;

            _orderArray[index].OnRemove();
            ArrayExtension.RemoveAt(ref _orderArray, index);
        }

        //I assume this is for copy pasting
        public void Insert(GameObject gameObject, int index)
        {
            if (index > _orderArray.Length) return;

            OData newOrderClass = GetOrderData_ForInsert(gameObject);
            ArrayExtension.Insert(ref _orderArray, index, newOrderClass);
        }


        #region Get OrderData
        OData GetOrderData_ForAdd(GameObject gameObject) => GetOrderData(gameObject, false);
        OData GetOrderData_ForInsert(GameObject gameObject) => GetOrderData(gameObject, true);

        //Since this class is not deriving from a monobehaviour, we need to pass in the reference of the gameobject this class is being serialized on
        OData GetOrderData(GameObject gameObject, bool isForInsert)
        {
            // Holder holder = GetComponent<Holder>();
            Holder holder = gameObject.GetComponent<Holder>();
            OData o = new OData();
            o.Initialize(holder, isForInsert);
            return o;
        }
        #endregion
        #endregion


#else
 //This is the monobehaviour version of the ArrayUser class. There is also basic class version where ArrayUser does not inherit from anything except for new()
    [System.Serializable]
    public class ArrayUser<OData, Holder, Data>
    where OData : OrderData<Holder>, new()
    where Holder : IArrayHolder
    where Data : new()
    {

        ///<Summary>
        ///This array is the order in which you get your Data. For eg, let Data be a monobehaviour that stores cakes. By looping through OData, you are retrieving the cakes from the Holder class which is where the CakeData[] is being stored & serialized.
        ///</Summary>
        [SerializeField]
        OData[] _orderArray = new OData[0];

#endif


    }
}