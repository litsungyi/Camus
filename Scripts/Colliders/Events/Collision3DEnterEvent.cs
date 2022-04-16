using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Collision3DEnterEvent : BaseCollision3DEvent
    {
        public Collision3DEnterEvent(Collision other)
            : base(other)
        {
        }
    }
}
