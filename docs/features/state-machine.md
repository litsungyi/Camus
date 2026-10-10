# State machine

`Scripts/StateMachines`

- `StateMachine<TOwner>` is a plain C# finite state machine with an explicit transition table:
  - Define states with `AddState(state)` (one instance per state type), then each allowed change with `AddTransition<TFrom, TTo>()`. All calls chain. `TFrom == TTo` allows re-entering a state.
  - `AddTransition<TFrom, TTo>(owner => condition)` also makes `Update` take the transition when the condition is true (checked after `OnUpdate`, in the order added).
  - `Start<TState>()` enters the first state. `ChangeState<TState>()` throws `InvalidOperationException` when the change is not in the table or the machine is not started; `CanChangeState<TState>()` checks first. A state that was never added throws `KeyNotFoundException`.
- `State<TOwner>` hooks: `OnEnter`, `OnUpdate(deltaTime)`, `OnExit`. A state reads `Owner` and `Machine`, and can call `ChangeState<TState>()`. Override the hooks as `protected override` from your own assembly.
- The owner drives it with `Update(deltaTime)` (from `Update()` or `IUpdatable.OnUpdate`); `UpdateManager` only registers MonoBehaviours.
- A `ChangeState` called from `OnExit`/`OnEnter` is queued and runs after the current change. `Stop()` exits without entering another state; call `Start` again to restart.
- Also: `CurrentState`, `PreviousState`, `IsRunning`, `IsInState<T>()`, `GetState<T>()`, `HasState<T>()`, and the `StateChanged(previous, current)` event (raised after `OnEnter`). Example 7 shows a menu -> play -> result flow.
