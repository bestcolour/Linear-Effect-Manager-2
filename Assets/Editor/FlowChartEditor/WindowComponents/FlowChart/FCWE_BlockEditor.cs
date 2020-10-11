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
            _blockEditor = ScriptableObject.CreateInstance<BlockScriptableInstance>();
            OnSelectBlockNode += HandleSelectBlockNode;
        }

        void BlockEditor_OnDisable()
        {
            OnSelectBlockNode -= HandleSelectBlockNode;
        }

        // void BlockEditor_OnGUI()
        // {

        // }

        #endregion

        private void HandleSelectBlockNode(BlockNode block)
        {
            Debug.Log($"Block is: {block.ID}");
            Selection.activeObject = _blockEditor;
        }




    }

}