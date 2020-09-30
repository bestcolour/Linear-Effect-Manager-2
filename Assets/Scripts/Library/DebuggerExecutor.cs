using System.Collections;
using System.Collections.Generic;
using LinearEffects;
using UnityEngine;

public class DebuggerExecutor : BaseEffectExecutor<DebuggerEffect> { }

//Step 2) Add System.Serializable attribute to your new command
[System.Serializable]
public class DebuggerEffect : BaseEffect
{
    [SerializeField]
    bool _bool = default;

    public override BaseEffectType Type => BaseEffectType.Instant;

    public override bool ExecuteEffect()
    {
        Debug.Log("HelloWord");
        return false;
    }
}
