namespace LinearEffects.General
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Serializable]
    public class PlayBlockExecutor : EffectExecutor<PlayBlockExecutor.PlayBlockEffect>
    {
        [System.Serializable]
        public class PlayBlockEffect : Effect
        {
            [Header("----- Play Block -----")]
            [SerializeField]
            BaseFlowChart _flowChart = default;

            [SerializeField]
            string _blockToPlay = default;

            public void PlayBlock()
            {
                _flowChart.PlayBlock(_blockToPlay);
            }
        }


        protected override bool ExecuteEffect(PlayBlockEffect effectData)
        {
            effectData.PlayBlock();
            return true;
        }


    }

}