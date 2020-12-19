namespace LinearEffectsEditor
{
    using UnityEditor;
    using LinearEffects;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public partial class FlowChartWindowEditor : EditorWindow
    {
        public delegate void CompareDataElementIndexCallback(SerializedProperty dataElementProperty, int dataElementIndex);

        ///<Summary>Ensures that there will always be the executor typed component on the gameobject and that it always has the OnRemoval event assigned on the baseEffectExecutor </Summary>
        public static BaseEffectExecutor StaticMethods_EnsureExecutorComponent(GameObject gameObject, Type holderType)
        {
            if (!holderType.IsSubclassOf(typeof(BaseEffectExecutor)))
            {
                Debug.Log($"Type {holderType} does not inherit from {typeof(BaseEffectExecutor)} and therefore adding this type to the OrderData is not possible!");
                return null;
            }

            BaseEffectExecutor holder;

            if (!gameObject.TryGetComponent(holderType, out Component component))
            {
                //If no component found, insert the orderdata into the order array
                component = gameObject.AddComponent(holderType);
                holder = component as BaseEffectExecutor;
                holder.InitializeSubs(StaticMethods_HandleOnRemoveEvent);
                return holder;
            }

            holder = component as BaseEffectExecutor;
            return holder;
        }

        static void StaticMethods_HandleOnRemoveEvent(int removedIndex, string effectorName)
        {
            //Compare the index and if the removed index is smaller than the data element index being looped checked thru, decrement it
            instance.NodeManager_SaveManager_CompareDataElementIndex
            (
                (SerializedProperty dataElementProperty, int dataElementIndex) =>
                {
                    if (removedIndex < dataElementIndex)
                    {
                        //set the data element index to something decremented
                        dataElementIndex--;
                        dataElementProperty.serializedObject.Update();
                        dataElementProperty.intValue = dataElementIndex;
                        dataElementProperty.serializedObject.ApplyModifiedProperties();
                    }
                }
                ,
                effectorName
            )
            ;
        }

        //This method does a check between element index but only via serialized property
        void NodeManager_SaveManager_CompareDataElementIndex(CompareDataElementIndexCallback compareDataElementIndex, string effectorName)
        {
            //Search through every block
            for (int blockIndex = 0; blockIndex < _allBlocksArrayProperty.arraySize; blockIndex++)
            {
                SerializedProperty blockProperty = _allBlocksArrayProperty.GetArrayElementAtIndex(blockIndex);
                SerializedProperty orderArray = blockProperty.FindPropertyRelative(Block.PROPERTYNAME_ORDERARRAY);

                for (int orderIndex = 0; orderIndex < orderArray.arraySize; orderIndex++)
                {
                    //Check if block's effect order name is the same as the fullEffectname
                    SerializedProperty orderElement = orderArray.GetArrayElementAtIndex(orderIndex);
                    string orderElementEffectName = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_EXECUTORNAME).stringValue;

                    if (orderElementEffectName != effectorName)
                    {
                        continue;
                    }

                    // Debug.Log($"EffectName {effectorName} FullName: {orderElementEffectName}");


                    //Check if the removed index is smaller than this order element's index
                    SerializedProperty dataElementProperty = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_DATAELEMENTINDEX);
                    int dataElmtIndex = dataElementProperty.intValue;
                    compareDataElementIndex?.Invoke(dataElementProperty, dataElmtIndex);
                }


            }

        }

    }


}