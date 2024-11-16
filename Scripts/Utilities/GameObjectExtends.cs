using Camus.Projects;
using UnityEngine;

namespace Camus.Utilities
{
    public static class GameObjectExtends
    {
        public static T Append<T>(this GameObject parent, T prefab)
            where T : Object
        {
            Debug.Assert(parent != null, "Parent is null!");
            Debug.Assert(prefab != null, "Prefab is null!");

            return Object.Instantiate<T>(prefab, parent.transform);
        }

        public static T AppendComponent<T>(this GameObject parent)
            where T : Component
        {
            Debug.Assert(parent != null, "Parent is null!");

            return parent.AddComponent<T>();
        }

        public static bool IsTagged(this GameObject gameObject, TagInfo tagInfo) => gameObject != null && gameObject.CompareTag(tagInfo.Name);
    }
}
