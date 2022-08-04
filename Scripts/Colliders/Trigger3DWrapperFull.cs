using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    public class Trigger3DWrapperFull : Trigger3DWrapperLight
    {
        private void OnTriggerStay(Collider other)
        {
            if (Filter != null && !Filter.Filter(other))
            {
                return;
            }

            EventSource?.Raise(new Trigger3DStayEvent(other));
        }
    }
}
