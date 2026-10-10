# Resources

`Scripts/ResourceSystems`

- `AssetBundleLoader` (static) loads and caches bundles from `StreamingAssets` by name or `AssetBundleInfo`. `UnloadAssetBundle` unloads a bundle.
- `LoadResource<T>` and `LoadEffectResource<T>` use **hard-coded, game-specific asset paths** (`Assets/Prefabs/UI/…`, `Assets/Atlas/boostV001effect/…`). Treat them as legacy code. They return `null` if the asset is missing.
