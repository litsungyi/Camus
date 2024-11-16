using System;
using UnityEngine;

namespace Camus.Core
{
    public static class Logger
    {
        private static ILogHandler logHandler = new LogHandler();

        #region Log

        public static void Log(string message, UnityEngine.Object context = null)
        {
            DoLog(LogType.Log, context, message);
        }

        public static void Log<T>(T tag, string message, UnityEngine.Object context = null) where T : struct
        {
            Log($"[{tag.ToString()}] {message}", context);
        }

        #endregion

        #region LogWarning

        public static void LogWarning(string message, UnityEngine.Object context = null)
        {
            DoLog(LogType.Warning, context, message);
        }

        public static void LogWarning<T>(T tag, string message, UnityEngine.Object context = null) where T : struct
        {
            LogWarning($"[{tag.ToString()}] {message}", context);
        }

        #endregion

        #region LogError

        public static void LogError(string message, UnityEngine.Object context = null)
        {
            DoLog(LogType.Error, context, message);
        }

        public static void LogError<T>(T tag, string message, UnityEngine.Object context = null) where T : struct
        {
            LogError($"[{tag.ToString()}] {message}", context);
        }

        #endregion

        #region LogException

        public static void LogException(Exception exception, UnityEngine.Object context = null)
        {
            logHandler.LogException(exception, context);
        }

        #endregion

        #region Detail

        private static void DoLog(LogType logType, UnityEngine.Object context, string message)
        {
            logHandler.LogFormat(logType, context, message);
        }

        #endregion
    }
}
