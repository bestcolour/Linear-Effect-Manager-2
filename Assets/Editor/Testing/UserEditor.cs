using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(User))]
public class UserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // User user = (User)target;

        if (GUILayout.Button("Add"))
        {
        }
        if (GUILayout.Button("Remove At"))
        {

        }
        if (GUILayout.Button("Insert"))
        {

        }



        base.OnInspectorGUI();
    }
}
