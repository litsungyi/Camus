using UnityEngine;
using Camus.EventSystems;

namespace Camus.Colliders.Events
{
    public abstract class BaseTriggerEvent : IEvent
    {
        public Collider Other
        {
            get;
        }

        protected BaseTriggerEvent(Collider other)
        {
            Other = other;
        }
    }
}
