using UnityEngine;

namespace Camus.Colliders.Filters
{
    public interface IColliderFilter3D
    {
        bool Filter(Collider collider);
    }
}
