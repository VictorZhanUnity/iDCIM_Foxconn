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
        /// <summary>
        /// �o�ͮɶ��I
        /// </summary>
        public string alarmTime;
        public DateTime AlarmTime => DateTimeHandler.StrToLocalTime(alarmTime);
        /// <summary>
        /// �y�z
        /// </summary>
        public string alarmMessage;
        public int alarmLevel = 1;
        /// <summary>
        /// �O�_�^�_���`
        /// </summary>
        public bool isBackToNormal = false;



        public string tagName;
        public int compareOrder;
        /// <summary>
        /// �iĵ�C��
        /// </summary>
        public string alarmColor;
    }
}
