using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Trigger2DExitEvent : BaseTrigger2DEvent
    {
        public Trigger2DExitEvent(Collider2D other)
            : base(other)
        {
        }
    }
}
