using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearEffects;

public class CallFirstBlock : MonoBehaviour
{
    [SerializeField]
    FlowChart _flowChart = default;

    private void Start()
    {
        //Call a certain block on awake
        _flowChart.PlayBlock(0);
    }

}
