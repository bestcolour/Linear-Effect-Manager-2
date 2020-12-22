using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearEffects;

public class Test : MonoBehaviour
{
    [SerializeField]
    FlowChart _flowChart = default;


    private void Awake()
    {
        //Call a certain block on awake
        _flowChart.PlayBlock(0);
    }

}
