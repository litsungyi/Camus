using System;
using UnityEngine;

namespace Camus.Core
{
    public class LogHandler : ILogHandler
    {
        private ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;

        public LogHandler()
        {
            Debug.unityLogger.logHandler = this;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            defaultLogHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            defaultLogHandler.LogException(exception, context);
        }
    }
}
