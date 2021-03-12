namespace ScriptableObjectEvents
{
    using System;
    using UnityEngine;


    ///<Summary>ScriptableObject which caches an event. Returns void when invoked, requires a string parameter</Summary>
    [CreateAssetMenu(fileName = nameof(SOEvent_RVoid_String), menuName = BaseScriptableEvent.CREATEASSETMENU_SCRIPTABLEEVENT + "/" + nameof(SOEvent_RVoid_String))]
    public class SOEvent_RVoid_String : BaseScriptableEvent
    {

        protected event Action<string> void_StringEvent = null;

        public virtual void SubscribeEvent(Action<string> action)
        {
            void_StringEvent += action;
#if UNITY_EDITOR
            RegisterMethodName(action);
#endif
        }

        public virtual void UnSubscribeEvent(Action<string> action)
        {
            void_StringEvent -= action;
#if UNITY_EDITOR
            UnRegisterMethodName(action);
#endif
        }

        public virtual void RaiseEvent(string s)
        {
            void_StringEvent?.Invoke(s);
        }



    }
}