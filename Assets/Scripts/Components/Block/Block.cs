﻿namespace LinearEffects
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
    public partial class Block : ArrayUser<Block.EffectOrder, BaseEffectExecutor>, ISavableData
    {
        #region Definitions
        [Serializable]
        public class BlockSettings
        {
            //======================= NODE PROPERTIES (ie properties which block node uses & saves) =========================
            public string BlockName;
#if UNITY_EDITOR
            public Color BlockColour;
            [HideInInspector]
            public Vector2 BlockPosition;
#endif


        }

      
        #endregion

        #region Exposed Fields
        [Header("<== Click To Open ==>")]
        [SerializeField]
        BlockSettings _blockSettings;

        #endregion

        #region Properties
        public string BlockName
        {
            get
            {
                return _blockSettings.BlockName;
            }
            // set
            // {
            //     _blockSettings.BlockName = value;
            // }
        }

        #endregion


#if UNITY_EDITOR
        #region Editor Time 

        #region Constants
        //Being used in FCWE_NodeManager_NodeCycler.cs
        public const string DEFAULT_BLOCK_NAME = "New Block";

        //All the default and propertypath name constants will be stored here in the Unity_editor section
        static readonly Color DEFAULT_BLOCK_COLOUR = new Color(0, 0.4f, 0.8f, 1f);

        //========================= BLOCK PROPERTYNAMES CONSTANTS =========================================
        public const string PROPERTYNAME_SETTINGS = "_blockSettings";
        public const string PROPERTYPATH_BLOCKNAME = PROPERTYNAME_SETTINGS + ".BlockName";
        public const string PROPERTYPATH_BLOCKCOLOUR = PROPERTYNAME_SETTINGS + ".BlockColour";
        public const string PROPERTYPATH_BLOCKPOSITION = PROPERTYNAME_SETTINGS + ".BlockPosition";

        public const string PROPERTYNAME_ORDERARRAY = "_orderArray";
        #endregion


        #region Initialization
        //Used in FCWE_NodeManager_NodeCreation.cs
        public Block(Vector2 position, string blockName, Color blockColour)
        {
            _blockSettings = new BlockSettings();
            _blockSettings.BlockName = blockName;
            _blockSettings.BlockPosition = position;
            _blockSettings.BlockColour = blockColour;
        }

        //Used in FCWE_NodeManager_NodeCreation.cs
        public Block(Vector2 position, string blockName)
        {
            Editor_DefaultConstruction();
            _blockSettings.BlockName = blockName;
            _blockSettings.BlockPosition = position;
        }

        //Used in BlockScriptableInstance.cs
        public Block()
        {
            Editor_DefaultConstruction();
        }

        void Editor_DefaultConstruction()
        {
            _blockSettings = new BlockSettings();
            // _blockSettings.BlockName = "New Block";
            _blockSettings.BlockColour = DEFAULT_BLOCK_COLOUR;
        }

        // ///<Summary>Forcefully subscribes the entire order array elements to their respective holder's events</Summary>
        // public void Editor_AddSubscription()
        // {
        //     for (int i = 0; i < _orderArray.Length; i++)
        //     {
        //         _orderArray[i].SubscribeToEvents();
        //     }
        // }

        #endregion

        #region Sets
        ///<Summary>For forceful renaming of block names when the flowchart window manager already contains a block with similar name</Summary>
        public void Editor_SetBlockName(string blockName) { _blockSettings.BlockName = blockName; }
        #endregion

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

        #region ArrayUser Methods
        ///<Summary>Adds a new order element into the block class. We need a gameobject which this Block class is being serialized on.</Summary>
        public EffectOrder AddNewOrderElement(GameObject gameObject, Type type, string fullEffectorName, string effectName)
        {
            //Check if type is inheriting from base effect executor
            if (!type.IsSubclassOf(typeof(BaseEffectExecutor)))
            {
                Debug.Log($"Type {type} does not inherit from {typeof(BaseEffectExecutor)} and therefore adding this type to the OrderData is not possible!");
                return null;
            }

            //Get new effect order
            EffectOrder addedOrder = GetNewOrderData(gameObject, type, false, fullEffectorName, effectName);
            ArrayExtension.Add(ref _orderArray, addedOrder);
            return addedOrder;
        }

        //Since this class is not deriving from a monobehaviour, we need to pass in the reference of the gameobject this class is being serialized on
        protected Block.EffectOrder GetNewOrderData(GameObject gameObject, Type typeOfHolder, bool isForInsert, string fullEffectorName, string effectName)
        {
            if (!gameObject.TryGetComponent(typeOfHolder, out Component component))
            {
                component = gameObject.AddComponent(typeOfHolder);
            }

            BaseEffectExecutor holder = component as BaseEffectExecutor;
            Block.EffectOrder o = new Block.EffectOrder();

            o.EffectName = effectName;
            o.FullEffectName = fullEffectorName;
            o.ParentBlockName = BlockName;

            o.OnAddNew(holder, isForInsert);
            return o;
        }

        public void RemoveAllOrderData()
        {
            //Remove order data from the largest element index
            for (int i = _orderArray.Length - 1; i > -1; i--)
            {
                RemoveOrderElementAt(i);
            }
        }

        #endregion


        #endregion
#endif
    }


}