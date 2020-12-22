namespace LinearEffects.General
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class LerpGraphicAlphaExecutor : UpdateEffectExecutor<LerpGraphicAlphaExecutor.LerpAlphaEffect>
    {
        [System.Serializable]
        public class LerpAlphaEffect : UpdateEffect
        {
            [Header("----- Target -----")]
            [SerializeField]
            [Range(0, 1)]
            float _targeAlpha = 0;

            [SerializeField]
            Graphic _graphic = default;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1f;

            //Runtime
            float _timer = default;
            float _startAlpha = default;
            Color _currentColour = default;


            public void BeginExecute()
            {
                _timer = _duration;
                _startAlpha = _graphic.color.a;
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
                _currentColour = _graphic.color;
                _currentColour.a = Mathf.Lerp(_targeAlpha, _startAlpha, percentage);
                _graphic.color = _currentColour;

                return false;
            }

            public void EndExecution()
            {
                _currentColour = _graphic.color;
                _currentColour.a = _targeAlpha;
                _graphic.color = _currentColour;
            }


        }

        protected override void BeginExecuteEffect(LerpAlphaEffect effectData)
        {
            effectData.BeginExecute();
        }

        protected override bool ExecuteEffect(LerpAlphaEffect effectData)
        {
            return effectData.Execute();
        }

        protected override void EndExecuteEffect(LerpAlphaEffect effectData)
        {
            effectData.EndExecution();
        }

    }


}