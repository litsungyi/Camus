using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Collision2DEnterEvent : BaseCollision2DEvent
    {
        public Collision2DEnterEvent(Collision2D other)
            : base(other)
        {
        }
    }
}
