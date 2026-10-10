# AGENTS.md

Guide for AI agents working in **Camus** (`com.litsungyi.camus`), a small Unity game-framework / utility library distributed as a Unity package (UPM, Git URL). This repo is **only the package**; there is no Unity project, no scenes, and no `Assets/` folder here. You cannot compile or run it from this repo alone; changes are verified by importing the package into a host Unity project.

## Package facts

| Item | Value |
|---|---|
| Unity | `6000.0`+, verified to compile warning-free (except one unused local) on 6000.3.16f1 |
| Manifest | `package.json` (version `0.0.1`) |
| Dependencies | `com.unity.ugui`, `com.unity.nuget.newtonsoft-json`. Only add dependencies that resolve from the Unity registry; an unresolvable one breaks install for every consumer (this happened with UniRx). |
| Bundled dependency | `Dependencies/SQLite4Unity3d.unitypackage`. It **must be imported into the host project first**, because `Camus.asmdef` references the `SQLite4Unity3d` assembly. Without it, the package fails to compile (`DataService.cs`). |

### Assemblies

| asmdef | Folder | Platforms | References |
|---|---|---|---|
| `Camus` | `Scripts/` (root asmdef at repo root) | all | `SQLite4Unity3d` |
| `Camus-Editor` | `Editor/` | Editor only, `allowUnsafeCode: true` | `Camus` |
| `Camus-Tests` | `Tests/` (manual harness MonoBehaviours) | all | `Camus`, `Camus-Editor`, TestAssemblies |
| `Camus-Editor-Tests` | `Tests/Editor/` | Editor, `UNITY_INCLUDE_TESTS` | `Camus`, Test Runner, NUnit |

Runtime code must not reference `UnityEditor` unless wrapped in `#if UNITY_EDITOR` (see `GizmoHelper`, `FPS`).

## Conventions

- **Namespaces:** `Camus.<Module>` (e.g. `Camus.Timers`). Exceptions:
  - `Inputs` lives in `Scripts/InputSystems/` but uses namespace `Camus.Inputs`.
  - `Projects` structs live in `Scripts/Core/Projects/` but use namespace `Camus.Projects`.
  - Global namespace (intentional): `App` (partial, split across modules), `DataService`, `PostBuild`, and the test harness classes.
- **Style** (`.editorconfig`): 4 spaces, LF, UTF-8, final newline, trim trailing whitespace. Allman braces. Auto-properties are written with multi-line `get; set;`. Group members with `#region` blocks. Use explicit interface implementations for framework callbacks (`void IUpdatable.OnUpdate(...)`).
- **Line endings / encoding:** `.gitattributes` forces LF on `.cs`, `.meta`, `.asset`, `.prefab`, `.json`. All files are UTF-8.
- **`Range` name clash:** `Camus.Utilities.Range` collides with `System.Range` when a file imports both `System` and `Camus.Utilities`. Use `using Range = Camus.Utilities.Range;` in those files.
- **`.meta` files:** every new file or folder needs a `.meta` file with a unique GUID. Don't delete or regenerate existing `.meta` files, because that breaks references in consumer projects.
- **Editor-only debug code:** use `[System.Diagnostics.Conditional("UNITY_EDITOR")]` (see `ObjectPool`, collider wrappers, `App.EnableFps`).
- **Commits:** atomic commits, Conventional-Commit-ish subjects (`feat:`, `fix:`, `chore (scope):`, `refactor:`, `style:`). Write a short imperative subject (50 characters max) and wrap the body at 72 characters. See `README.md`.
- **Architecture decisions:** record them in `docs/adr/` using MADR (`template.md`), and update `docs/adr/index.md`.
- **Versioning:** SemVer in `package.json`.

## Architecture overview

```
App (global partial Singleton<App>, lives under "(Singletons)" DontDestroyOnLoad GO)
 ├─ Core/App.cs         → SceneController, LocalizationManager (init commented out), FPS overlay
 ├─ Updatables/App.cs   → Updater (Middle priority), InternalUpdater (VeryHigh) via UpdateManagerHost
 └─ InputSystems/App.cs → InputManager (lazy AddComponent)

Global events: EventDispatcher (static, IDomainEvent)   ← Input, Drag, Locale events
Local events:  EventSourcing (instance API, IEvent)      ← UI controller/view, collider wrappers
```

To extend the app, add another `public partial class App` file in your module rather than editing `Core/App.cs`.

## Features

### Core (`Scripts/Core`)
- **`Singleton<T>`**: a MonoBehaviour singleton. `Instance` finds an existing instance, or creates one on a shared `(Singletons)` GameObject marked `DontDestroyOnLoad`. It is lock-guarded and returns `null` after app quit, to avoid creating ghost objects. **Any** singleton's `OnDestroy` sets the quitting flag.
- **`App`**: the global partial singleton and the framework entry point. `Awake` builds a `SceneController` and, in the editor, enables the FPS overlay. `EnableFps(bool)` is editor-only.
- **`Logger` / `LogHandler`**: a static logging facade with optional enum tags (`Log(tag, msg)` → `[TAG] msg`). `LogHandler` installs itself as `Debug.unityLogger.logHandler` and forwards to Unity's default handler. This is a hook point for custom sinks.
- **`Ids/Identity<T>`**: an abstract, serializable, strongly-typed ID wrapper (supports int/uint/long/ulong/string) with value equality, plus `IdentitySerializer` for Newtonsoft JSON. Subclass it per entity, e.g. `class ItemId : Identity<int>`.
- **`Projects/*Info`**: `SceneInfo`, `TagInfo`, `LayerInfo`, `SortingLayerInfo`, `AssetBundleInfo` are `IEquatable` value structs (id + name). They are generated into a `ProjectInfo` class by the editor Binder window (see Editor tooling) and consumed by `SceneController`, `AssetBundleLoader`, and `IsTagged` extensions.

### Event systems (`Scripts/EventSystems`)
- **`EventDispatcher`**: a static, global, type-keyed pub/sub for `IDomainEvent`. `Register<T>`, `Unregister<T>`, `Raise<T>`. It uses a static generic `EventHost<T>`, so dispatch involves no dictionary lookup. Always unregister in `OnDisable`/`OnDestroy`.
- **`EventSourcing`**: the same API for `IEvent`, but **instance-scoped**: each instance has its own subscribers. For example, each UI controller or collider wrapper has its own `EventSourcing`.
- **`EventSource<T>`**: an instance-scoped alternative, constrained to a base event type `T`. It is currently unused.
- Marker interfaces: `IDomainEvent` for global/app events and `IEvent` for local/component events.

### Update management (`Scripts/Updatables`)
- An alternative to per-MonoBehaviour `Update()` that avoids Unity's per-object message overhead.
- Implement `IUpdatable` / `IFixedUpdatable` / `ILateUpdatable`, then call `App.Instance.Updater.Register(this)` (`Unregister` to remove).
- `UpdateManager` keeps a HashSet per phase. Register and unregister calls are **queued and applied at the end of the next tick**, which makes it safe to mutate during iteration. Iteration order is not guaranteed.
- `UpdateManagerHost` (internal MonoBehaviour on App) drives up to 5 managers by `Priority` (VeryHigh → VeryLow) in order. `InternalUpdater` is VeryHigh (framework use) and `Updater` is Middle (game use).
- `Tests/Updatables/*` is a manual benchmark comparing `IUpdatable` against `Update()` across N objects.

### Timers (`Scripts/Timers`)
- `TimerBase` (MonoBehaviour) holds a serialized `TimerSetting { interval, loop, maxCount }`. It calls `onTimer` every `interval` seconds and stops after `maxCount` ticks unless `loop` is set.
- Controls: `StartTimer(onTimer)`, `StartTimer(delta, onTimer)`, `RestartTimer`, `StopTimer`, `PauseTimer`, `ResumeTimer`, and `UpdateSetting`. `Status` is one of None/Started/Paused/Stopped. Pausing and stopping disable the component.
- There are six concrete, sealed variants. Choose by update phase and driver:
  - Unity message driven: `UpdateTimer`, `FixedUpdateTimer`, `LateUpdateTimer`
  - `App.Updater` driven (via `TimerUpdatableBase`, which auto-registers in `OnEnable`): `UpdatableTimer`, `FixedUpdatableTimer`, `LateUpdatableTimer`
- `OnValidate` asserts `interval > 0` and `maxCount >= 0`. `Tests/Timers/TimerTest` is an OnGUI harness for manual testing.

### Input (`Scripts/InputSystems`, namespace `Camus.Inputs`)
- `InputManager` is an `IUpdatable` on App, accessed through `App.Instance.InputManager`. It uses the **legacy `UnityEngine.Input`** API, not the new Input System package.
- Each frame it raises the following through `EventDispatcher`:
  - Mouse buttons (Primary/Secondary/Middle) and touches (`Button.TouchBaseIndex + i`): `BeginPressEvent`, then either `PressEvent` (shorter than `LongPressThreshold`, default 0.5 s) or `LongPressEvent`, then `EndPressEvent`.
  - `MovingEvent` (position, direction, distance): every frame for the mouse, and on `TouchPhase.Moved` for touches.
  - `InputEvent`: a snapshot of `Input.GetAxis` for all registered axis keys (Unity defaults are preregistered; add more with `RegisterInput(key)`). Helper methods: `IsHigh`, `IsLow`, `IsPositive`, `IsNegative`, `GetValue`.
- `InputManager` registers itself with `App.Updater` when it is first accessed through `App.Instance.InputManager`. Events start on the next tick after that first access.
- In Player Settings, **Active Input Handling** must be "Input Manager (Old)" or "Both". With "Input System Package (New)" only, `UnityEngine.Input` throws.

### Drag and drop (`Scripts/Draggables`)
- Built on uGUI `EventSystem` handlers. Subclass `DragSource` (begin/drag/end) and `DropTarget` (drop). The scene needs an `EventSystem`, and objects need raycast targets.
- `DragManager` (static) tracks active drags and moves the source to the pointer position (screen space, which suits a screen-space canvas).
- Override hooks:
  - `DragSource.GetDraggableResult()`: Accepted or Denied.
  - `DragSource.GetDraggedResult()`: None, or ResetPosition when the drag ends without a drop.
  - `DropTarget.GetDroppableResult(source)`: Accepted or Denied.
  - Callbacks: `OnBeginDragging`, `OnEndDragging`, `OnCancelDragging`, `OnDropped`.
- Global events: `BeginDragEvent`, `EndDragEvent`, and `DropEvent(source, target)`.
- **Caution:** `DropTarget.IsDroppable` defaults to `false`. Set it to `true`, or drops are ignored.

### Localization (`Scripts/Localizables`)
- `LocalizationManager.Initialize(language, IList<LocalData>)` loads string tables. `LocalData` is `{ Key, Values: Language→string }`, JSON via `LanguageDictionarySerializer`.
- The `Language` enum uses ISO-639-style codes: Zh_Tw, Zh_Cn, En, Ja, Pt, Es, Fr, De, Ko, Ru, Ar.
- `GetLocalKey(key)` returns a cached `LocalKey`. `LocalKey.Value` lazily resolves the string through `App.Instance.LocalizationManager` and caches it until the language changes, which marks all keys dirty. A missing key returns `LOCAL_KEY_NOT_FOUND: <key>`.
- `LocalStringUpdater` binds a uGUI `Text` to a key and refreshes on `LocaleChangedEvent`.
- `LocalKeyDrawer` (editor) draws the key string in the inspector.
- `App` creates an empty manager; the host loads data with `App.Instance.LocalizationManager.Initialize(language, datas)`. Setting `CurrentLanguage` raises `LocaleChangedEvent`, and bound `LocalStringUpdater`s refresh.

### Object pool (`Scripts/ObjectPools`)
- `ObjectPool` is a MonoBehaviour configured in the inspector: `prefab`, `initialSize`, `growSize` (at least 1), `maxGrowTime`.
- It prewarms in `Awake`. `Acquire(parent)` / `Acquire<T>(parent)` activates and returns an instance, growing by `growSize` up to `maxGrowTime` times. It returns `null` once exhausted.
- `Release(go)` / `Release<T>(comp)` deactivates the instance and reparents it to the pool.
- In the editor, the internal `ObjectPoolRecord` tags each instance with its owning pool to catch foreign releases.

### Scenes (`Scripts/Scenes`)
- `App.Instance.SceneController.LoadSceneAsync(SceneInfo)` runs a coroutine on App:
  1. Calls `OnSceneUnloaded()` on the current `IScene`.
  2. Loads the new scene (single mode).
  3. Finds the first **root** GameObject that has an `IScene` component and calls `OnSceneLoaded()`.
- An `IScene` on a root object is optional. If no root object has one, the callbacks are skipped.

### UI framework (`Scripts/UiUtilities`)
- `UiManager` is a scene-placed MonoBehaviour with a static `Instance` (not `Singleton<T>`). It needs a `uiRoot`. It registers controllers by name:
  - `CreateUi`, `CreateAndShowUi`, `ShowUi`, `HideUi(name, cb, destroy)`.
  - `CreateEffect<T>(prefab)` instantiates the prefab under `uiRoot`.
- `UiBaseController<TView>` + `UiBaseView` form an MVC-ish pair:
  - The controller implements `IDisplyable` (Show/Hide/Demolish) and owns an `EventSourcing` that it injects into the view.
  - The view raises UI events and the controller subscribes to them.
  - Override hooks: `OnＡwaked` (note: **full-width `Ａ`** in the identifier), `OnDestroied`, `RegisterEvents`, `UnregisterEvents`, `InitializeView`.
  - If an `Animator` is assigned, the controller sets its `"Show"` bool.
- `ButtonHelper` wraps a `Button` and provides `SetOnClickedCallback`, `SetText`, and `SetColor`. `ButtonExtension.SetColor` does the same as an extension method.
- `IButtonProcessingHandler` / `IButtonProcessingExternalHandler` provide a "processing" flag (`BeginProcessing`/`EndProcessing`) intended to block double-clicks. The flag is not enforced anywhere yet.

### Colliders (`Scripts/Colliders`)
- Wrapper components turn Unity physics messages into `EventSourcing` events. Call `wrapper.Initialize(eventSourcing)` first, and optionally set `Filter`.
- There are 2D and 3D, Collision and Trigger variants:
  - `*WrapperLight` raises Enter and Exit only.
  - `*WrapperFull` adds Stay. Stay fires every physics frame, so use it only when needed.
- Events: `{Collision|Trigger}{2D|3D}{Enter|Stay|Exit}Event`, which carry `Other` (a `Collision` or `Collider`).
- Filters: `IColliderFilter2D` / `IColliderFilter3D`, with `TagFilter*` (tag list) and `NameFilter*` implementations.
- Wrappers `[RequireComponent]` a Rigidbody and a Collider, and validate in the editor that the collider is enabled.

### Resources (`Scripts/ResourceSystems`)
- `AssetBundleLoader` (static) loads and caches bundles from `StreamingAssets` by name or `AssetBundleInfo`. `UnloadAssetBundle` unloads a bundle.
- `LoadResource<T>` and `LoadEffectResource<T>` use **hard-coded, game-specific asset paths** (`Assets/Prefabs/UI/…`, `Assets/Atlas/boostV001effect/…`). Treat them as legacy code. They return `null` if the asset is missing.

### Persistence (`Scripts/Persistance/Sqlite`, spelled this way in the repo)
- `DataService(dbName)` copies `StreamingAssets/<db>` to `persistentDataPath` on first run (using a blocking `UnityWebRequest` for `://` paths such as Android) and opens a `SQLiteConnection`. It is a stub: there are no query methods yet, and the connection is private. It lives in the global namespace.

### Utilities (`Scripts/Utilities`)
- `Range`: an integer range that can be enumerated with `foreach`. Factories: `CreateCloseRange` (the default), `CreateOpenRange`, `CreateOpenCloseRange`, and `CreateCloseOpenRange`. The `End` field is stored as an exclusive bound.
- `RandomHelper`: `Next(a, b)` takes the bounds in either order. `Take(list, n)` samples without replacement using a partial Fisher–Yates shuffle.
- `FPS`: a rolling 100-frame average with a colour-coded editor OnGUI overlay.
- `GameObjectExtends.Append(prefab)` instantiates under a parent. `AppendComponent<T>` adds a component. `IsTagged(TagInfo)` is available on GameObject and Component.
- `ReadOnlyAttribute`: shows a serialized field greyed out in the inspector.

### Validators (`Scripts/Validators`, `Editor/Validators`)
- Mark required references with `[NotNull]`.
- The menu item **Camus › Validate All Null or Missing Reference** checks every enabled build scene and every prefab under `Assets/` for `[NotNull]` fields that are null and for missing object references (the "Missing" case where the instance ID is non-zero but the object is gone). It throws if any check fails.
- The check also runs automatically before every player build (`IPreprocessBuildWithReport`), and a failure raises `BuildFailedException`.
- Before checking, it asks you to save modified scenes. It also reports missing scripts and `[NotNull]` fields declared in base classes. When it finishes, it restores your open scenes.
- `PostBuild.Replace`: an unsafe, unused method-body swizzling hack. Don't use it.

### Editor tooling (`Editor/`, `Scripts/EditorHelpers`)
- **Camus › Binder Editor Window** (`BinderEditorWindow`) generates `<folder>/ProjectInfo.cs`. Each section is a nested static class of typed constants:
  - `SceneInfos`: build scenes
  - `TagInfos`: tags
  - `LayerInfos`: layers
  - `SortingLayerInfos`: sorting layers
  - `AssetBundleInfos`: asset bundle names
- Scene IDs are the build indices of **enabled** scenes. The generated file uses namespace `Company.Product` by default and is written to `Assets/Scripts/` by default. Use it instead of magic strings: `SceneController.LoadSceneAsync(ProjectInfo.SceneInfos.Main)`.
- `GizmoDrawer` + `GizmoHelper` draw configurable scene gizmos: a collider wireframe or solid, a disc, an icon, and a label with a selected colour. `GizmoDrawerInspector` (based on `BaseInspector`) provides the custom inspector.
- `SelectableRoot`: a `[SelectionBase]` marker, so a scene click selects the root object.
- `BaseInspector`: a template custom editor. It draws a read-only script field, then calls `OnDrawInspector()`, and applies changes if that returns true.
- `BaseEditorWindow`: currently empty.
- `ScriptableObjectUtility.CreateAsset<T>()`: creates a ScriptableObject asset in the selected folder.
- Property drawers: `ReadOnlyDrawer` and `LocalKeyDrawer`.

## Tests

- **Automated tests:** NUnit EditMode tests live in `Tests/Editor/` (asmdef `Camus-Editor-Tests`). To run them, set up a host project that:
  - lists the package under `"testables": ["com.litsungyi.camus"]` in `Packages/manifest.json`;
  - has SQLite4Unity3d imported into it.

  Then run:
  ```
  Unity -batchmode -nographics -projectPath <host> -runTests -testPlatform EditMode -assemblyNames Camus-Editor-Tests -testResults results.xml
  ```
  Add tests there for any pure logic you change.
- **Manual harnesses:** `TimerTest`, `UpdaterTester`, `UpdatableTest`, and `UpdateTest` are play-mode MonoBehaviours in `Tests/`. Drop them into a scene to try them out.
- **Compile check without tests:** run the same Unity command without `-runTests` and with `-quit`, then grep the log for `error CS`.

## Known issues and caveats

1. `Singleton<T>` sets its "quitting" flag in `OnDestroy`. If you destroy a singleton manually, `Instance` then returns `null` for the rest of the session. The flag is static, so with domain reload disabled (Enter Play Mode Options) it also persists into the next play session.
2. `UpdateManager` holds strong references. Destroyed objects must `Unregister`, or they keep being ticked.
3. `DragManager.EndDrag` never calls its `OnCancelDrag` local function. Ending a drag without a drop counts as an "end", not a "cancel". `DroppedResult` is unused.
4. `PostBuild.Replace` (Editor) is an unused, unsafe method-swizzling hack that depends on runtime memory layout. Don't call it.
5. `IButtonProcessingHandler` defines the processing flag, but nothing enforces it.
6. `Camus-Tests` (the manual harnesses) is included in player builds of consumer projects, and its classes are in the global namespace.

## Agent checklist

- Put new runtime code under `Scripts/<Module>/` with namespace `Camus.<Module>`. Put editor code under `Editor/`.
- Add `.meta` files for every new asset or folder.
- Wire global singletons through a new `partial class App` file, not by editing other modules' `App` files.
- Use `EventDispatcher` + `IDomainEvent` for app-wide events, and `EventSourcing` + `IEvent` for component-local events. Always pair Register with Unregister.
- Mark required serialized references `[NotNull]`, and use `[ReadOnly]` for debug-visible state.
- Update this file when you add, remove, or significantly change a feature.
