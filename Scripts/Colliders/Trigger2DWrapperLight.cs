using Camus.Colliders.Events;
using UnityEngine;

namespace Camus.Colliders
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Trigger2DWrapperLight : BaseColliderWrapper
    {
        private void Awake()
        {
            Validate();
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void Validate()
        {
            var baseRigidbody = gameObject.GetComponent<Rigidbody2D>();
            if (baseRigidbody == null)
            {
                Debug.LogError($"Rigidbody is Null!", gameObject);
            }

            var baseCollider = gameObject.GetComponent<Collider2D>();
            if (baseCollider == null)
            {
                Debug.LogError($"Collider is Null!", gameObject);
            }
            else if (!baseCollider.enabled)
            {
                Debug.LogError($"Collider is not Enabled!", baseCollider);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            EventSource?.Raise(new Trigger2DEnterEvent(other));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            EventSource?.Raise(new Trigger2DExitEvent(other));
        }
    }
}
