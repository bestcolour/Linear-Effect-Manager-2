using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinearEffects
{
    //Flow chart is the class which holds/creates blocks
    // which can be called at different times (ie when game starts, game ends , )
    [ExecuteInEditMode]
    public class FlowChart : MonoBehaviour
    {
        [SerializeField]
        Block[] _blocks = new Block[0];

#if UNITY_EDITOR
        public const string PROPERTYNAME_BLOCKARRAY = "_blocks";

#endif

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

