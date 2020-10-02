using UnityEngine;

namespace Camus.Colliders.Filters
{
    public interface IColliderFilter2D
    {
        bool Filter(Collider2D collider);
    }
}
