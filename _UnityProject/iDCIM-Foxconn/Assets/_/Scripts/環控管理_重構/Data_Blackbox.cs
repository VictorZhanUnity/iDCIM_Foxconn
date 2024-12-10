using System;
using UnityEngine;

[Serializable]
public class Data_Blackbox : ILandmarkData
{
    public string tagName;
    public float value;
    public Alarm alarm;

    public Transform model;

    public string DevicePath => model.name.Split(".")[0];

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
