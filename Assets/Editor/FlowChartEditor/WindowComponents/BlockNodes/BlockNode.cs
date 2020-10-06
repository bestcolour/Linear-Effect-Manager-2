using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNode
{
    // #region Constants
    static readonly Rect NODEBLOCK_SIZE = new Rect(Vector2.zero, new Vector2(100f, 200f));
    // #endregion

    string _label;
    Rect _rect;

    public BlockNode()
    // public BlockNode(Vector2 position)
    {
        _rect = NODEBLOCK_SIZE;
        _label = "New Block";
    }

  


}
