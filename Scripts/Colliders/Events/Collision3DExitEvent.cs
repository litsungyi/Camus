using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Collision3DExitEvent : BaseCollision3DEvent
    {
        public Collision3DExitEvent(Collision other)
            : base(other)
        {
        }
    }
}
