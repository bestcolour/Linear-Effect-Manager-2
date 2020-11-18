﻿namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using LinearEffects;
    using CategorizedSearchBox;
    using System.Collections.Generic;

    public partial class BlockInspector : ImprovedEditor
    {

        #region SearchBox Fields
        bool _isSearchBoxOpened = false;
        CategorizedSearchBox _searchBox = default;

        #region Constants
        // static readonly Vector2 SEARCHBOX_RECTSIZE = new Vector2(500f, EditorGUIUtility.singleLineHeight);
        const float SEARCHBAR_PADDING_TOP = 10f
        , SEARCHBOX_HEIGHT = 75f
        , SEARCHBAR_PADDING_BOT = 10f
        ;

        #endregion

        #endregion

        void BottomHalf_SearchBox_OnEnable()
        {
            _searchBox = new CategorizedSearchBox();
            _searchBox.Initialize(CommandData.GetEffectStrings());
            _isSearchBoxOpened = false;
        }
        void BottomHalf_SearchBox_OnDisable()
        {
            if (_isSearchBoxOpened)
            {
                _searchBox.DisableSearchBox();
                _isSearchBoxOpened = false;
            }
        }


        #region SearchBox ToolBar
        void BottomHalf_DrawSearchBoxButtons()
        {
            //================ DRAW SEARCHBOX BUTTON ===============
            switch (_isSearchBoxOpened)
            {
                case true:
                    if (GUILayout.Button("【―】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
                    {
                        BottomHalf_DisableSearchBox();
                    }
                    break;
                case false:
                    if (GUILayout.Button("【＋】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
                    {

                        _isSearchBoxOpened = true;
                        _searchBox.EnableSearchBox(BottomHalf_SearchBox_HandleUpDownKeyPressed, BottomHalf_SearchBox_HandleSearchBarTextChange, BottomHalf_SearchBox_HandleOnConfirm);
                    }

                    break;
            }
        }

        void BottomHalf_DisableSearchBox()
        {
            _isSearchBoxOpened = false;
            _searchBox.DisableSearchBox(BottomHalf_SearchBox_HandleUpDownKeyPressed, BottomHalf_SearchBox_HandleSearchBarTextChange, BottomHalf_SearchBox_HandleOnConfirm);
        }



        #endregion


        #region SearchBox Methods
        void BottomHalf_SearchBox_OnGUI()
        {
            //Draw only when opened
            if (!_isSearchBoxOpened) return;

            float height = _searchBox.Handle_OnGUI(BottomHalf_SearchBox_GetSearchBarRect(), SEARCHBOX_HEIGHT);

            //This ensures that the search box will always have enough space to be rendered (and if it cant fit in the the window then it will be considered as part of the scroll height)
            EditorGUILayout.LabelField(string.Empty, GUILayout.MinHeight(height + SEARCHBAR_PADDING_BOT));
        }

        Rect BottomHalf_SearchBox_GetSearchBarRect()
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            rect.y += rect.height + SEARCHBAR_PADDING_TOP;
            rect.height = EditorGUIUtility.singleLineHeight;
            return rect;
        }

        #region Handle Searchbox events
        void BottomHalf_SearchBox_HandleUpDownKeyPressed(bool isUpKeyPressed)
        {
            // Debug.Log($"IsUpKeyPressed: {isUpKeyPressed}");
            Repaint();

        }

        void BottomHalf_SearchBox_HandleSearchBarTextChange(string newSearchBarText)
        {
            // Debug.Log($"newSearchBarText: {newSearchBarText}");

        }

        void BottomHalf_SearchBox_HandleOnConfirm(string fullPathOfResultPressed)
        {
            // Debug.Log($"fullPathOfResultPressed: {fullPathOfResultPressed}");
            BottomHalf_AddEffect(fullPathOfResultPressed);
            BottomHalf_DisableSearchBox();
            Repaint();
        }

        #endregion

        #endregion

        #region Add Effect Command
        void BottomHalf_AddEffect(string effectorName)
        {
            if (!CommandData.TryGetExecutor(effectorName, out Type type))
            {
                return;
            }

            //Since the effector name has slashes inside of it (cause categorization and stuff),there is a need to remove it 
            int previousCategoryIndex = effectorName.LastIndexOf(CategorizedSearchBox.CATEGORY_IDENTIFIER);
            effectorName = previousCategoryIndex == -1 ? effectorName : effectorName.Remove(0, previousCategoryIndex + 1);

            _target.Block.AddNewOrderElement(BlockGameObject, type, effectorName);
            _target.SaveModifiedProperties();

        }
        #endregion

    }
}