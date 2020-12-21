namespace LinearEffects
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    //Flow chart is the class which holds/creates blocks
    // which can be called at different times (ie when game starts, game ends , )
    public partial class FlowChart : MonoBehaviour
    {
        [SerializeField]
        Block[] _blocks = new Block[0];

        [System.Serializable]
        class FlowChartSettings
        {
            [SerializeField]
            bool _someSetting = default;
        }

        [Header("Settings")]
        [SerializeField]
        FlowChartSettings _settings = new FlowChartSettings();


        public Block GetBlock(int index)
        {
            return _blocks[index];
        }



    }



}

