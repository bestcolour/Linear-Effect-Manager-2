namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;

    //The block editor handles the data of the currently editting FlowChart instance
    public partial class FlowChartWindowEditor : EditorWindow
    {
        BlockScriptableInstance _blockEditor;

        #region LifeTime
        void BlockEditor_OnEnable()
        {
            //Clear selection to prevent assemblyreload errors
            BlockEditor_OnNoBlockNodeFound();
            _blockEditor = ScriptableObject.CreateInstance<BlockScriptableInstance>();
            _blockEditor.OnCreation(_flowChart.gameObject);

            _blockEditor.OnSaveModifiedProperties += BlockEditor_HandleOnSaveModifiedProperties;
            OnSelectBlockNode += BlockEditor_HandleSelectBlockNode;
            OnNoBlockNodeFound += BlockEditor_OnNoBlockNodeFound;
        }



        void BlockEditor_OnDisable()
        {
            _blockEditor.OnSaveModifiedProperties -= BlockEditor_HandleOnSaveModifiedProperties;
            OnSelectBlockNode -= BlockEditor_HandleSelectBlockNode;
            OnNoBlockNodeFound -= BlockEditor_OnNoBlockNodeFound;
            Selection.activeObject = null;
        }

        #endregion


        private void BlockEditor_HandleSelectBlockNode(BlockNode node)
        {
            // Debug.Log($"Block is: {node.ID}");
            _blockEditor.ReadBlockNode(node);
            Selection.activeObject = _blockEditor;

        }

        private void BlockEditor_HandleOnSaveModifiedProperties(string prevName, string newName)
        {
            //If there is no change to the name of the blocknode,
            if (_allBlockNodesDictionary.ContainsKey(newName))
            {
                return;
            }

            //=========== THERE IS CHANGE TO BLOCKNODE NAME ===============
            BlockNode changedBlock = _allBlockNodesDictionary[prevName];

            //remove previous entry from dictionary (prev name)
            _allBlockNodesDictionary.Remove(prevName);

            //replace it with newName's entry
            _allBlockNodesDictionary.Add(newName, changedBlock);

        }

        void BlockEditor_OnNoBlockNodeFound()
        {
            Selection.activeObject = null;
        }

    }

}