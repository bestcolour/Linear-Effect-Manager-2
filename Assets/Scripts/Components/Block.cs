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
        class BlockSettings
        {
            [SerializeField]
            bool _randomBool = default;
        }

        [Serializable]
        public class EffectOrder : OrderData<BaseEffectExecutor<Effect>> { }
        #endregion

        #region Runtime Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        BlockSettings _settings = default;


        #endregion



#if UNITY_EDITOR
        #region Editor Time Cached Variables

        #region Constants
        static readonly Color DEFAULT_BLOCK_COLOUR = new Color(0, 0.4f, 0.8f, 1f);
        #endregion

        [field: SerializeField]
        public string BlockName = "New Block";
        [field: SerializeField]
        public Color BlockColour = DEFAULT_BLOCK_COLOUR;
        [field: SerializeField]
        public Vector2 BlockPosition = Vector2.zero;

        // public Block()
        // {
        //     BlockName = "New String";
        //     BlockColour = DEFAULT_BLOCK_COLOUR;
        //     BlockPosition = Vector2.zero;
        // }


        #endregion
#endif




    }
}