namespace LinearEffects
{
    using UnityEditor;
    using UnityEngine;

    public interface ISavableData
    {
        void SaveToSerializedProperty(SerializedProperty property);
        void LoadFromSerializedProperty(SerializedProperty property);
    }

#if UNITY_EDITOR
    #region    //============================== SERIALIZED PROPERTY EXTENSIONS ================================
    public static class BlockSerializedPropertyExtension
    {
        //Used wehn the window editor adds a new nodeblock 
        public static SerializedProperty AddToSerializedPropertyArray<T>(this SerializedProperty array, T instance) where T : ISavableData
        {
            if (!array.isArray)
            {
                Debug.Log($"Serialized Property {array.name} is not an array!");
                return null;
            }

            array.serializedObject.Update();
            array.arraySize++;
            SerializedProperty lastElement = array.GetArrayElementAtIndex(array.arraySize - 1);
            instance.SaveToSerializedProperty(lastElement);
            array.serializedObject.ApplyModifiedProperties();
            return lastElement;
        }
    }
    #endregion
#endif

}