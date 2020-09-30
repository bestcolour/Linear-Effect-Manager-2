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
        class ExecutorDataSet
        {
            public BaseEffectExecutor Executor;
            public List<int> Indices;
            // public List<int> Indices;
            public ExecutorDataSet(BaseEffectExecutor executor)
            {
                Executor = executor;
                Indices = new List<int>();
            }
        }
        #endregion

        #region Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        Settings _settings = default;


        //This list organises Executor type with the indices of elements called by this block in which the executor
        [SerializeField]
        List<ExecutorDataSet> _effectIndices = new List<ExecutorDataSet>();


        [SerializeField]
        List<int> _orderOfEffects = new List<int>();

        #endregion



#if UNITY_EDITOR
        //This should be drawn as a reorderable list
        [SerializeField]
        CommandLabel[] _commandLabels = default;


        public void EditorUse_AddEffect(Type type)
        {
            ExecutorDataSet executorSet = _effectIndices.Find(x => x.Executor.GetType() == type);

            //Check if current list has recorded this type of Executor. If not, addcomponent
            if (executorSet == null)
            {
                BaseEffectExecutor newExecutor = (BaseEffectExecutor)gameObject.AddComponent(type);
                //Update list
                executorSet = new ExecutorDataSet(newExecutor);
                _effectIndices.Add(executorSet);
            }

            int newIndex = executorSet.Executor.EditorUse_AddEffect();
            executorSet.Indices.Add(newIndex);
            UpdateEffectOrder();
        }


        public void UpdateEffectOrder()
        {

        }

#endif







    }

}