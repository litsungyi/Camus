using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Trigger3DEnterEvent : BaseTrigger3DEvent
    {
        public Trigger3DEnterEvent(Collider other)
            : base(other)
        {
        }
    }
}
