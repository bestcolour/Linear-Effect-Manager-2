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
        #endregion

        #region Properties
        public SerializedProperty BlockProperty { get; private set; }
        public string ID { get; private set; }
        public bool IsSelected { set; private get; }
        #endregion


        //==================== RUNTIME VARIABLES FROM BLOCK PROPERTY ============================
        string _label;
        Color _blockColour;



        #region Saving & Initialization

        public BlockNode(SerializedProperty blockProperty, Vector2 position)
        {
            BlockProperty = blockProperty;
            _rect = NODEBLOCK_SIZE;
            ID = System.Guid.NewGuid().ToString();
            IsSelected = false;

            LoadFrom();
            _rect.position = position;
        }

        public BlockNode(SerializedProperty blockProperty)
        {
            BlockProperty = blockProperty;
            _rect = NODEBLOCK_SIZE;
            ID = System.Guid.NewGuid().ToString();
            IsSelected = false;

            LoadFrom();
        }

        //Loads the block's cached variables into this node 
        void LoadFrom()
        {
            _label = BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue;
            _blockColour = BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKCOLOUR).colorValue;
            _rect.position = BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKPOSITION).vector2Value;
        }

        public void Save()
        {
            BlockProperty.serializedObject.Update();
            BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue = _label;
            BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKCOLOUR).colorValue = _blockColour;
            BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKPOSITION).vector2Value = _rect.position;
            BlockProperty.serializedObject.ApplyModifiedProperties();
        }

        public void ReloadNodeProperties()
        {
            _label = BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue;
            _blockColour = BlockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKCOLOUR).colorValue;
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