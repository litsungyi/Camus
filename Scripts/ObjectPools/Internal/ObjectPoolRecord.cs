using UnityEngine;

namespace Camus.ObjectPools.Internal
{
    internal class ObjectPoolRecord : MonoBehaviour
    {
        private ObjectPool source;

        internal void Create(ObjectPool pool)
        {
            source = pool;
        }

        internal void Release(ObjectPool target)
        {
            if (target != source)
            {
                Debug.LogError("Release pooled instance to different pool!");
                Debug.LogWarning($"Source: {source.name}!", source);
                Debug.LogWarning($"Target: {target.name}!", target);
            }
        }
    }
}
