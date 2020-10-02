using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the monobehaviour version of the ArrayUser class. There is also basic class version where ArrayUser does not inherit from anything except for new()
public class ArrayUser<OData, Holder, Data> : MonoBehaviour
where OData : OrderData<Holder, Data>, new()
where Holder : ArrayHolder<Data>
where Data : new()
{

    ///<Summary>
    ///This array is the order in which you get your Data. For eg, let Data be a monobehaviour that stores cakes. By looping through OData, you are retrieving the cakes from the Holder class which is where the CakeData[] is being stored & serialized.
    ///</Summary>
    [SerializeField]
    OData[] _orderArray = new OData[0];



    #region Editor Commands

    public void Add()
    {
        ArrayExtension.Add(ref _orderArray, GetOrderData_ForAdd());
    }

    public void RemoveAt(int index)
    {
        if (index >= _orderArray.Length) return;

        _orderArray[index].OnRemove();
        ArrayExtension.RemoveAt(ref _orderArray, index);
    }

    //I assume this is for copy pasting
    public void Insert(int index)
    {
        if (index > _orderArray.Length) return;

        OData newOrderClass = GetOrderData_ForInsert();
        ArrayExtension.Insert(ref _orderArray, index, newOrderClass);
    }


    #endregion



    #region Get OrderData
    OData GetOrderData_ForAdd() => GetOrderData(false);
    OData GetOrderData_ForInsert() => GetOrderData(true);

    OData GetOrderData(bool isForInsert)
    {
        Holder holder = GetComponent<Holder>();
        OData o = new OData();
        o.Initialize(holder, isForInsert);
        return o;
    }
    #endregion

}
