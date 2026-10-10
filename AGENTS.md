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

One file per feature in `docs/features/`. Read the relevant one before changing that module.

- [Core](docs/features/core.md) (`Scripts/Core`): `Singleton<T>`, `App`, `Logger`, `Identity<T>` IDs, `Projects/*Info` structs.
- [Event systems](docs/features/event-systems.md) (`Scripts/EventSystems`): `EventDispatcher` (global, `IDomainEvent`) and `EventSourcing` (local, `IEvent`).
- [Update management](docs/features/update-management.md) (`Scripts/Updatables`): `IUpdatable` & co. driven by `UpdateManager` instead of `Update()`.
- [Timers](docs/features/timers.md) (`Scripts/Timers`): `TimerBase` and its six sealed variants.
- [State machine](docs/features/state-machine.md) (`Scripts/StateMachines`): `StateMachine<TOwner>` with an explicit transition table.
- [Input](docs/features/input.md) (`Scripts/InputSystems`, namespace `Camus.Inputs`): `InputManager` on the Input System package; press/move/axis events.
- [Drag and drop](docs/features/drag-and-drop.md) (`Scripts/Draggables`): `DragSource` / `DropTarget` / `DragManager`, merge and drop rules.
- [Localization](docs/features/localization.md) (`Scripts/Localizables`): `LocalizationManager`, `LocalKey`, `LocalStringUpdater`.
- [Object pool](docs/features/object-pool.md) (`Scripts/ObjectPools`): `ObjectPool` acquire/release.
- [Scenes](docs/features/scenes.md) (`Scripts/Scenes`): `SceneController.LoadSceneAsync` and `IScene` callbacks.
- [UI framework](docs/features/ui-framework.md) (`Scripts/UiUtilities`): `UiManager`, `UiBaseController`/`UiBaseView`, button helpers.
- [Colliders](docs/features/colliders.md) (`Scripts/Colliders`): Collision/trigger wrappers that raise `EventSourcing` events, filters.
- [Resources](docs/features/resources.md) (`Scripts/ResourceSystems`): `AssetBundleLoader` (partly legacy, hard-coded paths).
- [Persistence](docs/features/persistence.md) (`Scripts/Persistance/Sqlite`, spelled this way in the repo): `DataService` SQLite stub.
- [Utilities](docs/features/utilities.md) (`Scripts/Utilities`): `Range`, `RandomHelper`, `FPS`, GameObject extensions, `ReadOnlyAttribute`.
- [Validators](docs/features/validators.md) (`Scripts/Validators`, `Editor/Validators`): `[NotNull]` and the missing-reference build validator.
- [Editor tooling](docs/features/editor-tooling.md) (`Editor/`, `Scripts/EditorHelpers`): Binder window (`ProjectInfo.cs`), gizmos, inspector/editor-window bases.

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
3. `PostBuild.Replace` (Editor) is an unused, unsafe method-swizzling hack that depends on runtime memory layout. Don't call it.
4. `IButtonProcessingHandler` defines the processing flag, but nothing enforces it.
5. `Camus-Tests` (the manual harnesses) is included in player builds of consumer projects, and its classes are in the global namespace.

## Agent checklist

- Put new runtime code under `Scripts/<Module>/` with namespace `Camus.<Module>`. Put editor code under `Editor/`.
- Add `.meta` files for every new asset or folder.
- Wire global singletons through a new `partial class App` file, not by editing other modules' `App` files.
- Use `EventDispatcher` + `IDomainEvent` for app-wide events, and `EventSourcing` + `IEvent` for component-local events. Always pair Register with Unregister.
- Mark required serialized references `[NotNull]`, and use `[ReadOnly]` for debug-visible state.
- When you add, remove, or significantly change a feature, update its file in `docs/features/` and the Features index here.
