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
        [System.Serializable]
        class Settings
        {
            [SerializeField]
            bool _randomBool = default;
        }

        class EffectIndices
        {
            public BaseEffectExecutor Executor;
            public List<int> Indices;
            public EffectIndices(BaseEffectExecutor executor)
            {
                Executor = executor;
                Indices = new List<int>();
            }

        }


        #endregion

        [Header("Some Settings")]
        [SerializeField]
        Settings _settings = default;


        List<EffectIndices> _effectIndices = new List<EffectIndices>();





#if UNITY_EDITOR

        //This should be drawn as a reorderable list
        [SerializeField]
        CommandLabel[] _commandLabels = default;


        public void EditorUse_AddEffect(Type type)
        {
            EffectIndices executorToFind = _effectIndices.Find(x => x.Executor.GetType() == type);

            //Check if current list has recorded this type of Executor. If not, addcomponent
            if (executorToFind == null)
            {
                BaseEffectExecutor newExecutor = (BaseEffectExecutor)gameObject.AddComponent(type);
                //Update list
                executorToFind = new EffectIndices(newExecutor);
                _effectIndices.Add(executorToFind);
            }

            executorToFind.Executor.EditorUse_AddEffect();
        }


#endif







    }

}