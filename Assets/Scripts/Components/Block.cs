namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using DualList;

    //A block class will hold the order of the commands to be executed and then call
    //the respective commandexecutor to execute those commands
    [System.Serializable]
    public class Block : ArrayUser<Block.EffectOrder, BaseEffectExecutor<Effect>, Effect>
    {
        #region Definitions
        [Serializable]
        class Settings
        {
            [SerializeField]
            bool _randomBool = default;
        }

        [Serializable]
        public class EffectOrder : OrderData<BaseEffectExecutor<Effect>>
        { }


        #endregion

        #region Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        Settings _settings = default;


        #endregion
    }
}