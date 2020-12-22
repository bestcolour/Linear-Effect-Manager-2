namespace LinearEffects.General
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    [System.Serializable]
    public class LerpGraphicColourExecutor : UpdateEffectExecutor<LerpGraphicColourExecutor.LerpColourEffect>
    {
        [System.Serializable]
        public class LerpColourEffect : UpdateEffect
        {
            [Header("----- Colour References -----")]
            [SerializeField]
            Color _targetColour = Color.white;

            [SerializeField]
            Graphic _graphic = default;

            [SerializeField]
            [Range(0, 1000)]
            float _duration = 1f;

            //Runtime
            float _timer = default;
            Color _startColour = default;


            public void StartExecute()
            {
                _timer = _duration;
                _startColour = _graphic.color;
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
                _graphic.color = Vector4.Lerp(_targetColour, _startColour, percentage);

                return false;
            }

            public void EndExecution()
            {
                _graphic.color = _targetColour;
            }

        }


        protected override void BeginExecuteEffect(LerpColourEffect effectData)
        {
            effectData.StartExecute();
        }

        protected override void EndExecuteEffect(LerpColourEffect effectData)
        {
            effectData.EndExecution();
        }

        protected override bool ExecuteEffect(LerpColourEffect effectData)
        {
            return effectData.Execute();
        }
    }

}