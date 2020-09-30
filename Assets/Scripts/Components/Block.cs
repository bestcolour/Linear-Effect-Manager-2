using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinearEffects
{
    //A block class will hold the order of the commands to be executed and then call
    //the respective commandexecutor to execute those commands
    // [RequireComponent(typeof(FlowChart))]
    // [ExecuteInEditMode]
    public class Block : MonoBehaviour
    {
        #region Definitions
        [System.Serializable]
        class Settings
        {
            [SerializeField]
            bool _randomBool = default;
        }


        #endregion

        [Header("Some Settings")]
        [SerializeField]
        Settings _settings = default;



        




#if UNITY_EDITOR

        //This should be drawn as a reorderable list
        [SerializeField]
        CommandLabel[] _commandLabels = default;

#endif







    }

}