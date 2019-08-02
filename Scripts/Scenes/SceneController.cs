using System.Collections;
using Camus.Projects;
using UnityEngine.SceneManagement;

namespace Camus.Scenes
{
    public class SceneController
    {
        private IScene currentScene;

        public IEnumerator LoadSceneAsync(SceneInfo sceneInfo)
        {
            currentScene?.OnSceneUnloaded();
            var loading = SceneManager.LoadSceneAsync(sceneInfo.Index);
            while (!loading.isDone)
            {
                yield return null;
            }

            var scene = SceneManager.GetActiveScene();
            currentScene?.OnSceneUnloaded();
        }
    }
}
