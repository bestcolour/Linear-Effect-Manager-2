using System.Collections;
using System.Collections.Generic;
using LinearEffects;
using UnityEngine;

[DisallowMultipleComponent]
public class Deb : UpdateEffectExecutor<Deb.DebuggerEffect>
{

    //Step 2) Add System.Serializable attribute to your new command
    [System.Serializable]
    public class DebuggerEffect : Effect
    {
        public bool Lol = default;
    }

    protected override bool ExecuteEffect(DebuggerEffect effectData)
    {
        effectData.Lol = true;
        return effectData.Lol;
    }
}


