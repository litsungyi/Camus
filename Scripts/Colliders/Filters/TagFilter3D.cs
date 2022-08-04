using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camus.Colliders.Filters
{
    [Serializable]
    public class TagFilter3D : IColliderFilter3D
    {
        [SerializeField]
        private List<string> tags = new List<string>();

        public void AddTag(string tag)
        {
            tags.Add(tag);
        }

        bool IColliderFilter3D.Filter(Collider collider)
        {
            return tags.Any(collider.CompareTag);
        }
    }
}
