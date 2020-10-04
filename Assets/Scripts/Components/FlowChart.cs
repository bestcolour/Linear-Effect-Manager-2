using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinearEffects
{
    //Flow chart is the class which holds/creates blocks
    // which can be called at different times (ie when game starts, game ends , )
    public class FlowChart : MonoBehaviour
    {
        [SerializeField]
        Block[] _blocks = default;

        [System.Serializable]
        class Settings
        {
            [SerializeField]
            bool _someSetting = default;
        }

        [Header("Settings")]
        [SerializeField]
        Settings _settings = default;

    }
}

