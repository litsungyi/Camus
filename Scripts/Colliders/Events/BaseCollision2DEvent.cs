using UnityEngine;
using Camus.EventSystems;

namespace Camus.Colliders.Events
{
    public abstract class BaseCollision2DEvent : IEvent
    {
        public Collision2D Other
        {
            get;
        }

        protected BaseCollision2DEvent(Collision2D other)
        {
            Other = other;
        }
    }
}
