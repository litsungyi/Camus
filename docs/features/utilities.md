# Utilities

`Scripts/Utilities`

- `Range`: an integer range that can be enumerated with `foreach`. Factories: `CreateCloseRange` (the default), `CreateOpenRange`, `CreateOpenCloseRange`, and `CreateCloseOpenRange`. The `End` field is stored as an exclusive bound.
- `RandomHelper`: `Next(a, b)` takes the bounds in either order. `Take(list, n)` samples without replacement using a partial Fisher–Yates shuffle.
- `FPS`: a rolling 100-frame average with a colour-coded editor OnGUI overlay.
- `GameObjectExtends.Append(prefab)` instantiates under a parent. `AppendComponent<T>` adds a component. `IsTagged(TagInfo)` is available on GameObject and Component.
- `ReadOnlyAttribute`: shows a serialized field greyed out in the inspector.
