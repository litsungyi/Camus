# Colliders

`Scripts/Colliders`

- Wrapper components turn Unity physics messages into `EventSourcing` events. Call `wrapper.Initialize(eventSourcing)` first, and optionally set `Filter`.
- There are 2D and 3D, Collision and Trigger variants:
  - `*WrapperLight` raises Enter and Exit only.
  - `*WrapperFull` adds Stay. Stay fires every physics frame, so use it only when needed.
- Events: `{Collision|Trigger}{2D|3D}{Enter|Stay|Exit}Event`, which carry `Other` (a `Collision` or `Collider`).
- Filters: `IColliderFilter2D` / `IColliderFilter3D`, with `TagFilter*` (tag list) and `NameFilter*` implementations.
- Wrappers `[RequireComponent]` a Rigidbody and a Collider, and validate in the editor that the collider is enabled.
