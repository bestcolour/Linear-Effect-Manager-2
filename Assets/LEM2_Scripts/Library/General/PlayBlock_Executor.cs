namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Serializable]
    ///<Summary>Plays a block name from a selected flowchart</Summary>
    public class PlayBlock_Executor : EffectExecutor<PlayBlock_Executor.PlayBlockEffect>
    {
        [System.Serializable]
        public class PlayBlockEffect : Effect
        {
            [Header("----- Play Block -----")]
            public BaseFlowChart FlowChart = default;

            public string BlockToPlay = default;

            public void PlayBlock()
            {
                FlowChart.PlayBlock(BlockToPlay);
            }
        }


        protected override bool ExecuteEffect(PlayBlockEffect effectData)
        {
            effectData.PlayBlock();
            return true;
        }


    }

}