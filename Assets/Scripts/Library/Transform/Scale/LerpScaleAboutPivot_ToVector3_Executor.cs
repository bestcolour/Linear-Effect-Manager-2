namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LerpScaleAboutPivot_ToVector3_Executor : UpdateEffectExecutor<LerpScaleAboutPivot_ToVector3_Executor.LerpScale>
    {
        [System.Serializable]
        public class LerpScale : UpdateEffect
        {
            [SerializeField]
            Transform _transform = default;

            [SerializeField]
            Vector3 _targetScale = default
            , _localPivot = default
            ;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1;

            //Runtime
            Vector3 _initialScale = default
            , _direction = default
            , _initialPos = default
            ;
            float _timer = default;

            public void BeginExecute()
            {
                _initialPos = _transform.position;
                // _pivotWorldPos = _transform.TransformPoint(_localPivot);
                //Get the original direction vector (with their mags intact) from the center of the transform to the pivot point
                _direction = _transform.TransformPoint(_localPivot) -_initialPos;
                _initialScale = _transform.localScale;
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
                //Lerp the scale of the cube
                _transform.localScale = Vector3.Lerp(_initialScale, _targetScale, percentage);


                //============= SETTING THE NEW POSITIION OF THE TRANSFORM ====================
                //Invert the direction so that we can change the transform's position in the scaleddirection
                Vector3 dir = -_direction;
                dir *= percentage;

                //Translate the dir point back to pivot
                dir += _initialPos;
                _transform.position = dir;

                return false;
            }


            public void EndExecute()
            {
                _transform.localScale = _targetScale;
            }


        }

        protected override void BeginExecuteEffect(LerpScale effectData)
        {
            effectData.BeginExecute();
        }

        protected override void EndExecuteEffect(LerpScale effectData)
        {
            effectData.EndExecute();
        }

        protected override bool ExecuteEffect(LerpScale effectData)
        {
            return effectData.Execute();
        }


    }

}