namespace LinearEffects
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    //Flow chart is the class which holds/creates blocks
    // which can be called at different times (ie when game starts, game ends , )
    public partial class BaseFlowChart : MonoBehaviour
    {
        #region Definitions
        [System.Serializable]
        protected class FlowChartSettings
        {
            [SerializeField]
            bool _someSetting = default;
        }

        #endregion

        #region Exposed Field
        [SerializeField]
        protected Block[] _blocks = new Block[0];

        [Header("Settings")]
        [SerializeField]
        protected FlowChartSettings _settings = new FlowChartSettings();
        #endregion

        #region Hidden Field
        ///<summary>A dictionary which holds all of the blocks on this flowchart. The key is the Block's Name and the value is the respective block </summary>
        protected Dictionary<string, Block> _blockDictionary = default;

        #endregion

        ///<Summary>Proxy Awake method call. Initializes the things needed for a flowchart to work. If you want an already established script, use FlowChart.cs instead</Summary>
        public void GameAwake()
        {
            _blockDictionary = new Dictionary<string, Block>();
            foreach (var block in _blocks)
            {
                _blockDictionary.Add(block.BlockName, block);
            }
        }


        ///<Summary>Proxy Update method call. It will update the blocks' effects (where some of the effects ought to be called over multiple frames)</Summary>
        public void GameUpdate()
        {

        }

        ///<Summary>Get the block via index in the block array</Summary>
        public Block GetBlock(int index)
        {
            return _blocks[index];
        }

        ///<Summary>Get the block via block name in the block dictionary</Summary>
        public Block GetBlock(string blockName)
        {
            return _blockDictionary[blockName];
        }



    }



}

