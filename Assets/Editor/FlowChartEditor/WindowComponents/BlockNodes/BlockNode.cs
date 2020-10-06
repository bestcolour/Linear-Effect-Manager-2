namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;

    public class BlockNode
    {
        #region Constants
        static readonly Rect NODEBLOCK_SIZE = new Rect(Vector2.zero, new Vector2(100f, 200f));
        #endregion


        string _label;
        Rect _rect;

        public BlockNode(Vector2 position)
        {
            Debug.Log("Creatin new node");
            _label = "New Block";
            _rect = NODEBLOCK_SIZE;
            _rect.position = position;
        }

        // public void Initialize(Vector2 position)
        // {
        //     _label = "New Block";
        //     _rect = NODEBLOCK_SIZE;
        //     _rect.position = position;
        // }

        public void OnGUI()
        {
            Draw();
        }

        private void Draw()
        {

            GUI.Box(_rect, _label);

        }
    }

}