namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LerpScale_ToVector3_Executor : UpdateEffectExecutor<LerpScale_ToVector3_Executor.LerpScale>
    {
        [System.Serializable]
        public class LerpScale : UpdateEffect
        {
            [SerializeField]
            Transform _transform = default;

            [SerializeField]
            Vector3 _targetScale = default;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1;

            //Runtime
            Vector3 _initialScale = default;
            float _timer = default;

            public void BeginExecute()
            {
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
                
                float percentage = _timer / _duration;
                _transform.localScale = Vector3.Lerp(_targetScale, _initialScale, percentage);
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