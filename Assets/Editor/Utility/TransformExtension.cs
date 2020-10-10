using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class TransformExtension
{
    public static string GetFullPath(this Transform t)
    {
        string path = t.name;

        while (t.parent != null)
        {
            path = path.Insert(0, $"{t.parent.name}/");
            t = t.parent;
        }

        //Append scene name
        path = path.Insert(0, $"{t.root.gameObject.scene.name}/");
        return path;
    }

    public static bool GetTransform(this Scene scene, string path, out Transform transform)
    {
        string originalPath = string.Copy(path);
        //Checking if there is a scene name in the path
        int slashFound = path.IndexOf("/");
        if (slashFound == -1)
        {
            Debug.Log($"There is no scene path in {path}");
            transform = null;
            return false;
        }

        //===================== CURRENTNAME = SCENE NAME ===========================
        string currentName = path.Substring(0, slashFound);
        //Remove the everything to the slash (inclusive of slash)
        path = path.Remove(0, slashFound + 1);

        //Check if the scene name in path matches the Scene.name
        if (currentName != scene.name)
        {
            Debug.Log($"The scene in {originalPath} is not the same scene as {scene.name}");
            transform = null;
            return false;
        }

        //=================== CURRENTNAME = ROOT GAMEOBJECT ==========================
        slashFound = path.IndexOf("/");
        currentName = path.Substring(0, slashFound);
        path = path.Remove(0, slashFound + 1);

        GameObject[] rootObjects = scene.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; i++)
        {
            if (rootObjects[i].name != currentName)
            {
                continue;
            }

            //Find the transform with the rest of the path
            transform = rootObjects[i].transform.Find(path);
            return true;
        }

        //Else if none of the root object's names matched,
        transform = null;
        return false;
    }



}
