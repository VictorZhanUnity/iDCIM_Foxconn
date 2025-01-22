using System;
using UnityEngine;

namespace VictorDev.Common
{
    public static class Debug
    {
        public static bool IsActivated { get; set; } = true;

        private static void CheckIsEditorEnviorment(Action action)
        {
            if (Application.isEditor || IsActivated)
            {
                action?.Invoke();
            }
        }

        public static void Log(object message) => 
            CheckIsEditorEnviorment(() => UnityEngine.Debug.Log(message));

        public static void LogWarning(object message) =>
            CheckIsEditorEnviorment(() => UnityEngine.Debug.LogWarning(message));

        public static void LogError(object message) =>
            CheckIsEditorEnviorment(() => UnityEngine.Debug.LogError(message));
    }
}