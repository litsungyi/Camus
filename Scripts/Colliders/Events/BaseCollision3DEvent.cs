using UnityEngine;
using Camus.EventSystems;

namespace Camus.Colliders.Events
{
    public abstract class BaseCollision3DEvent : IEvent
    {
        public Collision Other
        {
            get;
        }

        protected BaseCollision3DEvent(Collision other)
        {
            Other = other;
        }
    }
}
