namespace LinearCommandsEditor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using LinearCommands;
    using System;

    [CustomEditor(typeof(Block))]
    public class BlockEditor : Editor
    {
        #region Components
        BlockEditor_TopHalf _topHalf = new BlockEditor_TopHalf();
        BlockEditor_BottomHalf _bottomHalf = new BlockEditor_BottomHalf();
        #endregion

        #region Constants
        static readonly Vector2 STARTING_TOP_TO_BOT_HEIGHTRATIO = Vector2.one * 0.5f;
        #endregion


        //X represents top's height, Y represents bot's height 
        //This value will be manipulated by a middle section so that the player can resize the 2 halves
        Vector2 _topToBottomHeightRatio = STARTING_TOP_TO_BOT_HEIGHTRATIO;

        #region LifeTime Methods
        private void OnEnable()
        {
            _topHalf.OnEnable(serializedObject);
            _bottomHalf.OnEnable(serializedObject);

        }


        private void OnDisable()
        {
            _topHalf.OnDisable();
            _bottomHalf.OnDisable();

        }


        //Calll the reorderable list to update itself
        public override void OnInspectorGUI()
        {
            //Initialise each halve's sizes
            Vector2 topHalfSize, bottomHalfSize;
            topHalfSize.x = bottomHalfSize.x = Screen.width * 0.725f;
            topHalfSize.y = _topToBottomHeightRatio.x * Screen.height;
            bottomHalfSize.y = _topToBottomHeightRatio.y * Screen.height;


            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            _topHalf.OnInspectorGUI(topHalfSize);


            _bottomHalf.OnInspectorUpdate(bottomHalfSize);

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }


        #endregion




    }
}