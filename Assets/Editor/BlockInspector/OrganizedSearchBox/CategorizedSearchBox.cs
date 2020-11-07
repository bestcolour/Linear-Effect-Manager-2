using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public class CategorizedSearchBox : SearchField
{
    #region SearchBar Fields
    Rect _barRect = default;

    string _searchedBarText = "Testing";
    #endregion

    #region SearchBox Fields
    Rect _boxRect = default;

    public bool IsInSearchBox => _boxRect.Contains(Event.current.mousePosition, true);

    #region Constants

    const float BAR_CANCELICON_WIDTH = 17.5f;
    #endregion

    #endregion


    public virtual void EnableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
    {
        downOrUpArrowKeyPressed += handleDownOrUpArrowKeyPressed;
    }


    public virtual void DisableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
    {
        downOrUpArrowKeyPressed -= handleDownOrUpArrowKeyPressed;
    }

    ///<Summary>
    ///Returns the height that the entire searchbox will need to occupy
    ///</Summary>
    public virtual float Handle_OnGUI(Vector2 searchBarSize, float searchBoxHeight)
    {
        //========= SEARCHBAR ==============
        SearchBar_OnGUI(searchBarSize);

        //=========== SEARCHBOX ===============
        //Substract the cross icon's width
        SearchBox_OnGUI(searchBoxHeight);


        return _barRect.height + _boxRect.height;
    }


    #region Search Bar Methods
    protected void SearchBar_OnGUI(Vector2 size)
    {
        //====== UPDATE BAR RECT =========
        _barRect.width = size.x;
        _barRect.height = size.y;

        _searchedBarText = OnGUI(_barRect, _searchedBarText);
    }


    #endregion

    #region Search Box Methods
    void SearchBox_OnGUI(float searchBoxHeight)
    {
        SearchBox_UpdateBoxRect(searchBoxHeight);

        //========== DRAW BOX BG =========
        EditorGUI.DrawRect(_boxRect, GUI.color);




    }

    void SearchBox_UpdateBoxRect(float searchBoxHeight)
    {
        //must occur after _barRect updates itself
        //====== UPDATE BOX RECT =========
        _boxRect.x = _barRect.x;
        _boxRect.y = _barRect.y + _barRect.height;
        _boxRect.width = _barRect.width - BAR_CANCELICON_WIDTH;
        _boxRect.height = searchBoxHeight;
    }

    #endregion



}
