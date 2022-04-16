using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Collision3DStayEvent : BaseCollision3DEvent
    {
        public Collision3DStayEvent(Collision other)
            : base(other)
        {
        }
    }
}
