using UnityEngine;
using Camus.EventSystems;

namespace Camus.Colliders.Events
{
    public abstract class BaseTrigger2DEvent : IEvent
    {
        public Collider2D Other
        {
            get;
        }

        protected BaseTrigger2DEvent(Collider2D other)
        {
            Other = other;
        }
    }
}
