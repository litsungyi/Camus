using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Camus.Projects
{
    public class BinderEditorWindow : EditorWindow
    {
        private readonly string projectInfoTemplate = @"// <autogenerated/>

using Camus.Projects;

namespace {0}
{{
    public static class ProjectInfo
    {{

#region Scenes

        public static class SceneInfos
        {{
{1}
        }}

#endregion

#region Tags

        public static class TagInfos
        {{
{2}
        }}

#endregion

#region Layers

        public static class LayerInfos
        {{
{3}
        }}

#endregion

#region SortingLayers

        public static class SortingLayerInfos
        {{
{4}
        }}

#endregion

#region AssetBundles

        public static class AssetBundleInfos
        {{
{5}
        }}

#endregion
    }}
}}
";

        private const string sceneTemplate = "            public static readonly SceneInfo {0} = new SceneInfo({1}, \"{2}\");\n";
        private const string tagTemplate = "            public static readonly TagInfo {0} = new TagInfo({1}, \"{2}\");\n";
        private const string layerTemplate = "            public static readonly LayerInfo {0} = new LayerInfo({1}, \"{2}\");\n";
        private const string sortingLayerTemplate = "            public static readonly SortingLayerInfo {0} = new SortingLayerInfo({1}, \"{2}\");\n";
        private const string assetBundleTemplate = "            public static readonly AssetBundleInfo {0} = new AssetBundleInfo(\"{1}\");\n";

        private string myNamespace;
        private string folder = "Assets/Scripts/";

        [MenuItem("Camus/Binder Editor Window")]
        private static void Init()
        {
            BinderEditorWindow window = (BinderEditorWindow) EditorWindow.GetWindow(typeof(BinderEditorWindow));
            window.myNamespace = $"{PlayerSettings.companyName}.{PlayerSettings.productName}";
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Setting", EditorStyles.boldLabel);
            myNamespace = EditorGUILayout.TextField("Namespace", myNamespace);
            folder = EditorGUILayout.TextField("Folder", folder);
            if (GUILayout.Button("Parse"))
            {
                StringBuilder builder = new StringBuilder();
                var scenes = ParseScenes();
                var tags = ParseTags();
                var layers = ParseLayers();
                var sortingLayers = ParseSortingLayers();
                var assetBundles = ParseAssetBundles();

                builder.AppendFormat(projectInfoTemplate, myNamespace, scenes, tags, layers, sortingLayers, assetBundles);
                File.WriteAllText($"{folder}/ProjectInfo.cs", builder.ToString());
            }
        }

        private string ParseScenes()
        {
            StringBuilder builder = new StringBuilder();
            var max = SceneManager.sceneCountInBuildSettings;
            for (int index = 0; index < max; ++index)
            {
                var sceneSetting = EditorBuildSettings.scenes[index];
                //var scene = SceneManager.GetSceneByPath(sceneSetting.path);
                var sceneName = ParsePath(sceneSetting.path);
                var scene = SceneManager.GetSceneByBuildIndex(index);
                builder.AppendFormat(sceneTemplate, ParseName(sceneName), index, sceneName);
            }

            return builder.ToString();
        }

        private string ParseTags()
        {
            StringBuilder builder = new StringBuilder();
            var tags = UnityEditorInternal.InternalEditorUtility.tags;
            for (int index = 0; index < tags.Length; ++index)
            {
                var tag = tags[index];
                builder.AppendFormat(tagTemplate, ParseName(tag), index, tag);
            }

            return builder.ToString();
        }

        private string ParseLayers()
        {
            StringBuilder builder = new StringBuilder();
            var layers = UnityEditorInternal.InternalEditorUtility.layers;
            for (int index = 0; index < layers.Length; ++index)
            {
                var layer = layers[index];
                builder.AppendFormat(layerTemplate, ParseName(layer), index, layer);
            }

            return builder.ToString();
        }

        private string ParseSortingLayers()
        {
            StringBuilder builder = new StringBuilder();
            var sortingLayers = SortingLayer.layers;
            for (int index = 0; index < sortingLayers.Length; ++index)
            {
                var sortingLayer = sortingLayers[index];
                builder.AppendFormat(sortingLayerTemplate, ParseName(sortingLayer.name), sortingLayer.id, sortingLayer.name);
            }

            return builder.ToString();
        }

        private string ParseAssetBundles()
        {
            StringBuilder builder = new StringBuilder();
            var assetBundles = AssetDatabase.GetAllAssetBundleNames();
            for (int index = 0; index < assetBundles.Length; ++index)
            {
                var assetBundle = assetBundles[index];
                builder.AppendFormat(assetBundleTemplate, ParseName(assetBundle), assetBundle);
            }

            return builder.ToString();
        }

        private string ParseName(string text)
        {
            return text.Replace(" ", "_").Replace("/", "_").Replace("-", "_").Replace(".", "_").Replace(",", "_");
        }

        private string ParsePath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}
