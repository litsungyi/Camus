using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    public class Collision2DWrapperFull : Collision2DWrapperLight
    {
        private void OnCollisionStay2D(Collision2D other)
        {
            EventSource?.Raise(new Collision2DStayEvent(other));
        }
    }
}
