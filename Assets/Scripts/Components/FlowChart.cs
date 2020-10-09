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
        Block[] _blocks;

        //For editor usage only
#if UNITY_EDITOR
        public Block[] BlocksArray => _blocks;

        void Awake()
        {
            if (_blocks == null)
            {
                _blocks = new Block[0];
            }

        }


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


    }
}

