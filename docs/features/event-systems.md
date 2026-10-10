# Event systems

`Scripts/EventSystems`

- **`EventDispatcher`**: a static, global, type-keyed pub/sub for `IDomainEvent`. `Register<T>`, `Unregister<T>`, `Raise<T>`. It uses a static generic `EventHost<T>`, so dispatch involves no dictionary lookup. Always unregister in `OnDisable`/`OnDestroy`.
- **`EventSourcing`**: the same API for `IEvent`, but **instance-scoped**: each instance has its own subscribers. For example, each UI controller or collider wrapper has its own `EventSourcing`.
- **`EventSource<T>`**: an instance-scoped alternative, constrained to a base event type `T`. It is currently unused.
- Marker interfaces: `IDomainEvent` for global/app events and `IEvent` for local/component events.
