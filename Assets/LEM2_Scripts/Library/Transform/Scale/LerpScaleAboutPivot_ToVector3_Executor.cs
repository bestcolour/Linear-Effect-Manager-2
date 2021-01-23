namespace LinearEffects.DefaultEffects
{
    using UnityEngine;

    ///<Summary>Lerps a transform's scale towards a value about a local pivot</Summary>
    public class LerpScaleAboutPivot_ToVector3_Executor : UpdateEffectExecutor<LerpScaleAboutPivot_ToVector3_Executor.LerpScale>
    {
        [System.Serializable]
        public class LerpScale : UpdateEffect
        {
            public Transform TargetTransform = default;

            public Vector3 TargetScale = default
                , LocalPivot = default
                ;

            [Range(0, 1000)]
            public float Duration = 1;

            //Runtime
            Vector3 _initialScale = default
            , _direction = default
            , _initialPos = default
            ;
            float _timer = default;

            public void BeginExecute()
            {
                _initialPos = TargetTransform.position;
                // _pivotWorldPos = _transform.TransformPoint(_localPivot);
                //Get the original direction vector (with their mags intact) from the center of the transform to the pivot point
                _direction = TargetTransform.TransformPoint(LocalPivot) - _initialPos;
                _initialScale = TargetTransform.localScale;
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
                //Lerp the scale of the cube
                TargetTransform.localScale = Vector3.Lerp(_initialScale, TargetScale, percentage);


                //============= SETTING THE NEW POSITIION OF THE TRANSFORM ====================
                //Invert the direction so that we can change the transform's position in the scaleddirection
                Vector3 dir = -_direction;
                dir *= percentage;

                //Translate the dir point back to pivot
                dir += _initialPos;
                TargetTransform.position = dir;

                return false;
            }


            public void EndExecute()
            {
                TargetTransform.localScale = TargetScale;
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