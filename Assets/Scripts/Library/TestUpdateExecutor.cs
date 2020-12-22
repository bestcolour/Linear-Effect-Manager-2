namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class TestUpdateExecutor : EffectExecutor<TestUpdateExecutor.TestUpdateEffect>
    {
        [System.Serializable]
        public class TestUpdateEffect : Effect
        {
            public string _myAwesomeName = "DDDD";
            public GameObject _prefab = default;

            public void LogMyStuff()
            {
                Debug.Log($"My awesome name is {_myAwesomeName} and the prefab is {_prefab}", _prefab);
            }

        }

        protected override bool ExecuteEffect(TestUpdateEffect effectData)
        {
            effectData.LogMyStuff();
            return true;
        }
    }



}