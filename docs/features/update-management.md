# Update management

`Scripts/Updatables`

- An alternative to per-MonoBehaviour `Update()` that avoids Unity's per-object message overhead.
- Implement `IUpdatable` / `IFixedUpdatable` / `ILateUpdatable`, then call `App.Instance.Updater.Register(this)` (`Unregister` to remove).
- `UpdateManager` keeps a HashSet per phase. Register and unregister calls are **queued and applied at the end of the next tick**, which makes it safe to mutate during iteration. Iteration order is not guaranteed.
- `UpdateManagerHost` (internal MonoBehaviour on App) drives up to 5 managers by `Priority` (VeryHigh → VeryLow) in order. `InternalUpdater` is VeryHigh (framework use) and `Updater` is Middle (game use).
- `Tests/Updatables/*` is a manual benchmark comparing `IUpdatable` against `Update()` across N objects.
