namespace CategorizedSearchBox
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor.IMGUI.Controls;


    public partial class CategorizedSearchBox
    {
        #region Definitions
        public delegate void SearchBarTextChangeCallback(string newSearchBarText);
        public delegate void OnPressConfirmCallback(string fullResultPathPressed);
        public delegate void UpOrDownArrowPressedCallback(bool upArrowWasPressed);
        #endregion

        public void Initialize(string[] resultsToPopulate)
        {
            _searchBar = new SearchField();
            STYLE_RESULTS_EVEN = new GUIStyle("CN EntryBackOdd");
            STYLE_RESULTS_ODD = new GUIStyle("CN EntryBackEven");
            _library = new List<string>();
            _drawnCategories = new HashSet<string>();
            _library.AddRange(resultsToPopulate);

            //Need to call this to update on intit the results list 
            HandleSearchBarTextChange(string.Empty);
        }

        #region Enables & Disables

        //============ ENABLES ================
        public virtual void EnableSearchBox()
        {
            OnSearchBarTextChange += HandleSearchBarTextChange;
            UpOrDownArrowPressed += HandleDownOrUpArrowKeyPressed;
        }


        public virtual void EnableSearchBox(UpOrDownArrowPressedCallback handleDownOrUpArrowKeyPressed, SearchBarTextChangeCallback handleSearchBarTextChanged, OnPressConfirmCallback handleOnConfirmPressed)
        {
            EnableSearchBox();
            OnSearchBarTextChange += handleSearchBarTextChanged;
            UpOrDownArrowPressed += handleDownOrUpArrowKeyPressed;
            OnPressConfirm += handleOnConfirmPressed;
        }


        //Up down arrow keys pressed
        // public virtual void EnableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
        // {
        //     EnableSearchBox();
        //     downOrUpArrowKeyPressed += handleDownOrUpArrowKeyPressed;
        // }

        // //search bar text change
        // public virtual void EnableSearchBox(SearchBarTextChangeCallback handleSearchBarTextChange)
        // {
        //     EnableSearchBox();
        //     OnSearchBarTextChange += handleSearchBarTextChange;
        // }

        // //result confirmed and pressed
        // public virtual void EnableSearchBox(OnPressConfirmCallback handlePressConfirm)
        // {
        //     EnableSearchBox();
        //     OnPressConfirm += handlePressConfirm;
        // }

        // //up down + search text change
        // public virtual void EnableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed, Action handleSearchBarTextChange)
        // {
        //     EnableSearchBox(handleDownOrUpArrowKeyPressed);
        //     OnSearchBarTextChange += handleSearchBarTextChange;
        // }




        //========= DISABLES ===============
        public virtual void DisableSearchBox()
        {
            OnSearchBarTextChange -= HandleSearchBarTextChange;
            UpOrDownArrowPressed -= HandleDownOrUpArrowKeyPressed;
        }

        public virtual void DisableSearchBox(UpOrDownArrowPressedCallback handleDownOrUpArrowKeyPressed, SearchBarTextChangeCallback handleSearchBarTextChanged, OnPressConfirmCallback handleOnConfirmPressed)
        {
            DisableSearchBox();
            OnSearchBarTextChange -= handleSearchBarTextChanged;
            UpOrDownArrowPressed -= handleDownOrUpArrowKeyPressed;
            OnPressConfirm -= handleOnConfirmPressed;
        }

        // public virtual void DisableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed)
        // {
        //     DisableSearchBox();
        //     downOrUpArrowKeyPressed -= handleDownOrUpArrowKeyPressed;
        // }

        // public virtual void DisableSearchBox(SearchFieldCallback handleDownOrUpArrowKeyPressed, Action handleSearchBarChange)
        // {
        //     DisableSearchBox(handleDownOrUpArrowKeyPressed);
        //     OnSearchBarTextChange -= handleSearchBarChange;
        // }

        // public virtual void DisableSearchBox(Action handleSearchBarTextChange)
        // {
        //     DisableSearchBox();
        //     OnSearchBarTextChange -= handleSearchBarTextChange;
        // }


        #endregion



    }

}