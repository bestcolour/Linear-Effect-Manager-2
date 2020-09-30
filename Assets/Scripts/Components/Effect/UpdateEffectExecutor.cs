namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    //The UpdateEffectExecutor is assumed to have an ExecuteEffect function which needs multiple frame to complete
    public abstract class UpdateEffectExecutor<T> : EffectExecutor<T> 
    where T : Effect, new()
    {
     
    }

}