using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets._Scripts.Tools
{
    public static class GameDebug
    {
        private const string Condition = "UNITY_EDITOR";

        [Conditional(Condition)]
        public static void Log(object msg)
        {
            Debug.Log(msg);
        }

        [Conditional(Condition)]
        public static void LogErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }

        [Conditional(Condition)]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
        [Conditional(Condition)]
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
        [Conditional(Condition)]
        public static void LogException(Exception exception, UnityEngine.Object context)
        {
            Debug.LogException(exception, context);
        }
        [Conditional(Condition)]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
        [Conditional(Condition)]
        public static void LogWarning(object message, UnityEngine.Object context)
        {
            Debug.LogWarning(message, context);
        }
        [Conditional(Condition)]
        public static void LogFormat(string format, params object[] args)
        {
            Debug.LogFormat(format, args);
        }
        [Conditional(Condition)]
        public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
        {
            Debug.LogFormat(context, format, args);
        }
    }
}
