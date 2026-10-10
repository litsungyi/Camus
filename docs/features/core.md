# Core

`Scripts/Core`

- **`Singleton<T>`**: a MonoBehaviour singleton. `Instance` finds an existing instance, or creates one on a shared `(Singletons)` GameObject marked `DontDestroyOnLoad`. It is lock-guarded and returns `null` after app quit, to avoid creating ghost objects. **Any** singleton's `OnDestroy` sets the quitting flag.
- **`App`**: the global partial singleton and the framework entry point. `Awake` builds a `SceneController` and, in the editor, enables the FPS overlay. `EnableFps(bool)` is editor-only.
- **`Logger` / `LogHandler`**: a static logging facade with optional enum tags (`Log(tag, msg)` → `[TAG] msg`). `LogHandler` installs itself as `Debug.unityLogger.logHandler` and forwards to Unity's default handler. This is a hook point for custom sinks.
- **`Ids/Identity<T>`**: an abstract, serializable, strongly-typed ID wrapper (supports int/uint/long/ulong/string) with value equality, plus `IdentitySerializer` for Newtonsoft JSON. Subclass it per entity, e.g. `class ItemId : Identity<int>`.
- **`Projects/*Info`**: `SceneInfo`, `TagInfo`, `LayerInfo`, `SortingLayerInfo`, `AssetBundleInfo` are `IEquatable` value structs (id + name). They are generated into a `ProjectInfo` class by the editor Binder window (see [Editor tooling](editor-tooling.md)) and consumed by `SceneController`, `AssetBundleLoader`, and `IsTagged` extensions.
