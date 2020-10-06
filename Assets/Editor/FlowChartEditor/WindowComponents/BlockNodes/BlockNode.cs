using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNode
{
    // #region Constants
    static readonly Rect NODEBLOCK_SIZE = new Rect(Vector2.zero, new Vector2(100f, 50f));
    // #endregion

    string _label;
    Rect _rect;

    public BlockNode(Vector2 position)
    {
        _rect = NODEBLOCK_SIZE;
        _rect.position = position;
        _label = "New Block";
    }


    public void Draw()
    {
        GUI.Box(_rect, _label);
    }


    public bool ProcessMouseDown()
    {
        Event e = Event.current;

        if (_rect.Contains(e.mousePosition, true))
        {
            return true;
        }
        return false;
    }

    public void ProcessMouseUp()
    {

    }



}
