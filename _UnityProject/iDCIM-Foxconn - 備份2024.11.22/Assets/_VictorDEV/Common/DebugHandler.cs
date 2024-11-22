using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VictorDev.Common
{
    /// <summary>
    /// Debug Log管理器
    /// </summary> 
    public class DebugHandler : SingletonMonoBehaviour<DebugHandler>
    {
        [SerializeField] private bool isDebugEnabled = true;
        public static bool isDebugMode => Instance.isDebugEnabled;
        private void Awake()
        {
            Debug.Log($">>> Debug Mode: {(Instance.isDebugEnabled ? "ON" : "OFF")}");
        }

        /// <summary>
        /// 進行Log時發送事件
        /// </summary>
        public static Action<EnumLogType, string> onLogEvent;

        /// <summary>
        /// Log歷史記錄
        /// </summary>
        public static List<LogData> logHistory { get; private set; } = new List<LogData>();

        public static void ClearLogHistory() => logHistory.Clear();
        public static void Log(string msg) => ToLog(EnumLogType.Log, msg);
        public static void LogWarning(string msg) => ToLog(EnumLogType.LogWarning, msg);
        public static void LogError(string msg) => ToLog(EnumLogType.LogError, msg);
        private static void ToLog(EnumLogType logType, string msg)
        {
            if (isDebugMode == false) return;

            DateTime now = DateTime.Now;
            msg = $"[{now.ToString("HH:mm:ss")}] {msg} ";
            switch (logType)
            {
                case EnumLogType.Log: Debug.Log(msg); break;
                case EnumLogType.LogWarning: Debug.LogWarning(msg); break;
                case EnumLogType.LogError: Debug.LogError(msg); break;
            }
            RecordHistory(new LogData(logType, now, msg));

        }

        /// <summary>
        /// 記錄Log訊息
        /// </summary>
        private static void RecordHistory(LogData logData)
        {
            // [發送事件]
            onLogEvent?.Invoke(logData.logType, logData.msg);
            logHistory.Add(logData);
        }
#if UNITY_EDITOR
        public static void ClearConsole()
        {
            // 使用反射取得 UnityEditor.LogEntries 類型
            var logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            if (logEntries != null)
            {
                // 使用反射調用 Clear 方法
                var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
                clearMethod?.Invoke(null, null);
            }
            else
            {
                UnityEngine.Debug.LogWarning("無法取得 UnityEditor.LogEntries 類型。");
            }
        }
#endif
    }
    public enum EnumLogType
    {
        Log, LogWarning, LogError
    }

    public class LogData
    {
        public EnumLogType logType { get; set; }
        public DateTime time { get; set; }
        public string msg { get; set; }

        public LogData(EnumLogType logType, DateTime time, string msg)
        {
            this.logType = logType;
            this.time = time;
            this.msg = msg;
        }
    }
}
