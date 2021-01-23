namespace LinearEffects.DefaultEffects
{
    using UnityEngine;
    using UnityEngine.UI;

    ///<Summary>Lerp a graphic's alpha to a value</Summary>
    public class LerpGraphicAlpha_Executor : UpdateEffectExecutor<LerpGraphicAlpha_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : UpdateEffect
        {
            [Header("----- Target -----")]
            [Range(0, 1)]
            public float TargetAlpha = 0;

            public Graphic TargetGraphic = default;

            [Range(0, 1000)]
            public float Duration = 1f;

            //Runtime
            float _timer = default;
            float _startAlpha = default;
            Color _currentColour = default;


            public void BeginExecute()
            {
                _timer = Duration;
                _startAlpha = TargetGraphic.color.a;
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
                float percentage = (_timer / Duration);
                _currentColour = TargetGraphic.color;
                _currentColour.a = Mathf.Lerp(TargetAlpha, _startAlpha, percentage);
                TargetGraphic.color = _currentColour;

                return false;
            }

            public void EndExecution()
            {
                _currentColour = TargetGraphic.color;
                _currentColour.a = TargetAlpha;
                TargetGraphic.color = _currentColour;
            }


        }

        protected override void BeginExecuteEffect(MyEffect effectData)
        {
            effectData.BeginExecute();
        }

        protected override bool ExecuteEffect(MyEffect effectData)
        {
            return effectData.Execute();
        }

        protected override void EndExecuteEffect(MyEffect effectData)
        {
            effectData.EndExecution();
        }

    }


}