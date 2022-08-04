using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Collision2DExitEvent : BaseCollision2DEvent
    {
        public Collision2DExitEvent(Collision2D other)
            : base(other)
        {
        }
    }
}
