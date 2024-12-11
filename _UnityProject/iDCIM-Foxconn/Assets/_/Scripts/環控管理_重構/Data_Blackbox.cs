using System;
using UnityEngine;

[Serializable]
public class Data_Blackbox 
{
    public string tagName;
    public float value;
    public Alarm alarm;

    [Serializable]
    public class Alarm
    {
        public string tagName;
        public int compareOrder;
        public string alarmMessage;
        public int alarmLevel;
        public string alarmColor;
        public string alarmTime;
    }
}
