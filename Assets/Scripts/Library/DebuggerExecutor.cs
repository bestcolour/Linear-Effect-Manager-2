using System.Collections;
using System.Collections.Generic;
using LinearEffects;
using UnityEngine;

public class DebuggerExecutor : UpdateEffectExecutor<DebuggerEffect>
{
    protected override bool ExecuteEffect(DebuggerEffect effectData)
    {
        effectData.Lol = true;
        return effectData.Lol;
    }
}

//Step 2) Add System.Serializable attribute to your new command
[System.Serializable]
public class DebuggerEffect : Effect
{
    public bool Lol = default;


    // public override bool ExecuteEffect()
    // {
    //     Debug.Log("HelloWord");
    //     return true;
    // }
}
