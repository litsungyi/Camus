using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Camus.Validators
{
    public static class NotNullPostProcess
    {
        [PostProcessScene]
        public static void OnPostProcessScene()
        {
            var checkPassed = true;
            var allGameObjects = GetAllGameObjects();
            foreach (var gameObject in allGameObjects)
            {
                var components = GetAllComponents(gameObject);
                foreach (var component in components)
                {
                    checkPassed &= HasNullField(component, gameObject);
                }
            }

            if (!checkPassed)
            {
                throw new Exception("Null Check Failed!");
            }
        }

        private static IList<GameObject> GetAllGameObjects()
        {
            var scene = SceneManager.GetActiveScene();
            var rootGameObjects = scene.GetRootGameObjects();
            var gameObjects = new List<GameObject>();
            foreach (var rootGameObject in rootGameObjects)
            {
                gameObjects.Add(rootGameObject);
                var subGameObjects = GetAllChildren(rootGameObject);
                gameObjects.AddRange(subGameObjects);
            }

            return gameObjects;
        }

        private static List<GameObject> GetAllChildren(GameObject root)
        {
            var gameObjects = new List<GameObject>();
            var count = root.transform.childCount;
            for (int i = 0; i < count; ++i)
            {
                var child = root.transform.GetChild(i);
                gameObjects.Add(child.gameObject);

                var subGameObjects = GetAllChildren(child.gameObject);
                gameObjects.AddRange(subGameObjects);
            }

            return gameObjects;
        }

        private static List<Component> GetAllComponents(GameObject gameObject)
        {
            return gameObject.GetComponents<Component>().ToList();
        }

        private static bool HasNullField(Component component, GameObject instance)
        {
            var result = true;
            var type = component.GetType();
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(NotNullAttribute), false);
                if (attributes.Any())
                {
                    var fieldValue = fieldInfo.GetValue(component);
                    if (fieldValue.Equals(null))
                    {
                        Debug.LogError(string.Format("{1} is Null in {0}", instance.name, fieldInfo.Name), instance);
                        result = false;
                    }
                }
            }

            return result;
        }
    }
}
