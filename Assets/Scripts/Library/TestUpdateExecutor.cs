namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class TestUpdateExecutor : UpdateEffectExecutor<TestUpdateExecutor.TestUpdateEffect>
    {


        [System.Serializable]
        public class TestUpdateEffect : Effect
        {
            public string _myAwesomeName = "DDDD";

            public GameObject _prefab = default;

        }

        protected override bool ExecuteEffect(TestUpdateEffect effectData)
        {
            return true;
        }
    }



}