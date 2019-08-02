using System.Collections.Generic;
using System.IO;
using Camus.Projects;
using UnityEngine;

namespace Camus.ResourceSystems
{
    public static class AssetBundleLoader
    {
        private static IDictionary<string, AssetBundle> Bundles
        {
            get;
        } = new Dictionary<string, AssetBundle>();

        public static AssetBundle LoadAssetBundle(AssetBundleInfo bundleInfo)
        {
            return LoadAssetBundle(bundleInfo.Name);
        }

        public static AssetBundle LoadAssetBundle(string bundleName)
        {
            if (Bundles.TryGetValue(bundleName, out var assetBundle))
            {
                return assetBundle;
            }

            assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
            if (assetBundle == null)
            {
                return null;
            }

            Bundles.Add(bundleName, assetBundle);
            return assetBundle;
        }

        public static void UnloadAssetBundle(AssetBundleInfo bundleInfo, bool unloadAllLoadedObjects = false)
        {
            UnloadAssetBundle(bundleInfo.Name, unloadAllLoadedObjects);
        }

        public static void UnloadAssetBundle(string bundleName, bool unloadAllLoadedObjects = false)
        {
            if (!Bundles.TryGetValue(bundleName, out var assetBundle))
            {
                return;
            }

            assetBundle.Unload(unloadAllLoadedObjects);
        }

        public static T LoadResource<T>(AssetBundleInfo bundleInfo, string assetName) where T : MonoBehaviour
        {
            return LoadResource<T>(bundleInfo.Name, assetName);
        }

        public static T LoadResource<T>(string bundleName, string assetName) where T : MonoBehaviour
        {
            var bundle = LoadAssetBundle(bundleName);
            if (bundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return null;
            }

            var path = $"Assets/Prefabs/UI/{assetName}.prefab";
            var instance = bundle.LoadAsset<GameObject>(path);
            return instance.GetComponent<T>();
        }

        public static T LoadEffectResource<T>(AssetBundleInfo bundleInfo, string assetName) where T : MonoBehaviour
        {
            return LoadEffectResource<T>(bundleInfo.Name, assetName);
        }

        public static T LoadEffectResource<T>(string bundleName, string assetName) where T : MonoBehaviour
        {
            var bundle = LoadAssetBundle(bundleName);
            if (bundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return null;
            }

            var path = $"Assets/Atlas/boostV001effect/{assetName}.prefab";
            var instance = bundle.LoadAsset<GameObject>(path);
            return instance.GetComponent<T>();
        }
    }
}
