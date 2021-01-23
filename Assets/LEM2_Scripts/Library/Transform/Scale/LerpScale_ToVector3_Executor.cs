namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    ///<Summary>Lerps a transform's scale to a vector3 value</Summary>
    public class LerpScale_ToVector3_Executor : UpdateEffectExecutor<LerpScale_ToVector3_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : UpdateEffect
        {
            public Transform TargetTransform = default;

            public Vector3 TargetScale = default;

            [Range(0, 1000)]
            public float Duration = 1;

            //Runtime
            Vector3 _initialScale = default;
            float _timer = default;

            public void BeginExecute()
            {
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

                float percentage = _timer / Duration;
                TargetTransform.localScale = Vector3.Lerp(TargetScale, _initialScale, percentage);
                return false;
            }


            public void EndExecute()
            {
                TargetTransform.localScale = TargetScale;
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