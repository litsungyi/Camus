using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Collision2DStayEvent : BaseCollision2DEvent
    {
        public Collision2DStayEvent(Collision2D other)
            : base(other)
        {
        }
    }
}
