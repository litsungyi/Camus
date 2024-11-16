using Camus.EventSystems;
using UnityEngine;

namespace Camus.Colliders
{
    public abstract class BaseColliderWrapper : MonoBehaviour
    {
        protected EventSourcing EventSource
        {
            get;
            private set;
        }

        public void Initialize(EventSourcing eventSourcing)
        {
            EventSource = eventSourcing;
        }
    }
}
