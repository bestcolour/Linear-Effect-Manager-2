namespace LinearCommandsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearCommands;

    //The bottom half class will render the current observed command as well as the command toolbar (add,minus coppy etc)
    public class BlockEditor_BottomHalf 
    {
        #region Cached Variables
        SerializedObject serializedObject = default;

        Vector2 _scrollPosition = default;
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
            // _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(windowSize.x), GUILayout.Height(windowSize.y));

            EditorGUILayout.HelpBox("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque et cursus libero. Nunc ac scelerisque sapien, eget vestibulum dui. Quisque sed congue nibh. Ut hendrerit, lectus id tempor elementum, velit leo interdum ex, in gravida lectus magna ac leo. Suspendisse potenti. Praesent a nibh id magna bibendum pellentesque quis sed mauris. Ut et elit dui. Sed at pulvinar libero. Duis et ex purus. Morbi sit amet odio et massa aliquam porttitor vitae vitae justo. Quisque elementum felis sit amet ipsum dictum, in laoreet tellus fringilla. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris tortor mauris, consectetur at libero in, ultrices pharetra velit. Proin rhoncus, augue non tristique luctus, arcu lorem scelerisque ipsum, vitae suscipit augue elit vel risus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Fusce eget ante placerat, auctor massa vel, sodales urna.", MessageType.Info);
            EditorGUILayout.HelpBox("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque et cursus libero. Nunc ac scelerisque sapien, eget vestibulum dui. Quisque sed congue nibh. Ut hendrerit, lectus id tempor elementum, velit leo interdum ex, in gravida lectus magna ac leo. Suspendisse potenti. Praesent a nibh id magna bibendum pellentesque quis sed mauris. Ut et elit dui. Sed at pulvinar libero. Duis et ex purus. Morbi sit amet odio et massa aliquam porttitor vitae vitae justo. Quisque elementum felis sit amet ipsum dictum, in laoreet tellus fringilla. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris tortor mauris, consectetur at libero in, ultrices pharetra velit. Proin rhoncus, augue non tristique luctus, arcu lorem scelerisque ipsum, vitae suscipit augue elit vel risus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Fusce eget ante placerat, auctor massa vel, sodales urna.", MessageType.Info);
            EditorGUILayout.HelpBox("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque et cursus libero. Nunc ac scelerisque sapien, eget vestibulum dui. Quisque sed congue nibh. Ut hendrerit, lectus id tempor elementum, velit leo interdum ex, in gravida lectus magna ac leo. Suspendisse potenti. Praesent a nibh id magna bibendum pellentesque quis sed mauris. Ut et elit dui. Sed at pulvinar libero. Duis et ex purus. Morbi sit amet odio et massa aliquam porttitor vitae vitae justo. Quisque elementum felis sit amet ipsum dictum, in laoreet tellus fringilla. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris tortor mauris, consectetur at libero in, ultrices pharetra velit. Proin rhoncus, augue non tristique luctus, arcu lorem scelerisque ipsum, vitae suscipit augue elit vel risus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Fusce eget ante placerat, auctor massa vel, sodales urna.", MessageType.Info);
            EditorGUILayout.HelpBox("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque et cursus libero. Nunc ac scelerisque sapien, eget vestibulum dui. Quisque sed congue nibh. Ut hendrerit, lectus id tempor elementum, velit leo interdum ex, in gravida lectus magna ac leo. Suspendisse potenti. Praesent a nibh id magna bibendum pellentesque quis sed mauris. Ut et elit dui. Sed at pulvinar libero. Duis et ex purus. Morbi sit amet odio et massa aliquam porttitor vitae vitae justo. Quisque elementum felis sit amet ipsum dictum, in laoreet tellus fringilla. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris tortor mauris, consectetur at libero in, ultrices pharetra velit. Proin rhoncus, augue non tristique luctus, arcu lorem scelerisque ipsum, vitae suscipit augue elit vel risus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Fusce eget ante placerat, auctor massa vel, sodales urna.", MessageType.Info);
            // EditorGUILayout.EndScrollView();
        }
        #endregion
    }

}