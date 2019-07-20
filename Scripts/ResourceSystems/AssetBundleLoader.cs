using System.IO;
using UnityEngine;

namespace Camus.ResourceSystems
{
    public static class AssetBundleLoader
    {
        public static T LoadResource<T>(string bundleName, string assetName) where T : MonoBehaviour
        {
            var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
            return bundle.LoadAsset<T>(assetName);
        }
    }
}
