namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    ///<Summary>Lerps a transform's localposition from its current value to a target value</Summary>
    public class LerpLocalPosition_ToPosition_Executor : UpdateEffectExecutor<LerpLocalPosition_ToPosition_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : UpdateEffect
        {
            public Transform TargetTransform = default;

            [Tooltip("The local position of the target point you wish to lerp towards")]
            public Vector3 TargetLocalPos = default;

            [Range(0, 1000)]
            public float Duration = 1;

            //Runtime
            Vector3 _initialPosition = default;
            float _timer = default;

            public void BeginExecute()
            {
                _initialPosition = TargetTransform.localPosition;
                _timer = Duration;
            }

            public bool Execute()
            {
                if (_timer <= 0)
                {
                    TargetTransform.localPosition = TargetLocalPos;
                    return true;
                }

                _timer -= Time.deltaTime;

                float percentage = _timer / Duration;
                TargetTransform.localPosition = Vector3.Lerp(TargetLocalPos, _initialPosition, percentage);
                return false;
            }

        }

        protected override void BeginExecuteEffect(MyEffect effectData)
        {
            effectData.BeginExecute();
        }

        protected override bool ExecuteEffect(MyEffect effectData)
        {
            return effectData.Execute();
        }
    }

}