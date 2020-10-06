using System;
using Camus.EventSystems;
using UnityEngine;

namespace Camus.UiUtilities
{
    public abstract class UiBaseController<T> : MonoBehaviour, IDisplyable
        where T : UiBaseView
    {
        protected EventSourcing eventSourcing;

        [SerializeField]
        protected Animator animator;

        #region IDisplyable

        public virtual void Show(Action<bool> callback = null)
        {
            if (animator == null)
            {
                UiManager.Instance.ShowUi(name, callback);
                return;
            }

            UiManager.Instance.ShowUi(name, (result) =>
            {
                animator.SetBool("Show", true);
                callback?.Invoke(result);
            });
        }

        public virtual void Hide(bool destroy = false, Action<bool> callback = null)
        {
            if (destroy)
            {
                UiManager.Instance.HideUi(name, callback, true);
            }
            else
            {
                UiManager.Instance.HideUi(name, callback);
            }
        }

        public virtual void Demolish() => Destroy(gameObject);

        public virtual GameObject GetGameObject() => gameObject;

        public void OnNotify(string state)
        {
            switch(state)
            {
                case "BeginShow":
                    break;

                case "EndShow":
                    break;

                case "BeginHide":
                    break;

                case "EndHide":
                    break;
            }
        }

        #endregion

        protected abstract T View
        {
            get;
        }

        private void Awake()
        {
            eventSourcing = new EventSourcing();

            OnＡwaked();
            RegisterEvents();
            InitializeView();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
            this.OnDestroied();
        }

        protected virtual void OnＡwaked() { }
        protected virtual void OnDestroied() { }
        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }

        protected virtual void InitializeView()
        {
            View?.SetEventSourcing(eventSourcing);
        }
    }
}
