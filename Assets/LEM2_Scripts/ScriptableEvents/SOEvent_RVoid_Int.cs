namespace ScriptableObjectEvents
{
    using System;
    using UnityEngine;


    ///<Summary>ScriptableObject which caches an event. Returns void when invoked, requires a int parameter</Summary>
    [CreateAssetMenu(fileName = nameof(SOEvent_RVoid_Int), menuName = BaseScriptableEvent.CREATEASSETMENU_SCRIPTABLEEVENT + "/" + nameof(SOEvent_RVoid_Int))]
    public class SOEvent_RVoid_Int : BaseScriptableEvent
    {

        protected event Action<int> void_FloatEvent = null;

        public virtual void SubscribeEvent(Action<int> action)
        {
            void_FloatEvent += action;
#if UNITY_EDITOR
            RegisterMethodName(action);
#endif
        }

        public virtual void UnSubscribeEvent(Action<int> action)
        {
            void_FloatEvent -= action;
#if UNITY_EDITOR
            UnRegisterMethodName(action);
#endif
        }

        public virtual void RaiseEvent(int i)
        {
            void_FloatEvent?.Invoke(i);
        }



    }
}