using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    public class Collision3DWrapperFull : Collision3DWrapperLight
    {
        private void OnCollisionStay(Collision other)
        {
            if (Filter != null && !Filter.Filter(other.collider))
            {
                return;
            }

            EventSource?.Raise(new Collision3DStayEvent(other));
        }
    }
}
