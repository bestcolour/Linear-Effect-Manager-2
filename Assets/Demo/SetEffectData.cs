using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearEffects;
using LinearEffects.DefaultEffects;

[RequireComponent(typeof(BaseFlowChart))]
public class SetEffectData : MonoBehaviour
{
    [SerializeField]
    Animator _anim = default;

    private void Awake()
    {
        BaseFlowChart fc = GetComponent<BaseFlowChart>();
        fc.GameAwake();
        SetAnimatorBool_Executor.MyEffect effect = fc.GetEffect<SetAnimatorBool_Executor, SetAnimatorBool_Executor.MyEffect>("Block1", 0);

        effect.Animator = _anim;
    }
}
