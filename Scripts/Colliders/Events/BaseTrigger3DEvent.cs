using UnityEngine;
using Camus.EventSystems;

namespace Camus.Colliders.Events
{
    public abstract class BaseTrigger3DEvent : IEvent
    {
        public Collider Other
        {
            get;
        }

        protected BaseTrigger3DEvent(Collider other)
        {
            Other = other;
        }
    }
}
