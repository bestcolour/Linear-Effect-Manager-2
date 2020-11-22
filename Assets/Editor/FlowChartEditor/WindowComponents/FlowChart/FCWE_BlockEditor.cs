﻿namespace LinearEffectsEditor
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
            BlockEditor_HandleOnNoBlockNodeFound();
            _blockEditor = ScriptableObject.CreateInstance<BlockScriptableInstance>();
            _blockEditor.OnCreation(_flowChart.gameObject);

            _blockEditor.OnVerifyBlockNameChange = BlockEditor_HandleVerifyBlockNameChange;
            OnSelectBlockNode += BlockEditor_HandleSelectBlockNode;
            OnNoBlockNodeFound += BlockEditor_HandleOnNoBlockNodeFound;
        }



        void BlockEditor_OnDisable()
        {
            _blockEditor.OnVerifyBlockNameChange = null;
            OnSelectBlockNode -= BlockEditor_HandleSelectBlockNode;
            OnNoBlockNodeFound -= BlockEditor_HandleOnNoBlockNodeFound;
            Selection.activeObject = null;
        }

        #endregion

        #region Handle Methods

        private void BlockEditor_HandleSelectBlockNode(BlockNode node)
        {
            // Debug.Log($"Block is: {node.ID}");
            _blockEditor.ReadBlockNode(node);
            Selection.activeObject = _blockEditor;

        }

        #region  OnVerify BlockName Change
        private bool BlockEditor_HandleVerifyBlockNameChange(string prevName, string newName, out string uniqueName)
        {
            uniqueName = "";

            //If there is already an entry inside of the dictionary with that given newName,
            if (_allBlockNodesDictionary.ContainsKey(newName))
            {
                uniqueName = NodeManager_NodeCycler_GetUniqueBlockName(newName);
                BlockEditor_RenameDictionaryKey(prevName, uniqueName);
                return false;
            }

            BlockEditor_RenameDictionaryKey(prevName, newName);

            return true;

        }

        void BlockEditor_RenameDictionaryKey(string keyPrevName, string keyNewName)
        {
            BlockNode changedBlock = _allBlockNodesDictionary[keyPrevName];

            //remove previous entry from dictionary (prev name)
            _allBlockNodesDictionary.Remove(keyPrevName);

            //replace it with newName's entry
            _allBlockNodesDictionary.Add(keyNewName, changedBlock);
        }
        #endregion


        void BlockEditor_HandleOnNoBlockNodeFound()
        {
            Selection.activeObject = null;
        }

        #endregion


    }

}