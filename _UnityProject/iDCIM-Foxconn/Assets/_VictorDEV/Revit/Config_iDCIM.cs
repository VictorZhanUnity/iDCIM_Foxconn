using System.Collections.Generic;
using System.Linq;

namespace _VictorDEV.Revit
{
    public static class Config_iDCIM
    {
        public static string[] AlarmSystemType = new[]
        {
            "電壓", "溫度", "濕度", "煙霧", "消防", "空調", "漏水"
        };

        private static Dictionary<List<string>, string> AlarmSystemTypeDict = new Dictionary<List<string>, string>
        {
            { new List<string>() { "Utility", "UPS", "PDU" }, AlarmSystemType[0] },
            { new List<string>() { "RT" }, AlarmSystemType[1] },
            { new List<string>() { "RH" }, AlarmSystemType[2] },
            { new List<string>() { "Smoke" }, AlarmSystemType[3] },
            { new List<string>() { "Fire" }, AlarmSystemType[4] },
            { new List<string>() { "AirConditioner" }, AlarmSystemType[5] },
            { new List<string>() { "Leak" }, AlarmSystemType[6] },
        };
        
        /// 取得告警類別
        public static string GetAlarmSystem(string tagName)
            => AlarmSystemTypeDict.FirstOrDefault(keyPair => keyPair.Key.Any(keyword => tagName.Contains(keyword))).Value;
    }
}