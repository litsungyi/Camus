using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class DataService
{
    private SQLiteConnection _connection;

    public DataService( string DatabaseName )
    {
        var dbPath = Path.Combine( Application.persistentDataPath, DatabaseName );
        var filePath = Path.Combine( Application.streamingAssetsPath, DatabaseName );
        if ( !File.Exists( dbPath ) )
        {
            if ( filePath.Contains( "://" ) )
            {
                // Android/WebGL: StreamingAssets is inside the archive, read it via UnityWebRequest
                using var request = UnityWebRequest.Get(filePath);
                request.SendWebRequest();
                while (!request.isDone)
                {
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new IOException($"Failed to load database {filePath}: {request.error}");
                }

                File.WriteAllBytes(dbPath, request.downloadHandler.data);
            }
            else
            {
                File.Copy( filePath, dbPath );
            }

            _connection = new SQLiteConnection( dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create );
        }
        else
        {
            _connection = new SQLiteConnection( dbPath, SQLiteOpenFlags.ReadWrite );
        }
    }
}
