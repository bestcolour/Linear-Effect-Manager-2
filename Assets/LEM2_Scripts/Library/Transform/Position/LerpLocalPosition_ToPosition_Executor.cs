namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LerpLocalPosition_ToPosition_Executor : UpdateEffectExecutor<LerpLocalPosition_ToPosition_Executor.LerpTransform>
    {
        [System.Serializable]
        public class LerpTransform : UpdateEffect
        {
            [SerializeField]
            Transform _transform = default;

            [SerializeField]
            [Tooltip("The local position of the target point you wish to lerp towards")]
            Vector3 _targetLocalPos = default;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1;

            //Runtime
            Vector3 _initialPosition = default;
            float _timer = default;

            public void BeginExecute()
            {
                _initialPosition = _transform.localPosition;
                _timer = _duration;
            }

            public bool Execute()
            {
                if (_timer <= 0)
                {
                    _transform.localPosition = _targetLocalPos;
                    return true;
                }

                _timer -= Time.deltaTime;

                float percentage = _timer / _duration;
                _transform.localPosition = Vector3.Lerp(_targetLocalPos, _initialPosition, percentage);
                return false;
            }

        }

        protected override void BeginExecuteEffect(LerpTransform effectData)
        {
            effectData.BeginExecute();
        }

        protected override void EndExecuteEffect(LerpTransform effectData) { }

        protected override bool ExecuteEffect(LerpTransform effectData)
        {
            return effectData.Execute();
        }


    }

}