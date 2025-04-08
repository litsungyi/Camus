using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
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
                WWW www = new WWW( filePath );
                while ( !www.isDone )
                {
                }

                File.WriteAllBytes( dbPath, www.bytes );
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
    
    public DataService( string DatabaseName, string password )
        : this( DatabaseName )
    {
        _connection
    }
}