// Ref. http://wiki.unity3d.com/index.php/CreateScriptableObjectAsset

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Camus
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static void CreateAsset<T>() where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            CreateAsset<T>(path);
        }

        public static void CreateAsset<T>(string path) where T : ScriptableObject
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            T asset = ScriptableObject.CreateInstance<T>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Join(path, typeof(T).Name + ".asset"));
            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
