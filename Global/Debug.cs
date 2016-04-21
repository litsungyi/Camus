// Ref. http://answers.unity3d.com/questions/126315/debuglog-in-build.html

// NOTE: Add DISABLE_LOGS to your project's define setting to disable all logs

#if DISABLE_LOGS
// NOTE: Debug.Log only shown in development build
#define DISABLE_VERBOSE_LOGS

#if !DEBUG
// NOTE: Since Unity 5.1, DEBUG is automatically defined in development build
// NOTE: LogWarning, LogError, LogException should shown in development build
#define DISABLE_CRITICAL_LOGS
#endif  // !DEBUG
#endif  // DISABLE_LOGS

//#if !UNITY_EDITOR
// NOTE: Show all logs in Unity Editor
using UnityEngine;
using System.Diagnostics;
using System.Runtime.InteropServices;

#if DISABLE_VERBOSE_LOGS
public static partial class Debug
{
#region Log
    [Conditional( "DISABLE_VERBOSE_LOGS" )] public static void Log( object message ) {}
    [Conditional( "DISABLE_VERBOSE_LOGS" )] public static void Log( object message, UnityEngine.Object context ) {}
#endregion  // Log

#region LogWarning
    [Conditional( "DISABLE_CRITICAL_LOGS" )] public static void LogWarning( object message ) {}
    [Conditional( "DISABLE_CRITICAL_LOGS" )] public static void LogWarning( object message, UnityEngine.Object context ) {}
#endregion  // LogWarning

#region LogError
    [Conditional( "DISABLE_CRITICAL_LOGS" )] public static void LogError( object message ) {}
    [Conditional( "DISABLE_CRITICAL_LOGS" )] public static void LogError( object message, UnityEngine.Object context ) {}
#endregion  // LogError

#region LogException
    [Conditional( "DISABLE_CRITICAL_LOGS" )] public static void LogException( System.Exception exception ) {}
    [Conditional( "DISABLE_CRITICAL_LOGS" )] public static void LogException( System.Exception exception, UnityEngine.Object context ) {}
#endregion  // LogException
}
#endif  // DISABLE_CRITICAL_LOGS
//#endif  // !UNITY_EDITOR
