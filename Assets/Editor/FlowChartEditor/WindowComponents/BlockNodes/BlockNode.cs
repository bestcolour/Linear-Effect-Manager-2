namespace LinearEffectsEditor
{
    using LinearEffects;
    using UnityEngine;
    using UnityEditor;

    //Graphical representations of block in the flowchart window editor
    public class BlockNode
    {
        #region Constants
        //========================= NODE CONSTANTS =========================================
        static readonly Rect NODEBLOCK_SIZE = new Rect(Vector2.zero, new Vector2(100f, 50f));
        const float NODEBLOCK_SELECTION_THICKNESS = 5f;
        static readonly float NODEBLOCK_SELECTION_THICKNESS_SUM = NODEBLOCK_SELECTION_THICKNESS * 2;
        static readonly Color SELECTION_COLOUR = new Color(.486f, .99f, 0, 0.5f);



        #endregion

        #region Variables
        Rect _rect;

        //=============== SERIALIZED VARIABLES ===================
        string _label;
        Color _blockColour;
        #endregion

        #region Properties
        public string ID { get; private set; }
        public bool IsSelected { set; private get; }
        #endregion


        #region Saving & Initialization

        public BlockNode(Vector2 position)
        {
            _rect = NODEBLOCK_SIZE;
            _rect.position = position;
            _label = string.Empty;
            IsSelected = false;
            ID = System.Guid.NewGuid().ToString();
        }

        public BlockNode()
        {
            _rect = NODEBLOCK_SIZE;
            _label = string.Empty;
            IsSelected = false;
            ID = System.Guid.NewGuid().ToString();
        }

        //Loads the block's editor cached variables into this node 
        public void LoadFrom(SerializedProperty blockProperty)
        {
            _label = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKNAME).stringValue;
            _blockColour = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKCOLOUR).colorValue;
            _rect.position = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKPOSITION).vector2Value;
        }

        //Loads the block's editor cached variables into this node 
        public void LoadFrom(Block block)
        {
            _label = block.BlockName;
            _blockColour = block.BlockColour;
            _rect.position = block.BlockPosition;
        }

        public void SaveTo(SerializedProperty blockProperty)
        {
            blockProperty.serializedObject.Update();
            blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKNAME).stringValue = _label;
            blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKCOLOUR).colorValue = _blockColour;
            blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKPOSITION).vector2Value = _rect.position;
            blockProperty.serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Window Functions
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

                prevColour = GUIExtensions.Start_GUI_ColourChange(SELECTION_COLOUR);
                GUI.Box(rectCopy, string.Empty);
                GUIExtensions.End_GUI_ColourChange(prevColour);
            }

            prevColour = GUIExtensions.Start_GUI_ColourChange(_blockColour);
            GUI.Box(_rect, _label);
            GUIExtensions.End_GUI_ColourChange(prevColour);

        }

        public bool CheckIfClicked()
        {
            return _rect.Contains(Event.current.mousePosition, true);
        }

        public bool CheckRectOverlap(Rect selectionBox)
        {
            return _rect.Overlaps(selectionBox, true);
        }

        public void ProcessMouseDrag(Vector2 mouseDelta)
        {
            _rect.position += mouseDelta;
        }
        #endregion
    }

}