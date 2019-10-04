using System.Collections;
using System.Linq;
using Camus.Projects;
using UnityEngine.SceneManagement;

namespace Camus.Scenes
{
    public class SceneController
    {
        private IScene currentScene;

        public void LoadSceneAsync(SceneInfo sceneInfo)
        {
            App.Instance.StartCoroutine(DoLoadSceneAsync(sceneInfo));
        }

        private IEnumerator DoLoadSceneAsync(SceneInfo sceneInfo)
        {
            currentScene?.OnSceneUnloaded();
            var oldScene = SceneManager.GetActiveScene();
            var loading = SceneManager.LoadSceneAsync(sceneInfo.Index);
            while (!loading.isDone)
            {
                yield return null;
            }

            var newScene = SceneManager.GetActiveScene();
            currentScene = newScene.GetRootGameObjects().FirstOrDefault(s => s.GetComponent<IScene>() != null)?.GetComponent<IScene>();

            currentScene?.OnSceneLoaded();
            var unloading = SceneManager.UnloadSceneAsync(oldScene.buildIndex);
            while (!unloading.isDone)
            {
                yield return null;
            }
        }
    }
}
