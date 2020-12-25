namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LerpAnchoredPosition_ToVector3_Executor : UpdateEffectExecutor<LerpAnchoredPosition_ToVector3_Executor.LerpTransform>
    {
        [System.Serializable]
        public class LerpTransform : UpdateEffect
        {
            [SerializeField]
            RectTransform _transform = default;

            [SerializeField]
            Vector3 _targetPos = default;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1;

            //Runtime
            Vector3 _initialPosition = default;
            float _timer = default;

            public void BeginExecute()
            {
                _initialPosition = _transform.anchoredPosition;
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
                _transform.anchoredPosition = Vector3.Lerp(_targetPos, _initialPosition, percentage);
                return false;
            }


            public void EndExecute()
            {
                _transform.anchoredPosition = _targetPos;
            }


        }

        protected override void BeginExecuteEffect(LerpTransform effectData)
        {
            effectData.BeginExecute();
        }

        protected override void EndExecuteEffect(LerpTransform effectData)
        {
            effectData.EndExecute();
        }

        protected override bool ExecuteEffect(LerpTransform effectData)
        {
            return effectData.Execute();
        }


    }

}