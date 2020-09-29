﻿using UnityEngine;
using UnityEditor;

public static class EditorDebugExtension
{
    #region  Debug
    //Just a debug method
    public static void PrintAllProperties(SerializedObject serializedObject)
    {
        SerializedProperty firstP = serializedObject.GetIterator();
        while (firstP.NextVisible(true))
        {
            Debug.Log(firstP.name);
        };
    }


    public static bool TryGetProperty(SerializedProperty p, string propertyName, out SerializedProperty propertyFound)
    {
        propertyFound = p.FindPropertyRelative(propertyName);
        if (propertyFound != null)
        {
            return true;
        }

        Debug.LogWarning($"The property named: {propertyName} inside the Block class has been renamed to something else or it doesnt exist anymore!");
        return false;
    }
    #endregion

}