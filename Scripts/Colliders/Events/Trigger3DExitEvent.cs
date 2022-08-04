using UnityEngine;

namespace Camus.Colliders.Events
{
    public class Trigger3DExitEvent : BaseTrigger3DEvent
    {
        public Trigger3DExitEvent(Collider other)
            : base(other)
        {
        }
    }
}
