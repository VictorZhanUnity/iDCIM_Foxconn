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

        public static void Log(object message, bool isPrintArrow = true)
        {
            CheckIsEditorEnviorment(() => UnityEngine.Debug.Log((isPrintArrow ? "" : ">>> ") + message.ToString()));
        }

        public static void LogWarning(object message, bool isPrintArrow = true) =>
            CheckIsEditorEnviorment(() => UnityEngine.Debug.LogWarning((isPrintArrow? "": ">>> ") + message));

        public static void LogError(object message, bool isPrintArrow = true) =>
            CheckIsEditorEnviorment(() => UnityEngine.Debug.LogError((isPrintArrow? "": ">>> ") + message));
    }
}