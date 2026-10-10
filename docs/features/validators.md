# Validators

`Scripts/Validators`, `Editor/Validators`

- Mark required references with `[NotNull]`.
- The menu item **Camus › Validate All Null or Missing Reference** checks every enabled build scene and every prefab under `Assets/` for `[NotNull]` fields that are null and for missing object references (the "Missing" case where the instance ID is non-zero but the object is gone). It throws if any check fails.
- The check also runs automatically before every player build (`IPreprocessBuildWithReport`), and a failure raises `BuildFailedException`.
- Before checking, it asks you to save modified scenes. It also reports missing scripts and `[NotNull]` fields declared in base classes. When it finishes, it restores your open scenes.
- `PostBuild.Replace`: an unsafe, unused method-body swizzling hack. Don't use it.
