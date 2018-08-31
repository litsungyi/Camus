using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camus.ObjectPools
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private int minInstance;

        private Stack<GameObject> reusables = new Stack<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < minInstance; ++i)
            {
                var instance = CreateInstance();
                Release(instance);
            }
        }

        private GameObject CreateInstance()
        {
            var instance = GameObject.Instantiate(prefab, transform);
            return instance;
        }

        public GameObject Require()
        {
            var instance = reusables.Any() ? reusables.Pop() : CreateInstance();
            instance.SetActive(true);
            return instance;
        }

        public void Release(GameObject instance)
        {
            instance.SetActive(false);
            reusables.Push(instance);
        }
    }
}
