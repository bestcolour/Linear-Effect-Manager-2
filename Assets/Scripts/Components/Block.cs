using System;
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
        [Serializable]
        class Settings
        {
            [SerializeField]
            bool _randomBool = default;
        }

        [Serializable]
        class ExecutorData
        {
            BaseEffectExecutor _executor;
            int[] _effectIndicesCalledInExecutor;



            //This class should only be initialized/edited inside editor, not during gameplay
#if UNITY_EDITOR
            public ExecutorData(Block block, Type executorType)
            {
                _executor = (BaseEffectExecutor)block.gameObject.AddComponent(executorType);
                _effectIndicesCalledInExecutor = new int[0];
            }

            ///<Summary>
            ///(Editor Only) Returns the index of the newly added Effect Index called In the Executor when you add a new effect.
            ///</Summary>
            public int AddEffect()
            {
                return ArrayExtension.AddReturn(ref _effectIndicesCalledInExecutor, _executor.EditorUse_AddNewEffectEntry());
            }

            ///<Summary>
            ///(Editor Only) Removes the effect from the executor with the inputted effect index. An Order update is necessary after calling this function
            ///</Summary>
            public void RemoveEffectAt(int i)
            {
                //Get effect index
                i = GetEffectIndex(i);
                _executor.EditorUse_RemoveEffectAt(i);
            }

            int GetEffectIndex(int i)
            {
                return _effectIndicesCalledInExecutor[i];
            }

#endif




        }

        #endregion

        #region Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        Settings _settings = default;




        #endregion






#if UNITY_EDITOR
        //======================================================= EDITOR ZONE =======================================================================
        //This should be drawn as a reorderable list
        [SerializeField]
        CommandLabel[] _commandLabels = default;












#endif
    }
}