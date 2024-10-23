using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using Random = UnityEngine.Random;

/// <summary>
/// 資料項 - IAQ
/// </summary>
[Serializable]
public class Data_IAQ : Data_NoSQL
{
    /// <summary>
    /// [單位] IAQ代號比對
    /// </summary>
    public static readonly Dictionary<string, string> ColumnUnit = new Dictionary<string, string>()
    {
        {"RT", "°c"}, {"RH", "%"},{"CO2", "ppm"},{"CO", "ppm"},
        {"PM2.5", "ug/m3"}, {"PM10", "ug/m3"},{"VOCs", "ppb"},{"Formaldehyde", "ppb"},
        {"Ozone", "ppb"}, {"Lit", "lux"},
    };

    /// <summary>
    /// [名稱] IAQ代號比對
    /// </summary>
    public static readonly Dictionary<string, string> ColumnName = new Dictionary<string, string>()
    {
        {"RT", "溫度"}, {"RH", "濕度"},{"CO2", "二氧化碳濃度"},{"CO", "一氧化碳濃度"},
        {"PM2.5", "懸浮微粒PM2.5濃度"}, {"PM10", "懸浮微粒PM10濃度"},{"VOCs", "揮發性有機物濃度"},{"Formaldehyde", "甲醛濃度"},
        {"Ozone", "臭氧濃度"}, {"Lit", "環境光"},
    };

    public string ModelID;
    public float IAQ => float.Parse(GetValue("IAQ"));
    public float RT => float.Parse(GetValue("RT"));
    public float RH => float.Parse(GetValue("RH"));
    public float CO2 => float.Parse(GetValue("CO2"));
    public float CO => float.Parse(GetValue("CO"));
    public float PM2_5 => float.Parse(GetValue("PM2.5"));
    public float PM10 => float.Parse(GetValue("PM10"));
    public float VOCs => float.Parse(GetValue("VOCs"));
    public float HCHO => float.Parse(GetValue("Formaldehyde"));
    public float O3 => float.Parse(GetValue("Ozone"));
    public float Lux => float.Parse(GetValue("Lit"));

    /// <summary>
    /// IAQ指數等級顏色
    /// </summary>
    public Color levelColor => currentLevel.color;

    /// <summary>
    /// IAQ指數等級狀態文字
    /// </summary>
    public string levelStatus => currentLevel.status;
    private IAQLevel currentLevel => iaqLevels.Where(iaqLevel => iaqLevel.CheckLevel(IAQ)).FirstOrDefault();
    public Data_IAQ(Dictionary<string, string> sourceData) : base(sourceData) { }

    #region
    /// <summary>
    /// IAQ等級顏色區別
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
        public bool CheckLevel(float iaqValue)
            => iaqValue >= minThreshold && iaqValue <= maxThreshold;
    }
    #endregion
}
