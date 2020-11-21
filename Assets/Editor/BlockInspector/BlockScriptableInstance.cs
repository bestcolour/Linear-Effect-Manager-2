#if UNITY_EDITOR
namespace LinearEffectsEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;

    //This class will be instantiated whenever you start up the flowchart editor window and destroyed whenver unity recompiles.
    //The job of this class is to link the BlockNode and FlowChart's Block custom class together using a custom scriptable object inspector
    public class BlockScriptableInstance : ScriptableObject
    {

        #region Constants
        const string PROPERTYNAME_BLOCKPROPERTY = "<Block>k__BackingField";
        public const string PROPERTYPATH_SETTINGS = PROPERTYNAME_BLOCKPROPERTY + "." + Block.PROPERTYNAME_SETTINGS;
        public const string PROPERTYPATH_ORDERARRAY = PROPERTYNAME_BLOCKPROPERTY + "." + Block.PROPERTYNAME_ORDERARRAY;
        public const string PROPERTYPATH_ORDER = PROPERTYNAME_BLOCKPROPERTY + "." + Block.PROPERTYNAME_ORDERARRAY;

        #endregion

        [field: SerializeField]
        public Block Block { get; private set; }

        BlockNode _blockNode;

        #region Properties
        public GameObject BlockGameObject { get; private set; }

        SerializedProperty BlockProperty => _blockNode.BlockProperty;
        #endregion

        #region Events
        public event Action<string, string> OnSaveModifiedProperties = null;
        #endregion

        public void OnCreation(GameObject go)
        {
            BlockGameObject = go;
        }

        public void ReadBlockNode(BlockNode node)
        {
            _blockNode = node;
            Block = new Block();
            Block.LoadFromSerializedProperty(BlockProperty);
        }

        public void SaveModifiedProperties()
        {
            //That means maybe a recompliation occured
            if (_blockNode == null)
            {
                Debug.LogWarning("Reselect the block!");
                return;
            }

//Saving name before updating the blocknode's values
            string prevName = _blockNode.Label;

            BlockProperty.serializedObject.Update();
            Block.SaveToSerializedProperty(BlockProperty);
            BlockProperty.serializedObject.ApplyModifiedProperties();
            _blockNode.ReloadNodeProperties();

            //Call event
            OnSaveModifiedProperties?.Invoke(prevName, _blockNode.Label);
        }


    }

}
#endif