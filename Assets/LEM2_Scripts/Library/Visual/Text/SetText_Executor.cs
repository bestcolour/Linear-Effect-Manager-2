namespace LinearEffects.DefaultEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    [System.Serializable]
    ///<Summary>Sets the UnityEngine.UI text component's text value</Summary>
    public class SetText_Executor : EffectExecutor<SetText_Executor.MyEffect>
    {
        [System.Serializable]
        public class MyEffect : Effect
        {
            [Header("----- Play Block -----")]
            public Text TextUI = default;

            [TextArea]
            public string TextToSet = default;
        }


        protected override bool ExecuteEffect(MyEffect effectData)
        {
            effectData.TextUI.text = effectData.TextToSet;
            return true;
        }


    }

}