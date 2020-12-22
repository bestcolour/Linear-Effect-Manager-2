#if UNITY_EDITOR
namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public partial class BaseFlowChart : MonoBehaviour
    {

        protected partial class FlowChartSettings
        {
            [Tooltip("If this is set to true, all Exectuor components will be hidden in the inspector window")]
            [SerializeField]
            public bool HideExecutors = true;
        }

        public const string PROPERTYNAME_BLOCKARRAY = "_blocks";

        public Block Editor_GetBlock(string label)
        {
            int index = _blocks.FindIndex(x => x.BlockName == label);
            if (index == -1) return null;
            return _blocks[index];
        }


        #region Hiding In Inspector
        protected virtual void OnValidate()
        {
            HideFlags flag = _settings.HideExecutors ? HideFlags.HideInInspector : HideFlags.None;
            BaseEffectExecutor[] hideExecutors = GetComponents<BaseEffectExecutor>();

            foreach (var item in hideExecutors)
            {
                item.hideFlags = flag;
            }
        }

        #endregion

        #region Editor Inspector Methods
        ///<Summary>Clears all blocks in the flowchart and removing all the Executors on the flowchart</Summary>
        public void Editor_ResetFlowChart()
        {
            _blocks = new Block[0];
            BaseEffectExecutor[] allExecutorsOnFlowChart = GetComponents<BaseEffectExecutor>();
            foreach (var item in allExecutorsOnFlowChart)
            {
                DestroyImmediate(item);
            }
        }
        #endregion
    }

}
#endif