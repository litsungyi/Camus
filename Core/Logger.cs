using UnityEngine;
using System;

namespace Camus.Core
{
    public static class Logger
    {
        private static readonly string EmptyTag = string.Empty;

        private static ILogHandler logHandler = new LogHandler();

        #region Log

        public statuc void Log(string message)
        {
            logHandler.LogFormat(message);
        }

        public static void Log(string tag, string message)
        {
            logHandler.Log($"[{tag}] {message}");
        }

        public static void Log(string tag, string message, UnityEngine.Object context)
        {
            logHandler.Log($"[{tag}] {message}", context);
        }

        #endregion

        #region LogWarning

        public static void LogWarning(string tag, string message)
        {
            logHandler.LogWarning($"[{tag}] {message}");
        }

        public static void ILogger.LogWarning(string tag, string message, UnityEngine.Object context)
        {
            logHandler.LogWarning($"[{tag}] {message}", context);
        }

        #endregion

        #region LogError

        public static void ILogger.LogError(string tag, string message)
        {
            logHandler.LogError($"[{tag}] {message}");
        }

        public static void ILogger.LogError(string tag, object message, UnityEngine.Object context)
        {
            logHandler.LogError($"[{tag}] {message}", context);
        }

        #endregion

        #region LogException

        public static void ILogger.LogException(Exception exception)
        {
            logHandler.LogException(exception);
        }

        public static void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
        {
            logHandler.LogException(exception, context);
        }

        #endregion
    }
}
