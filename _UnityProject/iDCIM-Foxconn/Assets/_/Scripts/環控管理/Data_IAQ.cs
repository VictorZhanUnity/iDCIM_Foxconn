using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 資料項 - IAQ
/// </summary>
public class Data_IAQ : MonoBehaviour
{
    private Dictionary<string, string> dataDict;

    public void SetDictionaryData(Dictionary<string, string> dataDict)
    {
        this.dataDict = dataDict;
        foreach (IAQLevel item in iaqLevels)
        {
            if (item.CheckLevel(IAQ))
            {
                IAQStatus = item.status;
                IAQColor = item.color;
            }
        }
    }

    public int IAQ => (int)GetValue("IAQ");
    public string IAQStatus;
    public Color IAQColor;
    public float RT => (float)GetValue("RT");
    public float RH => (float)GetValue("RH");
    public int CO2 => (int)GetValue("CO2");
    public int CO => (int)GetValue("CO");
    public int PM2_5 => (int)GetValue("PM2.5");
    public int PM10 => (int)GetValue("PM10");
    public int TVOC => (int)GetValue("VOCs");
    public int HCHO => (int)GetValue("Formaldehyde");
    public int O3 => (int)GetValue("Ozone");
    public int Lux => (int)GetValue("Lit");

    private object GetValue(string key) => (dataDict.ContainsKey(key)) ? dataDict[key] : "";

    /// <summary>
    /// IAQ等級區別
    /// </summary>
    private readonly List<IAQLevel> iaqLevels = new List<IAQLevel>()
    {
        new IAQLevel(0, 50, "Excellent", 0x02e502),
        new IAQLevel(51, 100, "Good", 0x90d151),
        new IAQLevel(101, 150, "Lightly polluted", 0xfeff03),
        new IAQLevel(151, 200, "Moderately polluted", 0xf77f05),
        new IAQLevel(201, 250, "Heavily polluted", 0xfd0100),
        new IAQLevel(251, 350, "Severely polluted", 0x98004b),
        new IAQLevel(351, 400, "Extremely polluted", 0x663200),
    };

    [Serializable]
    private struct IAQLevel
    {
        public int minThreshold;
        public int maxThreshold;
        public string status;
        public Color color => ColorHandler.HexToColor(hexColor);

        private int hexColor;
        public IAQLevel(int minThreshold, int maxThreshold, string status, int hexColor) : this()
        {
            this.minThreshold = minThreshold;
            this.maxThreshold = maxThreshold;
            this.status = status;
            this.hexColor = hexColor;
        }

        /// <summary>
        /// 是否屬於此等級
        /// </summary>
        public bool CheckLevel(int iaqValue)
            => iaqValue >= minThreshold && iaqValue <= maxThreshold;
    }
}
