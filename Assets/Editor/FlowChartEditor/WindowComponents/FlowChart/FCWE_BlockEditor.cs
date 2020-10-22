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
            OnSelectBlockNode += BlockEditor_HandleSelectBlockNode;
            OnNoBlockNodeFound += BlockEditor_OnNoBlockNodeFound;
        }

        void BlockEditor_OnDisable()
        {
            OnSelectBlockNode -= BlockEditor_HandleSelectBlockNode;
            OnNoBlockNodeFound -= BlockEditor_OnNoBlockNodeFound;
            Selection.activeObject = null;
        }

        #endregion


        private void BlockEditor_HandleSelectBlockNode(BlockNode node)
        {
            // Debug.Log($"Block is: {node.ID}");
            _blockEditor.Initialize(node);
            Selection.activeObject = _blockEditor;

        }

        void BlockEditor_OnNoBlockNodeFound()
        {
            Selection.activeObject = null;
        }

    }

}