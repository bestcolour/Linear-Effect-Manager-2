namespace LinearCommandsEditor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using LinearCommands;

    //The bottom half class will render the current observed command as well as the command toolbar (add,minus coppy etc)
    public class BlockEditor_BottomHalf
    {
        #region Cached Variables
        SerializedObject serializedObject = default;
        #endregion


        #region LifeTime Method
        public void OnEnable(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
        }


        public void OnDisable()
        {
            serializedObject = null;
        }


        public void OnInspectorUpdate(Vector2 windowSize)
        {
            
            EditorGUILayout.HelpBox("Hello world",MessageType.Info);
        }
        #endregion
    }

}