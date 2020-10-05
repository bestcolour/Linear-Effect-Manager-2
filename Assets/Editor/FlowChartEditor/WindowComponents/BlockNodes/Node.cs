namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;

    public class Node
    {
        #region Constants
        static readonly Rect NODEBLOCK_SIZE = new Rect(Vector2.zero, new Vector2(100f, 200f));
        #endregion


        string _label = "New Block";
        Rect _rect = NODEBLOCK_SIZE;

        public void Initialize(Vector2 position)
        {
            _rect.position = position;
        }

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