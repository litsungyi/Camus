# Editor tooling

`Editor/`, `Scripts/EditorHelpers`

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
