using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    public class Trigger2DWrapper : BaseTriggerWrapper
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            EventSource?.Raise(new Trigger2DEnterEvent(other));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            EventSource?.Raise(new Trigger2DStayEvent(other));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            EventSource?.Raise(new Trigger2DExitEvent(other));
        }
    }
}
