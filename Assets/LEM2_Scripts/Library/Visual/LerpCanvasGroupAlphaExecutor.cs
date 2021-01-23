namespace LinearEffects.DefaultEffects
{
    using UnityEngine;
    ///<Summary>Lerps a canvas group's alpha to a value over time</Summary>
    [System.Serializable]
    public class LerpCanvasGroupAlpha_Executor : UpdateEffectExecutor<LerpCanvasGroupAlpha_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : UpdateEffect
        {
            [Header("----- Target -----")]
            [Range(0, 1)]
            public float TargetAlpha = 0;

            [SerializeField]
            public CanvasGroup TargetGroup = default;

            [SerializeField]
            [Range(0, 1000)]
            public float Duration = 1f;

            //Runtime
            float _timer = default;
            float _startAlpha = default;


            public void BeginExecute()
            {
                _timer = Duration;
                _startAlpha = TargetGroup.alpha;
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
                TargetGroup.alpha = Mathf.Lerp(TargetAlpha, _startAlpha, percentage);

                return false;
            }

            public void EndExecution()
            {
                TargetGroup.alpha = TargetAlpha;
            }

        }

        protected override void BeginExecuteEffect(MyEffect effectData)
        {
            effectData.BeginExecute();
        }

        protected override void EndExecuteEffect(MyEffect effectData)
        {
            effectData.EndExecution();
        }

        protected override bool ExecuteEffect(MyEffect effectData)
        {
            return effectData.Execute();
        }
    }

}