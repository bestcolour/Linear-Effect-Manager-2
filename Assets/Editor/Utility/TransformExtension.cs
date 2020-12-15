using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class TransformExtension
{
    ///<Summary>Gets the full path of a Transform which includes the scene name</Summary>
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

    ///<Summary>Checks if a scene name matches is found in the path</Summary>
    static bool CheckSceneNameIsPresent(string sceneName, string path, out string pathWithoutSceneName)
    {
        pathWithoutSceneName = string.Empty;

        //Checking if there is a scene name in the path
        int slashFound = path.IndexOf("/");
        if (slashFound == -1)
        {
#if UNITY_EDITOR
            Debug.Log($"There is no scene path in {path}");
#endif
            return false;
        }

        //===================== CURRENTNAME = SCENE NAME ===========================
        string sceneNameFound = path.Substring(0, slashFound);

        //Check if the scene name in path matches the Scene.name
        if (sceneNameFound != sceneName)
        {
#if UNITY_EDITOR
            Debug.Log($"The scene in {path} is not the same scene as {sceneName}");
#endif
            return false;
        }

        //Else the scene name matches up to the path's scene name
        pathWithoutSceneName = path.Remove(0, slashFound + 1);
        return true;
    }

    ///<Summary>Finds a transform given the full path of the Transform including the scene name</Summary>
    public static bool GetTransform(this Scene scene, string fullPath, out Transform transform)
    {
        //Check if the scene's name matches up to the path's scene name
        if (!CheckSceneNameIsPresent(scene.name, fullPath, out string pathWithoutSceneName))
        {
            transform = null;
            return false;
        }


        //=================== CURRENTNAME = ROOT GAMEOBJECT ==========================
        int slashFound = pathWithoutSceneName.IndexOf("/");
        string rootGameObjectName = slashFound == -1 ? pathWithoutSceneName : pathWithoutSceneName.Substring(0, slashFound);

        //Remove the rootgameobjectname's string
        string restOfThePath = rootGameObjectName.Remove(0, slashFound + 1);

        GameObject[] rootObjects = scene.GetRootGameObjects();
        for (int i = 0; i < rootObjects.Length; i++)
        {
            if (rootObjects[i].name != rootGameObjectName)
            {
                continue;
            }

            //Find the transform with the rest of the path
            transform = rootObjects[i].transform.Find(restOfThePath);

            //Since Find() might return null,
            return transform == null ? false : true;
        }

        //Else if none of the root object's names matched,
        transform = null;
        return false;
    }



}
