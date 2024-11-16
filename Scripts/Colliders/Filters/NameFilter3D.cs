using System;
using UnityEngine;

namespace Camus.Colliders.Filters
{
    [Serializable]
    public class NameFilter3D : IColliderFilter3D
    {
        [SerializeField]
        private string name = string.Empty;

        public string Name
        {
            set => name = value;
        }

        bool IColliderFilter3D.Filter(Collider collider)
        {
            return collider.name == name;
        }
    }
}
