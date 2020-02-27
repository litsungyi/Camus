using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Trigger2DEnterEvent : BaseTrigger2DEvent
    {
        public Trigger2DEnterEvent(Collider2D other)
            : base(other)
        {
        }
    }
}
