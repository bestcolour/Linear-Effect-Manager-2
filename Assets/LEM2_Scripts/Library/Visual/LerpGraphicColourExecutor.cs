namespace LinearEffects.DefaultEffects
{
    using UnityEngine;
    using UnityEngine.UI;
    [System.Serializable]
    ///<Summary>Lerp a graphic's color to another color value</Summary>
    public class LerpGraphicColour_Executor : UpdateEffectExecutor<LerpGraphicColour_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : UpdateEffect
        {
            [Header("----- Colour References -----")]
            public Color TargetColor = Color.white;

            [SerializeField]
            public Graphic TargetGraphic = default;

            [SerializeField]
            [Range(0, 1000)]
            public float Duration = 1f;

            //Runtime
            float _timer = default;
            Color _startColour = default;


            public void StartExecute()
            {
                _timer = Duration;
                _startColour = TargetGraphic.color;
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
                TargetGraphic.color = Vector4.Lerp(TargetColor, _startColour, percentage);

                return false;
            }

            public void EndExecution()
            {
                TargetGraphic.color = TargetColor;
            }

        }


        protected override void BeginExecuteEffect(MyEffect effectData)
        {
            effectData.StartExecute();
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