using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    public class TriggerWrapper : BaseTriggerWrapper
    {
        private void OnTriggerEnter(Collider other)
        {
            EventSource?.Raise(new TriggerEnterEvent(other));
        }

        private void OnTriggerStay(Collider other)
        {
            EventSource?.Raise(new TriggerStayEvent(other));
        }

        private void OnTriggerExit(Collider other)
        {
            EventSource?.Raise(new TriggerExitEvent(other));
        }
    }
}
