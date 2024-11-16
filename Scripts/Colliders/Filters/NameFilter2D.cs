using System;
using UnityEngine;

namespace Camus.Colliders.Filters
{
    [Serializable]
    public class NameFilter2D : IColliderFilter2D
    {
        [SerializeField]
        private string name = string.Empty;

        public string Name
        {
            set => name = value;
        }

        bool IColliderFilter2D.Filter(Collider2D collider)
        {
            return collider.name == name;
        }
    }
}
