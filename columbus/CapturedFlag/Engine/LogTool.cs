using System;
using UnityEngine;

namespace CapturedFlag.Engine
{
    public static class LogTool
    {
        public static LogLevel logLevel = LogLevel.ALL;

        static LogTool()
        {
            #if UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            logLevel = LogLevel.WARN | LogLevel.ERROR;
            #endif
            #if UNITY_EDITOR
            logLevel = LogLevel.DEBUG | LogLevel.WARN | LogLevel.ERROR | LogLevel.FATAL;
            #endif
        }

        [Flags]
        public enum LogLevel
        {
            NONE = 0,
            DEBUG = 1,
            WARN = 2,
            ERROR = 4,
            FATAL = 8,
            VERBOSE = 16,
            ALL = 31
        }

        private static string TagMessage(string message, UnityEngine.Object context = null, LogLevel level = LogLevel.VERBOSE)
        {
            var tag = "[" + level.ToString() + "] ";
            return (context != null) ? tag + "[" + context.GetType().Name + "]" + message : tag + message;
        }

        public static void Log(string message,  UnityEngine.Object context = null, LogLevel level = LogLevel.VERBOSE)
        {
            if ((logLevel & level) == level)
            {
                var msg = TagMessage(message, context, level);
                switch (level)
                {
                    case LogLevel.DEBUG:
                        if (Debug.isDebugBuild)
                        {
                            if (context != null)
                                Debug.Log(msg, context);
                            else
                                Debug.Log(msg);
                        }                           
                        break;
                    case LogLevel.WARN:
                        if (Debug.isDebugBuild)
                        {
                            if (context != null)
                                Debug.LogWarning(msg, context);
                            else
                                Debug.LogWarning(msg);
                        }
                        break;
                    case LogLevel.ERROR:
                        if (context != null)
                            Debug.LogError(msg, context);
                        else
                            Debug.LogError(msg);
                        break;
                    case LogLevel.FATAL:
                        if (context != null)
                            Debug.LogError(msg, context);
                        else
                            Debug.LogError(msg);
                        break;
                    default:
                        if (context != null)
                            Debug.Log(msg, context);
                        else
                            Debug.Log(msg);
                        break;
                }
            }
        }

        public static void LogDebug(string message, UnityEngine.Object context = null)
        {
            Log(message, context, LogLevel.DEBUG);
        }

        public static void LogWarning(string message, UnityEngine.Object context = null)
        {
            Log(message, context, LogLevel.WARN);
        }

        public static void LogError(string message, UnityEngine.Object context = null)
        {
            Log(message, context, LogLevel.ERROR);
        }

        public static void LogFatal(string message, UnityEngine.Object context = null)
        {
            Log(message, context, LogLevel.FATAL);
        }
    }
}
