namespace LinearCommandsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearCommands;
    using System;

    [CustomEditor(typeof(Block))]
    public class BlockEditor : Editor
    {
        #region Components
        BlockEditor_TopHalf _topHalf = new BlockEditor_TopHalf();
        BlockEditor_BottomHalf _bottomHalf = new BlockEditor_BottomHalf();
        BlockEditor_CenterDivision _centerDivision = new BlockEditor_CenterDivision();
        #endregion

        #region Constants
        #endregion


        float _ratioOfTopHalfToInspectorHeight = 0.5f;

        #region LifeTime Methods
        private void OnEnable()
        {
            _topHalf.OnEnable(serializedObject);
            _bottomHalf.OnEnable(serializedObject);
            _centerDivision.OnEnable();
            _centerDivision.OnDrag += HandleDivisonDrag;
        }

        private void OnDisable()
        {
            _topHalf.OnDisable();
            _bottomHalf.OnDisable();
            _centerDivision.OnDisable();
            _centerDivision.OnDrag -= HandleDivisonDrag;

        }

        //Calll the reorderable list to update itself
        public override void OnInspectorGUI()
        {
            //Initialise each halve's sizes
            Vector2 topHalfSize;
            topHalfSize.x = Screen.width * 0.725f;
            topHalfSize.y = _ratioOfTopHalfToInspectorHeight * Screen.height;


            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            _topHalf.OnInspectorGUI(topHalfSize);

            _centerDivision.OnInspectorGUI(Screen.width);

            _bottomHalf.OnInspectorGUI();

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }


        #endregion

        #region  HandleEvents
        private void HandleDivisonDrag(float mouseDeltaY)
        {
            if (mouseDeltaY == 0) return;

            float scaleMultiplier = 0.0015f;
            mouseDeltaY *= scaleMultiplier;
            _ratioOfTopHalfToInspectorHeight += mouseDeltaY;
            _ratioOfTopHalfToInspectorHeight = Mathf.Clamp(_ratioOfTopHalfToInspectorHeight, 0.1f, 0.9f);
        }
        #endregion

    }
}