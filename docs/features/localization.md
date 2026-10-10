# Localization

`Scripts/Localizables`

- `LocalizationManager.Initialize(language, IList<LocalData>)` loads string tables. `LocalData` is `{ Key, Values: Language→string }`, JSON via `LanguageDictionarySerializer`.
- The `Language` enum uses ISO-639-style codes: Zh_Tw, Zh_Cn, En, Ja, Pt, Es, Fr, De, Ko, Ru, Ar.
- `GetLocalKey(key)` returns a cached `LocalKey`. `LocalKey.Value` lazily resolves the string through `App.Instance.LocalizationManager` and caches it until the language changes, which marks all keys dirty. A missing key returns `LOCAL_KEY_NOT_FOUND: <key>`.
- `TryGetLocalString(key, out value)` looks a key up in the current language without logging or caching a `LocalKey`. Use it when the host has its own fallback (e.g. source text used as the key).
- `LocalStringUpdater` binds a uGUI `Text` to a key and refreshes on `LocaleChangedEvent`.
- `LocalKeyDrawer` (editor) draws the key string in the inspector.
- `App` creates an empty manager; the host loads data once with `App.Instance.LocalizationManager.Initialize(language, datas)` (check `Initialized` first, since App outlives scenes). `Initialize` raises `LocaleChangedEvent`, so `LocalStringUpdater`s enabled earlier refresh. Setting `CurrentLanguage` raises `LocaleChangedEvent`, and bound `LocalStringUpdater`s refresh.
