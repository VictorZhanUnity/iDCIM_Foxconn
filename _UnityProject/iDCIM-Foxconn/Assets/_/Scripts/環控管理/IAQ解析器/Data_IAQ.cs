using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// 資料項 - IAQ
/// </summary>
[Serializable]
public class Data_IAQ : Data_NoSQL
{
    public struct IAQ_DateFormat
    {
        public string columnName { get; set; }
        /// <summary>
        /// 欄位中文名稱
        /// </summary>
        public string columnName_ZH { get; set; }
        /// <summary>
        /// 單位名稱
        /// </summary>
        public string unitName { get; set; }

        public float minValue, maxValue;
        public IAQ_DateFormat(string columnName, string columnName_ZH, string unitName, float minValue, float maxValue)
        {
            this.columnName = columnName;
            this.columnName_ZH = columnName_ZH;
            this.unitName = unitName;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }

    /// <summary>
    /// [格式] 各項IAQ指數之格式，用於圖表、文字
    /// </summary>
    public static readonly Dictionary<string, IAQ_DateFormat> UnitName = new Dictionary<string, IAQ_DateFormat>()
    {
        {"RT", new IAQ_DateFormat("RT", "溫度", "°c", 0, 60)},
        {"RH", new IAQ_DateFormat("RH", "濕度", "%", 0, 100)},
        {"CO2", new IAQ_DateFormat("CO2", "二氧化碳濃度", "ppm", 0, 5000)},
        {"CO", new IAQ_DateFormat("CO", "一氧化碳濃度", "ppm", 0, 5000)},
        {"PM2.5", new IAQ_DateFormat("PM2.5", "懸浮微粒PM2.5濃度", "ug/m3", 0, 500)},
        {"PM10", new IAQ_DateFormat("PM10", "懸浮微粒PM10濃度", "ug/m3", 0, 500)},
        {"VOCs", new IAQ_DateFormat("VOCs", "揮發性有機物濃度", "ppb", 0, 700)},
        {"Formaldehyde", new IAQ_DateFormat("Formaldehyde", "甲醛濃度", "ppb", 0, 700)},
        {"Ozone", new IAQ_DateFormat("Ozone", "臭氧濃度", "ppb", 0, 700)},
        {"Lit", new IAQ_DateFormat("Lit", "環境光", "lux", 0, 100)},
        {"IAQ", new IAQ_DateFormat("IAQ", "IAQ指數", "", 0, 400)},
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
