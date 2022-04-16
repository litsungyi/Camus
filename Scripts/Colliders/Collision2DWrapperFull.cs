using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    public class Collision2DWrapperFull : Collision2DWrapperLight
    {
        private void OnCollisionStay2D(Collision2D other)
        {
            if (Filter != null && !Filter.Filter(other.collider))
            {
                return;
            }

            EventSource?.Raise(new Collision2DStayEvent(other));
        }
    }
}
