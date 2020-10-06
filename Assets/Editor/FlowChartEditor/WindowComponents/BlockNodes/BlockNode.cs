namespace LinearEffectsEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BlockNode
    {
        // #region Constants
        static readonly Rect NODEBLOCK_SIZE = new Rect(Vector2.zero, new Vector2(100f, 50f));
        const float NODEBLOCK_SELECTION_THICKNESS = 5f;
        static readonly float NODEBLOCK_SELECTION_THICKNESS_SUM = NODEBLOCK_SELECTION_THICKNESS * 2;

        // #endregion

        #region Variables
        string _label;
        Rect _rect;
        public string _id;
        #endregion

        #region Properties
        public bool IsSelected { private set; get; }
        #endregion



        public BlockNode(Vector2 position)
        {
            _rect = NODEBLOCK_SIZE;
            _rect.position = position;
            _label = "New Block";
            IsSelected = false;
            _id = System.Guid.NewGuid().ToString();
        }


        public void Draw()
        {
            Color prevColour;

            if (IsSelected)
            {
                //Modify rect
                Rect rectCopy = _rect;
                rectCopy.width += NODEBLOCK_SELECTION_THICKNESS_SUM;
                rectCopy.height += NODEBLOCK_SELECTION_THICKNESS_SUM;
                rectCopy.x -= NODEBLOCK_SELECTION_THICKNESS;
                rectCopy.y -= NODEBLOCK_SELECTION_THICKNESS;

                prevColour = GUIExtensions.Start_GUI_ColourChange(Color.red);
                GUI.Box(rectCopy, string.Empty);
                GUIExtensions.End_GUI_ColourChange(prevColour);
            }

            prevColour = GUIExtensions.Start_GUI_ColourChange(Color.white);
            GUI.Box(_rect, _label);
            GUIExtensions.End_GUI_ColourChange(prevColour);

        }


        public bool UpdateIsSelected(bool hasSelectedNodeBeenFound)
        {
            IsSelected = hasSelectedNodeBeenFound ? false : _rect.Contains(Event.current.mousePosition, true);
            return IsSelected;
        }

        public bool UpdateIfClicked()
        {
            //If  node hasnt been clicked
            if (!_rect.Contains(Event.current.mousePosition, true))
                return false;

            IsSelected = !IsSelected;
            return true;
        }

        public void ProcessMouseDrag(Vector2 mouseDelta)
        {
            _rect.position += mouseDelta;
        }



    }

}