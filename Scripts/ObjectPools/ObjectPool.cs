using System.Collections.Generic;
using System.Linq;
using Camus.ObjectPools.Internal;
using Camus.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Camus.ObjectPools
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private uint initialSize;

        [SerializeField]
        private uint growSize;

        [SerializeField]
        private uint maxGrowTime;

        [ReadOnly, SerializeField]
        private uint growTime;

        [ReadOnly, SerializeField]
        private uint currentSize;

        private bool CanGrow => growTime < maxGrowTime;

        private readonly Stack<GameObject> reusables = new Stack<GameObject>();

        #region Debug

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void AddDebugInfo(GameObject instance)
        {
            var record = instance.AddComponent<ObjectPoolRecord>();
            record.Create(this);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void CheckDebugInfo(GameObject instance)
        {
            var record = instance.GetComponent<ObjectPoolRecord>();
            if (record == null)
            {
                Debug.LogError("Pooled instance error!");
            }
            else
            {
                record.Release(this);
            }
        }

        #endregion

        private void Awake()
        {
            Assert.IsNotNull(prefab);
            Assert.IsTrue(growSize >= 1, "[ObjectPool] GrowSize less than 1!");

            if (growSize < 1)
            {
                Debug.LogError("[ObjectPool] GrowSize less than 1!", gameObject);
                growSize = 1;
            }

            currentSize = Growth(initialSize);
        }

        private uint Growth(uint size)
        {
            for (int i = 0; i < size; ++i)
            {
                var instance = Instantiate(prefab, transform);
                AddDebugInfo(instance);
                instance.SetActive(false);
                reusables.Push(instance);
            }

            return size;
        }

        private GameObject GetInstance()
        {
            if (!reusables.Any())
            {
                if (!CanGrow)
                {
                    return null;
                }

                ++growTime;
                currentSize += Growth(growSize);
            }

            return reusables.Pop();
        }

        public GameObject Acquire(Transform parent = null)
        {
            var instance = GetInstance();
            instance?.SetActive(true);
            if (parent != null)
            {
                instance?.transform.SetParent(parent);
            }

            return instance;
        }

        public T Acquire<T>(Transform parent = null)
            where T : MonoBehaviour
        {
            var instance = Acquire(parent);
            var target = instance?.GetComponent(typeof(T));
            return target as T;
        }

        public bool Release(GameObject instance)
        {
            Assert.IsNotNull(instance, "[ObjectPool] Release instance cannot be null!");

            if (instance == null)
            {
                Debug.LogError("[ObjectPool] Release instance cannot be null!", gameObject);
                return false;
            }

            instance.SetActive(false);
            reusables.Push(instance);
            instance.transform.SetParent(transform);
            CheckDebugInfo(instance);
            return true;
        }

        public bool Release<T>(T instance)
            where T : MonoBehaviour
        {
            return Release(instance.gameObject);
        }
    }
}
