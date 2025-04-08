using System.Collections;
using System.Linq;
using Camus.Projects;
using UnityEngine;
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
            // var oldScene = SceneManager.GetActiveScene();
            // var unloading = SceneManager.UnloadSceneAsync(oldScene);
            // yield return WaitUntilDone(unloading);

            var loading = SceneManager.LoadSceneAsync(sceneInfo.Name);
            yield return WaitUntilDone(loading);

            var newScene = SceneManager.GetActiveScene();
            currentScene = newScene.GetRootGameObjects().FirstOrDefault(s => s.GetComponent<IScene>() != null).GetComponent<IScene>();
            currentScene?.OnSceneLoaded();

            static IEnumerator WaitUntilDone(AsyncOperation async)
            {
                if (async != null)
                {
                    while (!async.isDone)
                    {
                        yield return null;
                    }
                }
            }
        }
    }
}
