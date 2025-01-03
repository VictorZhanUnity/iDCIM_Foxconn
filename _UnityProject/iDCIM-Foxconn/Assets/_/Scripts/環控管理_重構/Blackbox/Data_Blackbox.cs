using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class Data_Blackbox 
{
    public string tagName;
    public float? value { get; set; }
    public Alarm alarm;

    [Serializable]
    public class Alarm
    {
        /// <summary>
        /// 發生時間點
        /// </summary>
        public string alarmTime;
        /// <summary>
        /// 描述
        /// </summary>
        public string alarmMessage;
        public int alarmLevel;
        /// <summary>
        /// 是否回復正常
        /// </summary>
        public bool isBackToNormal = false;

        public string tagName;
        public int compareOrder;
        /// <summary>
        /// 告警顏色
        /// </summary>
        public string alarmColor;
    }
}
