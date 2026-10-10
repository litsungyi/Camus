# Scenes

`Scripts/Scenes`

- `App.Instance.SceneController.LoadSceneAsync(SceneInfo)` runs a coroutine on App:
  1. Calls `OnSceneUnloaded()` on the current `IScene`.
  2. Loads the new scene (single mode).
  3. Finds the first **root** GameObject that has an `IScene` component and calls `OnSceneLoaded()`.
- An `IScene` on a root object is optional. If no root object has one, the callbacks are skipped.
