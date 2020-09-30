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
            public int[] Indices;
            public ExecutorDataSet(BaseEffectExecutor executor)
            {
                Executor = executor;
                Indices = new int[0];
            }
        }

        [Serializable]
        class EffectsOrderDataSet
        {
            public int IndexOf_ExecutorDataSet;
            public int ExecutorDataSet_Effect_Index;
            public EffectsOrderDataSet(int indexOfExecutorDataSet, int elementIndexOfIndicesArray)
            {
                IndexOf_ExecutorDataSet = indexOfExecutorDataSet;
                ExecutorDataSet_Effect_Index = elementIndexOfIndicesArray;
            }
        }

        #endregion

        #region Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        Settings _settings = default;


        //This list organises Executor type with the indices of elements called by this block in which the executor
        [SerializeField]
        ExecutorDataSet[] _executor_and_effectIndices = new ExecutorDataSet[0];


        [SerializeField]
        EffectsOrderDataSet[] _orderOfEffects = new EffectsOrderDataSet[0];

        #endregion



#if UNITY_EDITOR
        //This should be drawn as a reorderable list
        [SerializeField]
        CommandLabel[] _commandLabels = default;


        public void EditorUse_AddEffect(Type type)
        {
            //Check if there is an existing type of executor
            int indexOfExecutorSet = _executor_and_effectIndices.FindIndex(x => x.Executor.GetType() == type);

            //If not, addcomponent
            if (indexOfExecutorSet == -1)
            {
                //====================UPDATE EXECUTOR DATASET ARR=================
                indexOfExecutorSet = ArrayExtension.AddReturn(ref _executor_and_effectIndices, new ExecutorDataSet((BaseEffectExecutor)gameObject.AddComponent(type)));
            }

            ExecutorDataSet executorSet = _executor_and_effectIndices[indexOfExecutorSet];

            //Update the Executor by adding new effect entry
            int newIndex = executorSet.Executor.EditorUse_AddNewEffectEntry();

            //Add a new index entry into the executorSet's indices array
            //also reuse newIndex to record the index in which this newIndex has been added at
            //==================UPDATE EXECUTOR DATASET ARR'S INDICES ARR=================
            newIndex = ArrayExtension.AddReturn(ref executorSet.Indices, newIndex);

            //====================UPDATE EFFECTS ORDER DATASET ARR==========================
            ArrayExtension.Add(ref _orderOfEffects, new EffectsOrderDataSet(indexOfExecutorSet, newIndex));

        }


       

#endif







    }

}