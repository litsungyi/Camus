using Camus.EventSystems;
using UnityEngine;

namespace Camus.UiUtilities
{
    public abstract class UiBaseView : MonoBehaviour
    {
        protected EventSourcing EventSourcing
        {
            get;
            private set;
        }

        internal void SetEventSourcing(EventSourcing eventSourcing)
        {
            EventSourcing = eventSourcing;

            OnUpdateEventSourcing();
        }

        protected abstract void OnUpdateEventSourcing();
     }
}
