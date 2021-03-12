namespace ScriptableObjectEvents
{
    using System;
    using UnityEngine;


    ///<Summary>ScriptableObject which caches an event. Returns void when invoked, requires a bool parameter</Summary>
    [CreateAssetMenu(fileName = nameof(SOEvent_RVoid_Bool), menuName = BaseScriptableEvent.CREATEASSETMENU_SCRIPTABLEEVENT + "/" + nameof(SOEvent_RVoid_Bool))]
    public class SOEvent_RVoid_Bool : BaseScriptableEvent
    {

        protected event Action<bool> void_BoolEvent = null;

        public virtual void SubscribeEvent(Action<bool> action)
        {
            void_BoolEvent += action;
#if UNITY_EDITOR
            RegisterMethodName(action);
#endif
        }

        public virtual void UnSubscribeEvent(Action<bool> action)
        {
            void_BoolEvent -= action;
#if UNITY_EDITOR
            UnRegisterMethodName(action);
#endif
        }

        public virtual void RaiseEvent(bool b)
        {
            void_BoolEvent?.Invoke(b);
        }



    }
}