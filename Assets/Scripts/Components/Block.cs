﻿namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using DualList;

    //A block class will hold the order of the commands to be executed and then call
    //the respective commandexecutor to execute those commands
    [System.Serializable]
    public class Block : ArrayUser<Block.EffectOrder, BaseEffectExecutor<Effect>, Effect>
    {
        #region Definitions
        [Serializable]
        class BlockSettings
        {
#if UNITY_EDITOR
            public string BlockName;
            public Color BlockColour;
            public Vector2 BlockPosition;
#endif
        }

        [Serializable]
        public class EffectOrder : OrderData<BaseEffectExecutor<Effect>> { }
        #endregion

        #region Runtime Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        BlockSettings _settings;


        #endregion



#if UNITY_EDITOR

        #region Constants
        //All the default and propertypath name constants will be stored here in the Unity_editor section
        static readonly Color DEFAULT_BLOCK_COLOUR = new Color(0, 0.4f, 0.8f, 1f);

        //========================= BLOCK PROPERTYNAMES CONSTANTS =========================================
        public const string PROPERTYNAME_SETTINGS = "_settings";
        public const string PROPERTYPATH_BLOCKNAME = PROPERTYNAME_SETTINGS + ".BlockName";
        public const string PROPERTYPATH_BLOCKCOLOUR = PROPERTYNAME_SETTINGS + ".BlockColour";
        public const string PROPERTYPATH_BLOCKPOSITION = PROPERTYNAME_SETTINGS + ".BlockPosition";

        public const string PROPERTYNAME_ORDERARRAY = "_orderArray";
        #endregion


        #region Editor Time Cached Variables

        // public string BlockName;
        // public Color BlockColour;
        // public Vector2 BlockPosition;

        public Block(Vector2 position)
        {
            _settings = new BlockSettings();
            _settings.BlockName = "New Block";
            _settings.BlockColour = DEFAULT_BLOCK_COLOUR;
            _settings.BlockPosition = position;
        }

        public Block()
        {
            _settings = new BlockSettings();
            _settings.BlockName = "New Block";
            _settings.BlockColour = DEFAULT_BLOCK_COLOUR;
        }

        //Add all your future variables inside here for saving from a block to a serializedProperty
        public void CopyBlockPropertiesTo(SerializedProperty blockProperty)
        {
            if (blockProperty.type != typeof(Block).Name)
            {
                Debug.Log($"The serializedProperty {blockProperty} is not of Block class! Property trying to be copied to: {blockProperty.type}");
                return;
            }

            blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKCOLOUR).colorValue = _settings.BlockColour;
            blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue = _settings.BlockName;
            blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKPOSITION).vector2Value = _settings.BlockPosition;


        }

        //Add all your future variables inside here for loading from a serializedProperty to a block
        public void LoadBlockPropertiesFrom(SerializedProperty blockProperty)
        {
            if (blockProperty.type != typeof(Block).Name)
            {
                Debug.Log($"The serializedProperty {blockProperty} is not of Block class! Property trying to be copied to: {blockProperty.type}");
                return;
            }

            _settings.BlockColour = blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKCOLOUR).colorValue;
            _settings.BlockName = blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKNAME).stringValue;
            _settings.BlockPosition = blockProperty.FindPropertyRelative(Block.PROPERTYPATH_BLOCKPOSITION).vector2Value;

        }

        #endregion

#endif
    }


#if UNITY_EDITOR
    #region    //============================== SERIALIZED PROPERTY EXTENSIONS ================================
    public static class BlockSerializedPropertyExtension
    {
        //Used wehn the window editor adds a new nodeblock 
        public static SerializedProperty AddToBlockPropertyArray(this SerializedProperty blockArray, Block newBlock)
        {
            if (!blockArray.isArray)
            {
                Debug.Log($"Serialized Property {blockArray.name} is not an array!");
                return null;
            }

            blockArray.serializedObject.Update();
            blockArray.arraySize++;

            SerializedProperty lastElement = blockArray.GetArrayElementAtIndex(blockArray.arraySize - 1);
            // lastElement.CopyBlockProperties(newBlock);

            newBlock.CopyBlockPropertiesTo(lastElement);
            blockArray.serializedObject.ApplyModifiedProperties();
            return lastElement;
        }
    }
    #endregion
#endif

}