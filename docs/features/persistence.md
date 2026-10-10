# Persistence

`Scripts/Persistance/Sqlite`, spelled this way in the repo

- `DataService(dbName)` copies `StreamingAssets/<db>` to `persistentDataPath` on first run (using a blocking `UnityWebRequest` for `://` paths such as Android) and opens a `SQLiteConnection`. It is a stub: there are no query methods yet, and the connection is private. It lives in the global namespace.
