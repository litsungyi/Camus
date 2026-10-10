# UI framework

`Scripts/UiUtilities`

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
