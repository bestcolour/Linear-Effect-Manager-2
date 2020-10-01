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

        #region  ExecutorDataSet Methods
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


#if UNITY_EDITOR
            ///<Summary>
            ///Adds a new entry to the dataset's Executor and Indices list. Returns index of the newly added integer in the Indices list.
            ///</Summary>
            public int Editor_AddToIndicesList()
            {
                int i = Executor.EditorUse_AddNewEffectEntry();
                return ArrayExtension.AddReturn(ref Indices, i);
            }



#endif


        }






        #endregion



        [Serializable]
        class EffectsOrderDataSet
        {
            public int IndexFor_ExecutorDataList;
            public int IndexFor_IndicesList;
            public EffectsOrderDataSet(int indexOfExecutorDataSet, int elementIndexOfIndicesArray)
            {
                IndexFor_ExecutorDataList = indexOfExecutorDataSet;
                IndexFor_IndicesList = elementIndexOfIndicesArray;
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


        ///<Summary>
        /// Adds a new effect to the block. This will update the 2 sets of arrays in order to have accurate serialized values.
        ///</Summary>
        public void EditorUse_AddEffect(Type type)
        {
            //Check if there is an existing type of executor. If not, addcomponent
            if (!TryFindExecutorSet(type, out int indexOfExecutorSet))
            {
                //====================UPDATE EXECUTOR DATASET LIST=================
                indexOfExecutorSet = ArrayExtension.AddReturn
                (
                    ref _executor_and_effectIndices,
                    new ExecutorDataSet
                        (
                            (BaseEffectExecutor)gameObject.AddComponent(type)
                        )
                    );

            }
            //====================UPDATE EXECUTOR & INDICES LISTS==========================
            int indicesListIndex = _executor_and_effectIndices[indexOfExecutorSet].Editor_AddToIndicesList();

            //====================UPDATE EFFECTS ORDER DATASET LIST==========================
            ArrayExtension.Add(ref _orderOfEffects, new EffectsOrderDataSet(indexOfExecutorSet, indicesListIndex));
        }

        ///<Summary>
        /// Removes an effect at its index on the reorderable list from the block. This will update the 2 sets of arrays in order to have accurate serialized values.
        ///</Summary>
        public void EditorUse_RemoveEffect(int i)
        {
            //If order of effects does not hold indexOfEffectInOrder
            if (i >= _orderOfEffects.Length)
            {
                return;
            }


            //Due to the removal of an element from a list, we need to ensure that everything is updated
            //Updated means that values which are removed should cause other dependent values to -1, etc

            //====================UPDATE EFFECTS ORDER DATASET LIST==========================
            //Get and remove the orderDataSet we are current oberving
            EffectsOrderDataSet orderDataSet = _orderOfEffects[i];
            ArrayExtension.RemoveAt(ref _orderOfEffects, i);


            //====================UPDATE EXECUTOR DATASET LIST==========================
            ExecutorDataSet executorSet = _executor_and_effectIndices[orderDataSet.IndexFor_ExecutorDataList];

            //Get and remove the effect index from the indices list
            int effectIndex = executorSet.Indices[orderDataSet.IndexFor_IndicesList];
            ArrayExtension.RemoveAt(ref executorSet.Indices, orderDataSet.IndexFor_ExecutorDataList);

            //If there are no more effects being called by this block on that executor, remove this executorset from the list of executorSets
            if (executorSet.Indices.Length <= 0)
            {
                ArrayExtension.Remove(ref _executor_and_effectIndices, executorSet);
            }
            else
            {
                //ISSUE: When i have multiple blocks, this will not work because i will have to update all blocks
                //Update all the indices' elements above the removed index. (Assuming that all the indices' elements are greater than the removed index's element)
                for (int indexAboveRemovedIndex = orderDataSet.IndexFor_ExecutorDataList; indexAboveRemovedIndex < executorSet.Indices.Length; indexAboveRemovedIndex++)
                {
                    executorSet.Indices[indexAboveRemovedIndex] -= 1;
                }
            }

            //====================REMOVE EFFECT FROM EXECUTOR USING INDEX==========================
            executorSet.Executor.EditorUse_RemoveEffectAt(effectIndex);
        }


        #region Supporting Methods
        bool TryFindExecutorSet(Type type, out int indexOfExecutorSet)
        {
            indexOfExecutorSet = _executor_and_effectIndices.FindIndex(x => x.Executor.GetType() == type);

            if (indexOfExecutorSet == -1)
            {
                return false;
            }

            return true;
        }

        #endregion


#endif







    }

}