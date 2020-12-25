namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LerpRotate_ToVector3_Executor : UpdateEffectExecutor<LerpRotate_ToVector3_Executor.LerpRotate>
    {
        [System.Serializable]
        public class LerpRotate : UpdateEffect
        {
            [SerializeField]
            Transform _transform = default;

            [SerializeField]
            Vector3 _targetRotation = default
            ;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1;


            Quaternion _initialRot = default
           , _targetRot = default
            ;

            float _timer = default;

            public void BeginExecute()
            {
                _initialRot = _transform.localRotation;
                _targetRot = Quaternion.Euler(_targetRotation);
                _timer = _duration;
            }

            public bool Execute()
            {
                if (_timer <= 0)
                {
                    return true;
                }

                _timer -= Time.deltaTime;

                float percentage = 1 - (_timer / _duration);
                _transform.localRotation = Quaternion.Lerp(_initialRot, _targetRot, percentage);
                return false;
            }


            public void EndExecute()
            {
                _transform.localRotation = _targetRot;
            }


        }

        protected override void BeginExecuteEffect(LerpRotate effectData)
        {
            effectData.BeginExecute();
        }

        protected override void EndExecuteEffect(LerpRotate effectData)
        {
            effectData.EndExecute();
        }

        protected override bool ExecuteEffect(LerpRotate effectData)
        {
            return effectData.Execute();
        }


    }

}