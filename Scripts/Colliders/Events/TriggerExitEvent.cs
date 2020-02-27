using UnityEngine;

namespace Camus.Colliders.Events
{
    public class TriggerExitEvent : BaseTriggerEvent
    {
        public TriggerExitEvent(Collider other)
            : base(other)
        {
        }
    }
}
