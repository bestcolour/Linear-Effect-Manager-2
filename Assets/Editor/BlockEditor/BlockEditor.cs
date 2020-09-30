namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
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
        const string EDITORPREFS_HEIGHTRATIO = "TopHalf_To_Height";
        const float DEFAULT_HEIGHTRATIO = 0.5f;
        #endregion


        Block _target = null;
        float _ratioOfTopHalfToInspectorHeight = DEFAULT_HEIGHTRATIO;

        #region LifeTime Methods
        private void OnEnable()
        {
            _target = (Block)target;

            _topHalf.OnEnable(serializedObject);
            _bottomHalf.OnEnable(_target);
            _centerDivision.OnEnable();
            _centerDivision.OnDrag += HandleDivisonDrag;

            Load();
        }

        private void OnDisable()
        {
            _topHalf.OnDisable();
            _bottomHalf.OnDisable();
            _centerDivision.OnDisable();
            _centerDivision.OnDrag -= HandleDivisonDrag;

            Save();
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

            _bottomHalf.OnInspectorGUI(Screen.width);

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


        #region Saving Editor's Preferences
        void Load()
        {

            if (!EditorPrefs.HasKey(EDITORPREFS_HEIGHTRATIO))
            {
                EditorPrefs.SetFloat(EDITORPREFS_HEIGHTRATIO, DEFAULT_HEIGHTRATIO);
                _ratioOfTopHalfToInspectorHeight = DEFAULT_HEIGHTRATIO;
                return;
            }

            _ratioOfTopHalfToInspectorHeight = EditorPrefs.GetFloat(EDITORPREFS_HEIGHTRATIO);
        }

        void Save()
        {
            EditorPrefs.SetFloat(EDITORPREFS_HEIGHTRATIO, _ratioOfTopHalfToInspectorHeight);
        }
        #endregion

    }
}