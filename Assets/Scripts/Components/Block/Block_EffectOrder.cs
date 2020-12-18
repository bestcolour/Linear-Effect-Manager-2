namespace LinearEffects
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public partial class Block
    {
        [Serializable]
        public class EffectOrder
#if UNITY_EDITOR
        : ISavableData
#endif
        // public class EffectOrder : OrderData<BaseEffectExecutor>, ISavableData
        {
            [SerializeField]
            protected BaseEffectExecutor _refHolder;
            [SerializeField]
            protected int _dataElmtIndex;

#if UNITY_EDITOR

            //All these functions are used during unity editor time to manage the Holder's array as well as the OrderData itself
            //none of these will be used in the actual build except for the variables stored
            public virtual void OnAddNew(BaseEffectExecutor holder, bool isInsert)
            {
                _refHolder = holder;
                _dataElmtIndex = _refHolder.AddNewObject(isInsert);
            }

            //To be called before removing the order intsance from the list
            public virtual void OnRemove()
            {
                _refHolder.RemoveObjectAt(_dataElmtIndex);
            }

            //For when the holder is not null
            public virtual void OnInsertCopy()
            {
                //Tell the holder to do a copy of my current data index details and add it to the end of the array
                _dataElmtIndex = _refHolder.DuplicateDataElement(_dataElmtIndex);
            }

            public virtual void OnInsertCopy(BaseEffectExecutor holder)
            {
                _refHolder = holder;
                OnInsertCopy();
            }

            ///<Summary>Does a manual removal check in the case where the FlowChart editor's OnRemoval event does not include the block in which this EffectOrder is being serialized on</Summary>
            public virtual void ManualRemovalCheck(int removedIndex)
            {
                if (removedIndex < _dataElmtIndex)
                {
                    _dataElmtIndex--;
                }
            }

            ///<Summary>Does a manual insert check in the case where the FlowChart editor's OnInsert event does not include the block in which this EffectOrder is being serialized on</Summary>
            public virtual void ManualInsertCheck(int insertedIndex)
            {
                if (insertedIndex < _dataElmtIndex)
                {
                    _dataElmtIndex++;
                }
            }

            #region ISavable Methods
            #region Constants
            public const string PROPERTYNAME_EXECUTORNAME = "ExecutorName"
            , PROPERTYNAME_FULLEXECUTORNAME = "FullExecutorName"
            , PROPERTYNAME_REFHOLDER = "_refHolder"
            , PROPERTYNAME_DATAELEMENTINDEX = "_dataElmtIndex"
            ;
            // public const string PROPERTYNAME_ERRORLOG = "ErrorLog";
            #endregion

            public string ExecutorName;
            public string FullExecutorName;
            // public string ErrorLog = "Error";

            public void SaveToSerializedProperty(SerializedProperty property)
            {
                property.FindPropertyRelative(PROPERTYNAME_REFHOLDER).objectReferenceValue = _refHolder;
                property.FindPropertyRelative(PROPERTYNAME_DATAELEMENTINDEX).intValue = _dataElmtIndex;
                // property.FindPropertyRelative(PROPERTYNAME_ERRORLOG).stringValue = ErrorLog;
                property.FindPropertyRelative(PROPERTYNAME_EXECUTORNAME).stringValue = ExecutorName;
                property.FindPropertyRelative(PROPERTYNAME_FULLEXECUTORNAME).stringValue = FullExecutorName;
            }

            public void LoadFromSerializedProperty(SerializedProperty property)
            {
                _refHolder = (BaseEffectExecutor)property.FindPropertyRelative(PROPERTYNAME_REFHOLDER).objectReferenceValue;
                _dataElmtIndex = property.FindPropertyRelative(PROPERTYNAME_DATAELEMENTINDEX).intValue;
                // ErrorLog = property.FindPropertyRelative(PROPERTYNAME_ERRORLOG).stringValue;
                ExecutorName = property.FindPropertyRelative(PROPERTYNAME_EXECUTORNAME).stringValue;
                FullExecutorName = property.FindPropertyRelative(PROPERTYNAME_FULLEXECUTORNAME).stringValue;
            }
            #endregion

#endif

        }
    }

}