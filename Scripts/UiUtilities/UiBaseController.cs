using System;
using Camus.EventSystems;
using UnityEngine;

namespace Camus.UiUtilities
{
    public abstract class UiBaseController<T> : MonoBehaviour, IDisplyable
        where T : UiBaseView
    {
        protected UiEventSourcing eventSourcing;

        #region IDisplyable

        public virtual void Show(Action<bool> callback)
        {
            UiManager.Instance.ShowUi(name, callback);
            gameObject.SetActive(true);
        }

        public virtual void Hide(bool destroy, Action<bool> callback)
        {
            if (destroy)
            {
                UiManager.Instance.HideUi(name, callback, true);
            }
            else
            {
                UiManager.Instance.HideUi(name, callback);
                gameObject.SetActive(false);
            }
        }

        public virtual void Demolish()
        {
            Destroy(gameObject);
        }

        #endregion

        protected abstract T View
        {
            get;
        }

        private void Awake()
        {
            eventSourcing = new UiEventSourcing();

            OnAwake();
            RegisterEvents();
            InitializeView();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
            this.OnDestroied();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnDestroied() { }
        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }

        protected virtual void InitializeView()
        {
            View?.SetEventSourcing(eventSourcing);
        }
    }
}
