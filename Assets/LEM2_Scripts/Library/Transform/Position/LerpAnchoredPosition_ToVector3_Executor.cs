namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    ///<Summary>Lerps a recttransform's anchored value from its current value to a target value</Summary>
    public class LerpAnchoredPosition_ToVector3_Executor : UpdateEffectExecutor<LerpAnchoredPosition_ToVector3_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : UpdateEffect
        {
            public RectTransform TargetTransform = default;

            public Vector3 TargetPosition = default;

            [Range(0, 1000)]
            public float Duration = 1;

            //Runtime
            Vector3 _initialPosition = default;
            float _timer = default;

            public void BeginExecute()
            {
                _initialPosition = TargetTransform.anchoredPosition;
                _timer = Duration;
            }

            public bool Execute()
            {
                if (_timer <= 0)
                {
                    return true;
                }

                _timer -= Time.deltaTime;

                float percentage = _timer / Duration;
                TargetTransform.anchoredPosition = Vector3.Lerp(TargetPosition, _initialPosition, percentage);
                return false;
            }


            public void EndExecute()
            {
                TargetTransform.anchoredPosition = TargetPosition;
            }


        }

        protected override void BeginExecuteEffect(MyEffect effectData)
        {
            effectData.BeginExecute();
        }

        protected override void EndExecuteEffect(MyEffect effectData)
        {
            effectData.EndExecute();
        }

        protected override bool ExecuteEffect(MyEffect effectData)
        {
            return effectData.Execute();
        }


    }

}