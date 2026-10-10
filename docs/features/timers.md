# Timers

`Scripts/Timers`

- `TimerBase` (MonoBehaviour) holds a serialized `TimerSetting { interval, loop, maxCount }`. It calls `onTimer` every `interval` seconds and stops after `maxCount` ticks unless `loop` is set.
- Controls: `StartTimer(onTimer)`, `StartTimer(delta, onTimer)`, `RestartTimer`, `StopTimer`, `PauseTimer`, `ResumeTimer`, and `UpdateSetting`. `Status` is one of None/Started/Paused/Stopped. Pausing and stopping disable the component.
- There are six concrete, sealed variants. Choose by update phase and driver:
  - Unity message driven: `UpdateTimer`, `FixedUpdateTimer`, `LateUpdateTimer`
  - `App.Updater` driven (via `TimerUpdatableBase`, which auto-registers in `OnEnable`): `UpdatableTimer`, `FixedUpdatableTimer`, `LateUpdatableTimer`
- `OnValidate` asserts `interval > 0` and `maxCount >= 0`. `Tests/Timers/TimerTest` is an OnGUI harness for manual testing.
