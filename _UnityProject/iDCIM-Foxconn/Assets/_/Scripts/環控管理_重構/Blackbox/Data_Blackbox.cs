using Newtonsoft.Json;
using System;
using _VictorDEV.DateTimeUtils;
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
        /// 發生時間
        public string alarmTime;
        public DateTime AlarmTime => DateTimeHandler.StrToLocalTime(alarmTime);
        /// 描述
        public string alarmMessage;
        /// 是否恢復正常
        public bool isBackToNormal = false;

        public string tagName;
        public int compareOrder;
        public string alarmColor;
    }
}
