namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    [System.Serializable]
    public class LerpCanvasGroupAlphaExecutor : UpdateEffectExecutor<LerpCanvasGroupAlphaExecutor.LerpAlpha>
    {
        [System.Serializable]
        public class LerpAlpha : UpdateEffect
        {
            [Header("----- Target -----")]
            [SerializeField]
            [Range(0, 1)]
            float _targeAlpha = 0;

            [SerializeField]
            CanvasGroup _canvasGroup = default;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1f;

            //Runtime
            float _timer = default;
            float _startAlpha = default;


            public void BeginExecute()
            {
                _timer = _duration;
                _startAlpha = _canvasGroup.alpha;
            }

            public bool Execute()
            {
                if (_timer <= 0)
                {
                    return true;
                }

                //Count down the timer
                _timer -= Time.deltaTime;

                //By inverting your start and target vector, you can skip the 1 - (_timer / _duration)
                float percentage = (_timer / _duration);
                _canvasGroup.alpha = Mathf.Lerp(_targeAlpha, _startAlpha, percentage);

                return false;
            }

            public void EndExecution()
            {
                _canvasGroup.alpha = _targeAlpha;
            }

        }


        protected override void BeginExecuteEffect(LerpAlpha effectData)
        {
            effectData.BeginExecute();
        }

        protected override void EndExecuteEffect(LerpAlpha effectData)
        {
            effectData.EndExecution();
        }

        protected override bool ExecuteEffect(LerpAlpha effectData)
        {
            return effectData.Execute();
        }
    }

}