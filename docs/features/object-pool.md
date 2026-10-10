# Object pool

`Scripts/ObjectPools`

- `ObjectPool` is a MonoBehaviour configured in the inspector: `prefab`, `initialSize`, `growSize` (at least 1), `maxGrowTime`.
- It prewarms in `Awake`. `Acquire(parent)` / `Acquire<T>(parent)` activates and returns an instance, growing by `growSize` up to `maxGrowTime` times. It returns `null` once exhausted.
- `Release(go)` / `Release<T>(comp)` deactivates the instance and reparents it to the pool.
- In the editor, the internal `ObjectPoolRecord` tags each instance with its owning pool to catch foreign releases.
