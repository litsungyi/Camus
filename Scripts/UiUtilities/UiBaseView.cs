using Camus.EventSystems;
using UnityEngine;

namespace Camus.UiUtilities
{
    public abstract class UiBaseView : MonoBehaviour
    {
        protected UiEventSourcing eventSourcing;

        internal void SetEventSourcing(UiEventSourcing uiEventSourcing)
        {
            this.eventSourcing = uiEventSourcing;

            OnUpdateEventSourcing();
        }

        protected abstract void OnUpdateEventSourcing();
     }
}
