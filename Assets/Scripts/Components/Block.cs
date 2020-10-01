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

        // [Serializable]
        // class ExecutorData
        // {
        //     BaseEffectExecutor _executor;
        //     int[] _effectIndicesCalledInExecutor;
        //                 //This class should only be initialized/edited inside editor, not during gameplay
        //     #if UNITY_EDITOR
        //                 public ExecutorData(Block block, Type executorType)
        //                 {
        //                     _executor = (BaseEffectExecutor)block.gameObject.AddComponent(executorType);
        //                     _effectIndicesCalledInExecutor = new int[0];
        //                 }

        //                 ///<Summary>
        //                 ///(Editor Only) Returns the index of the newly added Effect Index called In the Executor when you add a new effect.
        //                 ///</Summary>
        //                 public int AddEffect()
        //                 {
        //                     return ArrayExtension.AddReturn(ref _effectIndicesCalledInExecutor, _executor.EditorUse_AddNewEffectEntry());
        //                 }

        //                 ///<Summary>
        //                 ///(Editor Only) Removes the effect from the executor with the inputted effect index. An Order update is necessary after calling this function
        //                 ///</Summary>
        //                 public void RemoveEffectAt(int i)
        //                 {
        //                     //Get effect index
        //                     i = GetEffectIndex(i);
        //                     _executor.EditorUse_RemoveEffectAt(i);
        //                 }

        //                 int GetEffectIndex(int i)
        //                 {
        //                     return _effectIndicesCalledInExecutor[i];
        //                 }

        //     #endif
        // }

        [Serializable]
        class EffectOrder
        {
            [SerializeField]
            BaseEffectExecutor _executor;

            [SerializeField]
            int _executorEffectIndex;

#if UNITY_EDITOR
            public BaseEffectExecutor GetExecutor() { return _executor; }

            //Initializable during only editor time
            public EffectOrder(BaseEffectExecutor executor)
            {
                _executor = executor;
                _executorEffectIndex = executor.EditorUse_AddNewEffectEntry();
            }

            public void OnRemove(int index)
            {
                _executor.EditorUse_RemoveEffectAt(index);

            }

            public void UpdateAfterEffectRemoval(int indexRemoved)
            {
                //Update the effect index due to the removal of a element in the executor
                if (_executorEffectIndex > indexRemoved)
                {
                    _executorEffectIndex--;
                }
            }
#endif

        }


        #endregion

        #region Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        Settings _settings = default;


        EffectOrder[] _orderOfEffects = new EffectOrder[0];


        #endregion






#if UNITY_EDITOR
        //====================================================== EDITOR ZONE=================================================================
        //This should be drawn as a reorderable list
        [SerializeField]
        CommandLabel[] _commandLabels = default;


        public void Editor_AddEffect(Type type)
        {
            ArrayExtension.Add(ref _orderOfEffects, new EffectOrder(GetExecutor(type)));
        }

        public void Editor_InsertEffect(Type type, int index)
        {
            ArrayExtension.Insert(ref _orderOfEffects, index, new EffectOrder(GetExecutor(type)));
        }

        public void Editor_RemoveEffectOrder(int index)
        {
            if (index >= _orderOfEffects.Length)
            {
                return;
            }


            _orderOfEffects[index].OnRemove(index);
            AfterEffectRemoval_Update(_orderOfEffects[index].GetExecutor(), index);

            //Remove order effects after it is done
            ArrayExtension.RemoveAt(ref _orderOfEffects, index);
        }

        public void Editor_ReArrangeEffectOrder(int effectA, int effectB)
        {
            EffectOrder a = _orderOfEffects[effectA], b = _orderOfEffects[effectB];

            _orderOfEffects[effectA] = b;
            _orderOfEffects[effectB] = a;
        }


        #region  Support Methods for Effect Order
        BaseEffectExecutor GetExecutor(Type type)
        {
            //For now we use getcomponent
            if (!TryGetComponent(type, out Component component))
            {
                //No component found, add new one
                //for now we add component to the block
                return (BaseEffectExecutor)gameObject.AddComponent(type);
            }

            return (BaseEffectExecutor)component;
        }

        void AfterEffectRemoval_Update(BaseEffectExecutor executorReference, int effectIndex)
        {
            EffectOrder[] effectsAffected = _orderOfEffects.FindAll(x => x.GetExecutor() == executorReference);

            for (int i = 0; i < effectsAffected.Length; i++)
            {
                effectsAffected[i].UpdateAfterEffectRemoval(effectIndex);
            }
        }

        #endregion

#endif
    }
}