using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    public class Trigger2DWrapperFull : Trigger2DWrapperLight
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            EventSource?.Raise(new Trigger2DStayEvent(other));
        }
    }
}
