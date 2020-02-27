using UnityEngine;

namespace Camus.Colliders.Events
{
    public class TriggerStayEvent : BaseTriggerEvent
    {
        public TriggerStayEvent(Collider other)
            : base(other)
        {
        }
    }
}
