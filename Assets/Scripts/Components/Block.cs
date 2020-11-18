namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using DualList;

    //A block class will hold the order of the commands to be executed and then call
    //the respective commandexecutor to execute those commands
    [Serializable]
    public class Block : ArrayUser<Block.EffectOrder, BaseEffectExecutor>, ISavableData
    {
        #region Definitions
        [Serializable]
        class BlockSettings
        {
#if UNITY_EDITOR
            //======================= NODE PROPERTIES (ie properties which block node uses & saves) =========================
            public string BlockName;
            public Color BlockColour;
            [HideInInspector]
            public Vector2 BlockPosition;
#endif


        }

        [Serializable]
        public class EffectOrder : OrderData<BaseEffectExecutor>, ISavableData
        {
#if UNITY_EDITOR
            #region Constants
            public const string PROPERTYNAME_EFFECTNAME = "EffectName"
            , PROPERTYNAME_FULLEFFECTNAME = "FullEffectName"
            , PROPERTYNAME_REFHOLDER = "_refHolder"
            , PROPERTYNAME_DATAELEMENTINDEX = "_dataElmtIndex"
            ;
            // public const string PROPERTYNAME_ERRORLOG = "ErrorLog";
            #endregion

            public string EffectName;
            public string FullEffectName;
            // public string ErrorLog = "Error";

            public void SaveToSerializedProperty(SerializedProperty property)
            {
                property.FindPropertyRelative(PROPERTYNAME_REFHOLDER).objectReferenceValue = _refHolder;
                property.FindPropertyRelative(PROPERTYNAME_DATAELEMENTINDEX).intValue = _dataElmtIndex;
                // property.FindPropertyRelative(PROPERTYNAME_ERRORLOG).stringValue = ErrorLog;
                property.FindPropertyRelative(PROPERTYNAME_EFFECTNAME).stringValue = EffectName;
                property.FindPropertyRelative(PROPERTYNAME_FULLEFFECTNAME).stringValue = FullEffectName;
            }

            public void LoadFromSerializedProperty(SerializedProperty property)
            {
                _refHolder = (BaseEffectExecutor)property.FindPropertyRelative(PROPERTYNAME_REFHOLDER).objectReferenceValue;
                _dataElmtIndex = property.FindPropertyRelative(PROPERTYNAME_DATAELEMENTINDEX).intValue;
                // ErrorLog = property.FindPropertyRelative(PROPERTYNAME_ERRORLOG).stringValue;
                EffectName = property.FindPropertyRelative(PROPERTYNAME_EFFECTNAME).stringValue;
                FullEffectName = property.FindPropertyRelative(PROPERTYNAME_FULLEFFECTNAME).stringValue;


                _refHolder.OnRemoveObject += HandleRemoveObject;
                _refHolder.OnInsertNewObject += HandleInsertObject;
            }

            public void CopyValuesFrom(EffectOrder e)
            {
                _refHolder = e._refHolder;
                _dataElmtIndex = e._dataElmtIndex;
                EffectName = e.EffectName;
                FullEffectName = e.FullEffectName;

            }

#endif
        }
        #endregion

        #region Runtime Cached Variables
        [Header("<== Click To Open ==>")]
        [SerializeField]
        BlockSettings _blockSettings;


        #endregion


#if UNITY_EDITOR
        #region Editor Time 

        #region Constants
        //All the default and propertypath name constants will be stored here in the Unity_editor section
        static readonly Color DEFAULT_BLOCK_COLOUR = new Color(0, 0.4f, 0.8f, 1f);

        //========================= BLOCK PROPERTYNAMES CONSTANTS =========================================
        public const string PROPERTYNAME_SETTINGS = "_blockSettings";
        public const string PROPERTYPATH_BLOCKNAME = PROPERTYNAME_SETTINGS + ".BlockName";
        public const string PROPERTYPATH_BLOCKCOLOUR = PROPERTYNAME_SETTINGS + ".BlockColour";
        public const string PROPERTYPATH_BLOCKPOSITION = PROPERTYNAME_SETTINGS + ".BlockPosition";

        public const string PROPERTYNAME_ORDERARRAY = "_orderArray";
        #endregion


        public Block(Vector2 position)
        {
            _blockSettings = new BlockSettings();
            _blockSettings.BlockName = "New Block";
            _blockSettings.BlockColour = DEFAULT_BLOCK_COLOUR;
            _blockSettings.BlockPosition = position;
        }

        public Block()
        {
            _blockSettings = new BlockSettings();
            _blockSettings.BlockName = "New Block";
            _blockSettings.BlockColour = DEFAULT_BLOCK_COLOUR;
        }

        #region Handle Interface Methods

        //Add all your future variables inside here for saving from a block to a serializedProperty
        public void SaveToSerializedProperty(SerializedProperty blockProperty)
        {
            if (blockProperty.type != typeof(Block).Name)
            {
                Debug.Log($"The serializedProperty {blockProperty} is not of Block class! Property trying to be copied to: {blockProperty.type}");
                return;
            }

            //================ BLOCK SETTINGS ========================
            blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKCOLOUR).colorValue = _blockSettings.BlockColour;
            blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue = _blockSettings.BlockName;
            blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKPOSITION).vector2Value = _blockSettings.BlockPosition;


            //============= SAVING ORDER ARRAY =====================
            SerializedProperty orderArrayProperty = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_ORDERARRAY);
            orderArrayProperty.ClearArray();
            //Apply clearing the array first
            orderArrayProperty.serializedObject.ApplyModifiedProperties();
            for (int i = 0; i < _orderArray.Length; i++)
            {
                orderArrayProperty.AddToSerializedPropertyArray(_orderArray[i]);
            }

        }

        //Add all your future variables inside here for loading from a serializedProperty to a block
        public void LoadFromSerializedProperty(SerializedProperty blockProperty)
        {
            if (blockProperty.type != typeof(Block).Name)
            {
                Debug.Log($"The serializedProperty {blockProperty} is not of Block class! Property trying to be copied to: {blockProperty.type}");
                return;
            }

            _blockSettings.BlockColour = blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKCOLOUR).colorValue;
            _blockSettings.BlockName = blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue;
            _blockSettings.BlockPosition = blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKPOSITION).vector2Value;

            //============= LOADING ORDER ARRAY =====================
            SerializedProperty orderArrayProperty = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_ORDERARRAY);
            _orderArray = new EffectOrder[orderArrayProperty.arraySize];
            for (int i = 0; i < _orderArray.Length; i++)
            {
                SerializedProperty currentElement = orderArrayProperty.GetArrayElementAtIndex(i);
                _orderArray[i] = new EffectOrder();
                _orderArray[i].LoadFromSerializedProperty(currentElement);
            }
        }


        #endregion

        #region Override Methods
        public void AddNewOrderElement(GameObject gameObject, Type type, string fullEffectName, string effectName)
        {
            if (!type.IsSubclassOf(typeof(BaseEffectExecutor)))
            {
                Debug.Log($"Type {type} does not inherit from {typeof(BaseEffectExecutor)} and therefore adding this type to the OrderData is not possible!");
                return;
            }

            ArrayExtension.Add(ref _orderArray, GetNewOrderData(gameObject, type, false, fullEffectName, effectName));
        }

        //Since this class is not deriving from a monobehaviour, we need to pass in the reference of the gameobject this class is being serialized on
        protected Block.EffectOrder GetNewOrderData(GameObject gameObject, Type typeOfHolder, bool isForInsert, string fullEffectName, string effectName)
        {
            if (!gameObject.TryGetComponent(typeOfHolder, out Component component))
            {
                component = gameObject.AddComponent(typeOfHolder);
            }

            BaseEffectExecutor holder = component as BaseEffectExecutor;
            Block.EffectOrder o = new Block.EffectOrder();

            o.EffectName = effectName;
            o.FullEffectName = fullEffectName;

            o.OnAddNew(holder, isForInsert);
            return o;
        }

        #endregion


        #endregion
#endif
    }


}