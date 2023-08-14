using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Trigger2DStayEvent : BaseTrigger2DEvent
    {
        public Trigger2DStayEvent(Collider2D other)
            : base(other)
        {
        }
    }
}
