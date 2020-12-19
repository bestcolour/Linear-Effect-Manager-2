namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    //A block class will hold the order of the commands to be executed and then call
    //the respective commandexecutor to execute those commands
    [Serializable]
    public partial class Block
#if UNITY_EDITOR
    : ISavableData
#endif
    // public partial class Block : ArrayUser<Block.EffectOrder, BaseEffectExecutor>, ISavableData
    {
        #region Definitions
        [Serializable]
        public partial class BlockProperties
        {
            //======================= NODE PROPERTIES (ie properties which block node uses & saves) =========================
            public string BlockName;
        }


        #endregion

        #region Exposed Fields
        [Header("<== Click To Open ==>")]
        [SerializeField]
        BlockProperties _blockSettings;

        ///<Summary>
        ///This array is the order in which you get your Data. For eg, let Data be a monobehaviour that stores cakes. By looping through OData, you are retrieving the cakes from the Holder class which is where the CakeData[] is being stored & serialized.
        ///</Summary>
        [SerializeField]
        protected EffectOrder[] _orderArray = new EffectOrder[0];

        #endregion

        #region Properties
        public string BlockName
        {
            get
            {
                return _blockSettings.BlockName;
            }
        }

        #endregion


    }


}