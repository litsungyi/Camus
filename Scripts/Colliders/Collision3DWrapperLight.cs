using Camus.Colliders.Events;
using Camus.Colliders.Filters;
using UnityEngine;

namespace Camus.Colliders
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Collision3DWrapperLight : BaseColliderWrapper
    {
        public IColliderFilter3D Filter
        {
            get;
            set;
        }

        private void Awake()
        {
            Validate();
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void Validate()
        {
            var baseRigidbody = gameObject.GetComponent<Rigidbody>();
            if (baseRigidbody == null)
            {
                Debug.LogError($"Rigidbody is Null!", gameObject);
            }

            var baseCollider = gameObject.GetComponent<Collider>();
            if (baseCollider == null)
            {
                Debug.LogError($"Collider is Null!", gameObject);
            }
            else if (!baseCollider.enabled)
            {
                Debug.LogError($"Collider is not Enabled!", baseCollider);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (Filter != null && !Filter.Filter(other.collider))
            {
                return;
            }

            EventSource?.Raise(new Collision3DEnterEvent(other));
        }

        private void OnCollisionExit(Collision other)
        {
            if (Filter != null && !Filter.Filter(other.collider))
            {
                return;
            }

            EventSource?.Raise(new Collision3DExitEvent(other));
        }
    }
}
