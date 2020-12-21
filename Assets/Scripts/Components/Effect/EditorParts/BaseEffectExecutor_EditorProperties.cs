#if UNITY_EDITOR
namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public abstract partial class BaseEffectExecutor
    {
        //This is stored here because EffectExecutor class is a generic class
        public const string PROPERTYNAME_EFFECTDATAS = "_effectDatas";

        public delegate void ChangeObjectArrayCallBack(int objectIndex, string effectName);

        #region Subscribing to EventList
        protected ChangeObjectArrayCallBack OnRemoveObject = null;

        public virtual void InitializeSubs(ChangeObjectArrayCallBack onRemove)
        {
            OnRemoveObject = onRemove;
        }
        #endregion

        public abstract int AddNewObject();

        public abstract void RemoveObjectAt(int index,string executorSearchBoxName );

        public abstract int DuplicateDataElement(int index);
    }

}
#endif