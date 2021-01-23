namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    ///<Summary>Lerps a transform's rotation to a vector3 value</Summary>
    public class LerpRotate_ToVector3_Executor : UpdateEffectExecutor<LerpRotate_ToVector3_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : UpdateEffect
        {
            public Transform TargetTransform = default;

            public Vector3 TargetRotation = default
            ;

            [Range(0, 1000)]
            public float Duration = 1;

            //Runtime
            Quaternion _initialRot = default
           , _targetRot = default
            ;

            float _timer = default;

            public void BeginExecute()
            {
                _initialRot = TargetTransform.localRotation;
                _targetRot = Quaternion.Euler(TargetRotation);
                _timer = Duration;
            }

            public bool Execute()
            {
                if (_timer <= 0)
                {
                    return true;
                }

                _timer -= Time.deltaTime;

                float percentage = 1 - (_timer / Duration);
                TargetTransform.localRotation = Quaternion.Lerp(_initialRot, _targetRot, percentage);
                return false;
            }


            public void EndExecute()
            {
                TargetTransform.localRotation = _targetRot;
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