namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;

    //Handles the saving and loading of main FlowChartWindow level data ie. FlowChart _flowChart variable
    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Constants
        const string EDITORPREFS_PREV_FLOWCHART_SCENEPATH = "FlowChartPath";
        #endregion

        void SaveManager_SaveFlowChartPath()
        {
            if (_flowChart == null) return;
            string path =  _flowChart.transform.GetFullPath();
            EditorPrefs.SetString(EDITORPREFS_PREV_FLOWCHART_SCENEPATH, path);
        }

        FlowChart SaveManager_TryLoadFlowChartPath()
        {
            string path = EditorPrefs.GetString(EDITORPREFS_PREV_FLOWCHART_SCENEPATH);
            if (path == string.Empty)
            {
                return null;
            }

            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                Scene loadedScene = EditorSceneManager.GetSceneAt(i);
                if (!loadedScene.GetTransform(path, out Transform flowChartTransform))
                {
                    continue;
                }

                if (!flowChartTransform.TryGetComponent<FlowChart>(out FlowChart flowChart))
                {
                    continue;
                }

                return flowChart;
            }


            //Else if no flowchart is found
            return null;

        }

        FlowChart SaveManager_TryLoadFlowChartPath_Runtime()
        {
            string path = EditorPrefs.GetString(EDITORPREFS_PREV_FLOWCHART_SCENEPATH);
            if (path == string.Empty)
            {
                return null;
            }

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (!loadedScene.GetTransform(path, out Transform flowChartTransform))
                {
                    continue;
                }

                if (!flowChartTransform.TryGetComponent<FlowChart>(out FlowChart flowChart))
                {
                    continue;
                }

                return flowChart;
            }

            //Else if no flowchart is found
            return null;

        }
    }

}