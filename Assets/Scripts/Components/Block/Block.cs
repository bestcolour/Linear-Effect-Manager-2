namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    //A block class will hold the order of the effects to be executed and then call
    //the respective effectexecutor to execute those effects
    [Serializable]
    public partial class Block
#if UNITY_EDITOR
    : ISavableData
#endif
    {
        #region Definitions
        [Serializable]
        public partial class BlockSettings
        {
            //======================= NODE PROPERTIES (ie properties which block node uses & saves) =========================
            public string BlockName;
        }


        #endregion

        #region Exposed Fields
        [Header("<== Click To Open ==>")]
        [SerializeField]
        BlockSettings _blockSettings;

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


        ///<Summary>Runs all of the effect code on the block by sequentially going down the Effect Order array</Summary>
        public void ExecuteBlockEffects()
        {

        }

    }


}