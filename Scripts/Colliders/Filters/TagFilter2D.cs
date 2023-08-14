using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camus.Colliders.Filters
{
    [Serializable]
    public class TagFilter2D : IColliderFilter2D
    {
        [SerializeField]
        private List<string> tags = new List<string>();

        public void AddTag(string tag)
        {
            tags.Add(tag);
        }

        bool IColliderFilter2D.Filter(Collider2D collider)
        {
            return tags.Any(collider.CompareTag);
        }
    }
}
