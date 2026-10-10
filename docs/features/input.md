# Input

`Scripts/InputSystems`, namespace `Camus.Inputs`

- `InputManager` is an `IUpdatable` on App, accessed through `App.Instance.InputManager`. It uses the **Input System package** (`com.unity.inputsystem`; `Camus.asmdef` references `Unity.InputSystem`).
- Each frame it raises the following through `EventDispatcher`:
  - Mouse buttons (Primary/Secondary/Middle, from `Mouse.current`) and touches (`Button.TouchBaseIndex + i`, from `Touchscreen.current.touches`): `BeginPressEvent`, then either `PressEvent` (shorter than `LongPressThreshold`, default 0.5 s) or `LongPressEvent`, then `EndPressEvent`.
  - `MovingEvent` (position, direction, distance): every frame for the mouse, and when a pressed touch moves.
  - `InputEvent`: `ReadValue<float>()` of every registered `InputAction`, keyed by name. Helper methods: `IsHigh`, `IsLow`, `IsPositive`, `IsNegative`, `GetValue`.
- Predefined actions keep the legacy Input Manager axis names and default bindings (keyboard, mouse, gamepad): Horizontal, Vertical, Fire1–3, Jump, Mouse X/Y, Mouse ScrollWheel, Submit, Cancel. They are registered by default. `RegisterInput(key)` / `UnregisterInput(key)` toggle a predefined one; `RegisterInput(key, InputAction)` adds your own (the manager enables it while registered, and you own it).
- `InputManager` registers itself with `App.Updater` when it is first accessed through `App.Instance.InputManager`. Events start on the next tick after that first access.
- In Player Settings, **Active Input Handling** must be "Input System Package (New)" or "Both". uGUI scenes that still use `StandaloneInputModule` need "Both", or switch their EventSystem to `InputSystemUIInputModule`.
