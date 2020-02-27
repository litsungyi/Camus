using UnityEngine;

namespace Camus.Colliders.Events
{
    public class TriggerEnterEvent : BaseTriggerEvent
    {
        public TriggerEnterEvent(Collider other)
            : base(other)
        {
        }
    }
}
