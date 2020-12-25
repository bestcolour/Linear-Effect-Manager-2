
namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LerpRotateAboutPivot_ToVector3_Executor : UpdateEffectExecutor<LerpRotateAboutPivot_ToVector3_Executor.LerpRotate>
    {
        [System.Serializable]
        public class LerpRotate : UpdateEffect
        {
            [SerializeField]
            Transform _transform = default;

            [SerializeField]
            Vector3 _targetRotation = default
            , _localPivot = default
            ;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1;

            Vector3 _direction = default
            , _pivotWorldPos = default
            ;
            Quaternion _initialRot = default
           , _targetRot = default
            ;

            float _timer = default;

            public void BeginExecute()
            {
                _initialRot = _transform.localRotation;
                _targetRot = Quaternion.Euler(_targetRotation);
                _pivotWorldPos = _transform.TransformPoint(_localPivot);
                _direction = _transform.position - _pivotWorldPos;
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
                Quaternion q = Quaternion.Lerp(_initialRot, _targetRot, percentage);
                _transform.localRotation = q;


                //=========== SETTING TRANSFORM POSITION AFTER ROTATION ===========
                Vector3 dir = _direction;
                //Apply rotation to direction vector
                dir = q * dir;
                dir += _pivotWorldPos;

                _transform.position = dir;

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