namespace VictorDev.Common
{
    public static class Debug
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#endif
        }
    }
}
