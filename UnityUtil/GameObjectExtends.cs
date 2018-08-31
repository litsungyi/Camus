using UnityEngine;

namespace Camus
{
    public static class GameObjectExtends
    {
        public static T Append<T>(this GameObject parent, T prefab)
            where T : UnityEngine.Object
        {
            Debug.Assert(parent != null, "Parent is null!");
            Debug.Assert(prefab != null, "Prefab is null!");

            var instance = GameObject.Instantiate<T>(prefab) as GameObject;
            instance.transform.parent = parent.transform;
            return instance as T;
        }

        public static T AppendComponent<T>(this GameObject parent)
            where T : Component
        {
            Debug.Assert(parent != null, "Parent is null!");

            var instance = parent.AddComponent<T>();
            return instance;
        }
    }
}
