using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectTesting : MonoBehaviour
{
    class Data
    {
        public int Index = 1;
        public Data(int index) { Index = index; }
    }

    private void Awake()
    {
        object[] objectArray = new object[3] { new Data(1), new Data(2), new Data(3) };


        Data[] dataArray = objectArray.Select(x => (Data)x).ToArray();
        // Data[] dataArray = (Data[])objectArray;




        foreach (var item in dataArray)
        {
            Debug.Log(item.Index);
        }

        dataArray.Add(new Data(4), out Data[] newArray);
        objectArray = newArray;


        // foreach (var item in objectArray)
        // {
        //     Debug.Log(item);
        // }

    }
}
