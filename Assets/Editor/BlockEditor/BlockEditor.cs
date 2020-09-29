namespace LinearCommandsEditor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using LinearCommands;

    [CustomEditor(typeof(Block))]
    public class BlockEditor : Editor
    {
        BlockEditor_TopHalf _commandList = new BlockEditor_TopHalf();



        #region LifeTime Methods
        private void OnEnable()
        {
            _commandList.OnEnable(serializedObject);
        }

        private void OnDisable()
        {
            _commandList.OnDisable();
        }


        //Calll the reorderable list to update itself
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _commandList.OnInspectorGUI();


            serializedObject.ApplyModifiedProperties();
        }

        #endregion




    }
}