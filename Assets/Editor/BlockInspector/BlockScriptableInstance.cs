#if UNITY_EDITOR
namespace LinearEffectsEditor
{
    using System.Collections;
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
        public GameObject BlockGameObject { get; private set; }

        SerializedProperty BlockProperty => _blockNode.BlockProperty;
        public void OnCreation(GameObject go)
        {
            BlockGameObject = go;
        }

        public void Initialize(BlockNode node)
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
            BlockProperty.serializedObject.Update();
            Block.SaveToSerializedProperty(BlockProperty);
            BlockProperty.serializedObject.ApplyModifiedProperties();
            _blockNode.ReloadNodeProperties();
        }


    }

}
#endif