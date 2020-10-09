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
        int slashFound = path.IndexOf("/");
        if (slashFound == -1)
        {
            Debug.Log($"There is no scene path in {path}");
            transform = null;
            return false;
        }

        //===================== SCENE NAME ===========================
        string currentName = path.Substring(0, slashFound);

        if (currentName != scene.name)
        {
            Debug.Log($"The scene in {path} is not the same scene as {scene.name}");
            transform = null;
            return false;
        }

        path = path.Remove(0, slashFound);
        slashFound = path.IndexOf("/");

        //if slash cant be found, that might mean that the current string is the root already
        GameObject rootGO;
        if (slashFound == -1)
        {
            rootGO = GameObject.Find(path);

            if (rootGO == null)
            {
                Debug.Log($"There is no GameObject with the name {path}!");
                transform = null;
                return false;
            }

            transform = rootGO.transform;
            return true;
        }

        //===================== ROOT NAME ===========================
        currentName = path.Substring(0, slashFound);
        path = path.Remove(0, slashFound);
        rootGO = GameObject.Find(currentName);

        if (rootGO == null)
        {
            Debug.Log($"There is no GameObject with the name {currentName}!");
            transform = null;
            return false;
        }

        transform = rootGO.transform.Find(path);
        return true;
    }



}
