using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearEffects;

public class CallBlockName : MonoBehaviour
{

    [SerializeField]
    private string _blockName;

    BaseFlowChart _flowChart = default;
    void Start()
    {
        _flowChart = GetComponent<BaseFlowChart>();
        _flowChart.PlayBlock(_blockName);
    }
}
