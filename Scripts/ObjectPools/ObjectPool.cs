using System.Collections.Generic;
using System.Linq;
using Camus.ObjectPools.Internal;
using UnityEngine;
using UnityEngine.Assertions;

namespace Camus.ObjectPools
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private bool fixedSize;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private int minInstance;

        private readonly Stack<GameObject> reusables = new Stack<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < minInstance; ++i)
            {
                var instance = Instantiate(prefab, transform);
                AddDebugInfo(instance);
                instance.SetActive(false);
                reusables.Push(instance);
            }
        }

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

        private GameObject CreateInstance()
        {
            if (fixedSize)
            {
                Debug.LogError("Fixed ObjectPool cannot growth.");
                return null;
            }

            var instance = Instantiate(prefab, transform);
            AddDebugInfo(instance);
            return instance;
        }

        public GameObject Require(Transform parent = null)
        {
            var instance = reusables.Any() ? reusables.Pop() : CreateInstance();
            instance?.SetActive(true);
            if (parent != null)
            {
                instance?.transform.SetParent(parent);
            }

            return instance;
        }

        public T Require<T>(Transform parent = null)
            where T : MonoBehaviour
        {
            var instance = Require(parent);
            var target = instance.GetComponent(typeof(T));
            return target as T;
        }

        public void Release(GameObject instance)
        {
            Assert.IsNotNull(instance);

            instance.SetActive(false);
            reusables.Push(instance);
            instance.transform.SetParent(transform);
            CheckDebugInfo(instance);
        }

        public void Release<T>(T instance)
            where T : MonoBehaviour
        {
            Release(instance.gameObject);
        }
    }
}
