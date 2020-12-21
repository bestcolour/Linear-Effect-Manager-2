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

        #region Hidden Field
        int _effectFrontier = 0;
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


        //This method is not perfect.
        //Below I list down 3 scenarios in which the _orderArray (the effects array) may look like
        //Legend: 
        //The alphabets & word below represent effects in your _orderArray (the effects array)

        //I : Instant effect (effects which are determined to be finished in a single frame call)
        //U : Updatable effect  (effects which are determined to be finished in after multiple frame calls)
        //UHalt : Updatable effect which halts code flow
        //Scenario 1:
        // I -> I -> I 
        //This scenario is straight forward, in a single frame call, all of the block's effects will be executed 

        //Scenario 2:
        // I -> UHalt -> I 
        //This scenario is straight forward also, in the first frame call, the first 2 effects will called. From the second frame onwards, the block will call starting from UHalt and will skip the first I effect. The 3rd effect will only be called after the 2nd effect returns a "true" value when calling it.

        //Scenario 3:
        // I -> U -> I 
        //This scenario is a bit tricky.

        //NOTE TO SELF THIS IS WRONG. YOU CANT APPROACH CODE EXECUTION IN THE BLOCK LIKE THIS.


        ///<Summary>Runs all of the effect code on the block by sequentially going down the Effect Order array. Returns true when all of the block's effects have been fully finished. This should be called inside an update loop. </Summary>
        public bool ExecuteBlockEffects()
        {
            bool isAllCodeExecuted = false;

            for (; _effectFrontier < _orderArray.Length; _effectFrontier++)
            {
                EffectOrder effect = _orderArray[_effectFrontier];
                isAllCodeExecuted = effect.CallEffect(out bool haltUntilFinished);

                //Return this loop if code flow is being halted and the current effect has not been finished
                if (haltUntilFinished && !isAllCodeExecuted)
                {
                    return isAllCodeExecuted;
                }


            }

            //If all code is executed, reset the frontier
            if (isAllCodeExecuted)
            {
                _effectFrontier = 0;
            }

            return isAllCodeExecuted;
        }

        //Heres how calling of effects is going to work:
        //For instant effects, all of it will be called within one frame
        //for update effects, the effects will be updated once everyframe.
        //there will be a boolean on every updatable effect to determine whether that effect will stop the code flow or allow code to flow past it
        //all effects on a block needs to return a true value inorder to say that a block has finished executing all its effects




    }


}