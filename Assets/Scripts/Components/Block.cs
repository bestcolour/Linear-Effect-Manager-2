namespace LinearEffects
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
            [SerializeField]
            bool _randomBool = default;
        }

        [Serializable]
        public class EffectOrder : OrderData<BaseEffectExecutor<Effect>> { }
        #endregion

        #region Runtime Cached Variables
        [Header("Some Settings")]
        [SerializeField]
        BlockSettings _settings = default;


        #endregion



#if UNITY_EDITOR

        #region Constants
        //All the default and propertypath name constants will be stored here in the Unity_editor section
        static readonly Color DEFAULT_BLOCK_COLOUR = new Color(0, 0.4f, 0.8f, 1f);

        //========================= BLOCK PROPERTYNAMES CONSTANTS =========================================
        public const string PROPERTYNAME_BLOCKNAME = "BlockName";
        public const string PROPERTYNAME_BLOCKCOLOUR = "BlockColour";
        public const string PROPERTYNAME_BLOCKPOSITION = "BlockPosition";
        #endregion


        #region Editor Time Cached Variables

        [field: SerializeField]
        public string BlockName;
        [field: SerializeField]
        public Color BlockColour;
        [field: SerializeField]
        public Vector2 BlockPosition;

        public Block(Vector2 position)
        {
            BlockName = "New Block";
            BlockColour = DEFAULT_BLOCK_COLOUR;
            BlockPosition = position;
        }

        public Block()
        {
            BlockName = "New Block";
            BlockColour = DEFAULT_BLOCK_COLOUR;
        }
        #endregion



#endif





    }


#if UNITY_EDITOR

    //============================== SERIALIZED PROPERTY EXTENSIONS ================================
    public static class BlockPropertyExtension
    {
        public static void AddToBlockPropertyArray(this SerializedProperty blockArray, Block newBlockElement)
        {
            if (!blockArray.isArray)
            {
                Debug.Log($"Serialized Property {blockArray.name} is not an array!");
                return;
            }

            blockArray.serializedObject.Update();
            blockArray.arraySize++;

            SerializedProperty lastElement = blockArray.GetArrayElementAtIndex(blockArray.arraySize - 1);
            lastElement.CopyBlockProperties(newBlockElement);
            blockArray.serializedObject.ApplyModifiedProperties();
        }

        public static void CopyBlockProperties(this SerializedProperty blockProperty, Block blockToCopyFrom)
        {
            if (blockProperty.type != typeof(Block).Name)
            {
                Debug.Log($"The serializedProperty {blockProperty} is not of Block class! Property trying to be copied to: {blockProperty.type}");
                return;
            }

            blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKCOLOUR).colorValue = blockToCopyFrom.BlockColour;
            blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKNAME).stringValue = blockToCopyFrom.BlockName;
            blockProperty.FindPropertyRelative(Block.PROPERTYNAME_BLOCKPOSITION).vector2Value = blockToCopyFrom.BlockPosition;


        }



    }
#endif

}