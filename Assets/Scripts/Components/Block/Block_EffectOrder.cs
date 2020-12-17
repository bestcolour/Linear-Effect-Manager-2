namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using DualList;

    public partial class Block
    {
        [Serializable]
        public class EffectOrder : OrderData<BaseEffectExecutor>, ISavableData
        {
#if UNITY_EDITOR
            #region Constants
            public const string PROPERTYNAME_EFFECTNAME = "EffectName"
            , PROPERTYNAME_FULLEFFECTNAME = "FullEffectName"
            , PROPERTYNAME_PARENTBLOCKNAME = "ParentBlockName"
            , PROPERTYNAME_REFHOLDER = "_refHolder"
            , PROPERTYNAME_DATAELEMENTINDEX = "_dataElmtIndex"
            ;
            // public const string PROPERTYNAME_ERRORLOG = "ErrorLog";
            #endregion

            public string EffectName;
            public string FullEffectName;
            public string ParentBlockName;
            // public string ErrorLog = "Error";

            public void SaveToSerializedProperty(SerializedProperty property)
            {
                property.FindPropertyRelative(PROPERTYNAME_REFHOLDER).objectReferenceValue = _refHolder;
                property.FindPropertyRelative(PROPERTYNAME_DATAELEMENTINDEX).intValue = _dataElmtIndex;
                // property.FindPropertyRelative(PROPERTYNAME_ERRORLOG).stringValue = ErrorLog;
                property.FindPropertyRelative(PROPERTYNAME_PARENTBLOCKNAME).stringValue = ParentBlockName;
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
                ParentBlockName = property.FindPropertyRelative(PROPERTYNAME_PARENTBLOCKNAME).stringValue;

                // //Subscribe to the ref holder when a block is being loaded (which hence loads its order data)
                // _refHolder.SubToOnRemove(HandleRemoveObject);
                // _refHolder.SubToOnInsert(HandleInsertObject);

                // _refHolder.OnRemoveObject += HandleRemoveObject;
                // _refHolder.OnInsertNewObject += HandleInsertObject;
            }

            // public override void SubscribeToEvents()
            // {
            //     Debug.Log($"Parent block name: {ParentBlockName}");
            //     base.SubscribeToEvents();
            // }

#endif
        }
    }

}