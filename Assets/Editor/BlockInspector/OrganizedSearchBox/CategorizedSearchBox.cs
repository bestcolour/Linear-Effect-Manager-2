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

    string _searchedBarText = String.Empty;
    #endregion

    #region SearchBox Fields
    Rect _boxRect = default;
    Vector2 _scrollPosition = default;
    int _maxNumberOfOptions = default;

    public bool IsInSearchBox => _boxRect.Contains(Event.current.mousePosition, true);

    #region Constants

    const float BAR_CANCELICON_WIDTH = 17.5f;
    static readonly Color BOX_OPTION_EVEN_COLOUR = Color.white;
    static readonly Color BOX_OPTION_ODD_COLOUR = Color.gray;
    #endregion

    #endregion

    public virtual void EnableSearchBox()
    {
    }
    public virtual void EnableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
    {
        downOrUpArrowKeyPressed += handleDownOrUpArrowKeyPressed;
    }


    public virtual void DisableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
    {
        downOrUpArrowKeyPressed -= handleDownOrUpArrowKeyPressed;
    }
    public virtual void DisableSearchBox()
    {
    }

    ///<Summary>
    ///Returns the height that the entire searchbox will need to occupy
    ///</Summary>
    public virtual float Handle_OnGUI(Rect searchBarRect, float searchBoxHeight)
    {
        //========= SEARCHBAR ==============
        SearchBar_OnGUI(searchBarRect);

        //=========== SEARCHBOX ===============
        //Substract the cross icon's width
        SearchBox_OnGUI(searchBoxHeight);


        return _barRect.height + _boxRect.height;
    }


    #region Search Bar Methods
    protected void SearchBar_OnGUI(Rect rect)
    {
        //====== UPDATE BAR RECT =========
        _barRect = rect;

        _searchedBarText = OnGUI(_barRect, _searchedBarText);
    }


    #endregion

    #region Search Box Methods
    void SearchBox_OnGUI(float searchBoxHeight)
    {
        SearchBox_UpdateValues(searchBoxHeight);

        //========== DRAW BOX BG =========
        // EditorGUI.DrawRect(_boxRect, GUI.color);

        SearchBox_DrawOptions();
    }

    void SearchBox_UpdateValues(float searchBoxHeight)
    {
        //must occur after _barRect updates itself
        //====== UPDATE BOX RECT =========
        _boxRect.x = _barRect.x;
        _boxRect.y = _barRect.y + _barRect.height;
        _boxRect.width = _barRect.width - BAR_CANCELICON_WIDTH;
        _boxRect.height = searchBoxHeight;


        _maxNumberOfOptions = Mathf.FloorToInt(_boxRect.height / EditorGUIUtility.singleLineHeight);
    }

    void SearchBox_DrawOptions()
    {
        // ============ DRAW A SPACE ===============
        EditorGUILayout.Space(_barRect.y + _barRect.height);

        // ============ SCROLL VIEW ================
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.MinWidth(_boxRect.width), GUILayout.MinHeight(_boxRect.height));

        for (int i = 0; i < _maxNumberOfOptions; i++)
        {
            Color prevColour = i % 2 == 0 ? GUIExtensions.Start_GUIBg_ColourChange(BOX_OPTION_EVEN_COLOUR) : GUIExtensions.Start_GUI_ColourChange(BOX_OPTION_ODD_COLOUR);
            EditorGUILayout.LabelField(i.ToString());

            GUIExtensions.End_GUIBg_ColourChange(prevColour);
        }

        GUILayout.EndScrollView();

    }

    #endregion



}
