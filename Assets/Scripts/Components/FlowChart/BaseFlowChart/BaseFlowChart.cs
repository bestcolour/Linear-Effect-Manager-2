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
        ///<summary>A hashset which holds all of the blocks on this flowchart has been called to execute its effects.</summary>
        protected HashSet<Block> _activeBlocks = default;
        ///<summary>A list which holds the blocks which have fully executed all its effects and is waiting for removal after all blocks are updated</summary>
        protected List<Block> _blockRemovalList = default;
        #endregion



        #region Public Get & Play Methods
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

        ///<Summary>Plays a block via block index</Summary>
        public void PlayBlock(int index)
        {
            Block intendedToPlayBlock = _blocks[index];
            PlayBlock(intendedToPlayBlock);
        }

        ///<Summary>Plays a block via block name</Summary>
        public void PlayBlock(string blockName)
        {
            Block intendedToPlayBlock = _blockDictionary[blockName];
            PlayBlock(intendedToPlayBlock);
        }

        ///<Summary>Plays a block via block instance</Summary>
        public void PlayBlock(Block block)
        {
            if (_activeBlocks.Contains(block))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"The block {block.BlockName} on the flowchart {name} is already playing!", this);
#endif
                return;
            }

            _activeBlocks.Add(block);
        }
        #endregion

        #region Life Cycle
        ///<Summary>Proxy Awake method call. Initializes the things needed for a flowchart to work. If you want an already established script, use FlowChart.cs instead</Summary>
        public void GameAwake()
        {
            _blockDictionary = new Dictionary<string, Block>();
            foreach (var block in _blocks)
            {
                _blockDictionary.Add(block.BlockName, block);
            }

            _activeBlocks = new HashSet<Block>();
            _blockRemovalList = new List<Block>();
        }


        ///<Summary>Proxy Update method call. It will update the blocks' effects (where some of the effects ought to be called over multiple frames)</Summary>
        public void GameUpdate()
        {
            foreach (var block in _activeBlocks)
            {
                if (block.ExecuteBlockEffects())
                {
                    //Remove block from hashset
                    _blockRemovalList.Add(block);
                }
            }

            TryRemoveFinishedBlocks();
        }

        private void TryRemoveFinishedBlocks()
        {
            if (_blockRemovalList.Count <= 0)
            {
                return;
            }

            foreach (var item in _blockRemovalList)
            {
                _activeBlocks.Remove(item);
            }
            _blockRemovalList.Clear();
        }

        #endregion

















    }
}

