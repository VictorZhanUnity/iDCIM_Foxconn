using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using Newtonsoft.Json;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Managers;
using static AlarmHistoryDataManager;
using static Data_Blackbox;
using Debug = VictorDev.Common.Debug;
using Random = UnityEngine.Random;

/// [Demo資料處理] 告警歷史記錄
public class DemoDataHandler_AlarmHistoryData : DemoDataHandler
{
    [Header(">>> [Demo資料項] - 告警歷史記錄")] public List<Data_AlarmHistoryData> alarmList;

    public override void OnInit(Action onInitComplete = null) => onInitComplete?.Invoke();

    public override void InvokeJsonData()
    {
        jsonDataString = JsonConvert.SerializeObject(alarmList);
        base.InvokeJsonData();
    }

    #region[ContenxtMenu: 產生Demo資料項，並轉化成Json字串]

    [ContextMenu("- 產生資料")]
    private void GenerateDemoData()
    {
        Debug.Log(">>> 產生資料...");
        alarmList = new List<Data_AlarmHistoryData>();
        tagNames.ForEach(tagName =>
        {
            Data_AlarmHistoryData record = new Data_AlarmHistoryData() { tagName = tagName };
            record.alarms = new List<Alarm>();
            for (int i = 0; i < Random.Range(1, 11); i++)
            {
                //亂數決定年份
                bool isToday = Random.Range(1, 20) > 18;
                int year = Random.Range(1, 11) < 8 ? 2024 : 2025;
                Alarm data = new Alarm()
                {
                    alarmTime = DateTimeHandler.GetRandomDateTimeInYear(year, year == 2025)
                        .ToString(DateTimeHandler.Format_GlobalTime),
                    alarmMessage = alarmMessageSet.FirstOrDefault(keyPair =>
                        tagName.Contains(keyPair.Key, StringComparison.OrdinalIgnoreCase)).Value,
                };

                // 是否為今日發生
                if (isToday)
                {
                    Debug.Log($"\t今日資料: {tagName}");
                    data.alarmTime = DateTimeHandler.GetRandomDateTimeInToday().ToString(DateTimeHandler.Format_GlobalTime);
                }

                record.alarms.Add(data);
            }

            record.alarms = record.alarms.OrderBy(alarm => alarm.alarmTime).ToList();
            alarmList.Add(record);
        });
        Debug.Log(">>> 產生資料完畢.");
    }

    #endregion

    #region [Components]

    [Header(">>> 感應器標籤")] public List<string> tagNames = new List<string>()
    {
        "T/H-01/RT/Status",
        "T/H-01/RH/Status",
        "T/H-03/RT/Status",
        "T/H-03/RH/Status",
        "T/H-05/RT/Status",
        "T/H-05/RH/Status",
        "T/H-04/Smoke/Status",

        "Utility/L1/Input/Voltage/Status",
        "Utility/L1/Output/Voltage/Status",
        "Utility/L1/Output/Current/Status",
        "Utility/L2/Input/Voltage/Status",
        "Utility/L2/Output/Voltage/Status",
        "Utility/L2/Output/Current/Status",
        "Utility/L3/Input/Voltage/Status",
        "Utility/L3/Output/Voltage/Status",
        "Utility/L3/Output/Current/Status",
        "Utility/TotalPower/Status",
        "Utility/TotalFrequency/Status",
        "Utility/PowerFactor/Status",
        "UPS/TotalPower/Status",
        "UPS/L1/Output/Voltage/Status",
        "UPS/L1/Output/Current/Status",
        "UPS/L2/Output/Voltage/Status",
        "UPS/L2/Output/Current/Status",
        "UPS/L3/Output/Voltage/Status",
        "UPS/L3/Output/Current/Status",
        "UPS/Quantity/Status",
        "UPS/PowerSupply/Status",
        "PDU/A01-1/Status",
        "PDU/A01-2/Status",
        "PDU/A02-1/Status",
        "PDU/A02-2/Status",
        "PDU/B01-1/Status",
        "PDU/B01-2/Status",
        "PDU/B02-1/Status",
        "PDU/B02-2/Status",
        "PDU/B03-1/Status",
        "PDU/B03-2/Status",

        "Alarm/Fire/Main/Status",
        "Alarm/Fire/First/Status",
        "Alarm/Fire/Secondary/Status",
        "Alarm/Fire/Gas/Release/Status",
        "Alarm/AirConditioner/A/Leak/1/Status",
        "Alarm/AirConditioner/A/Leak/2/Status",
        "Alarm/AirConditioner/A/Status",
        "Alarm/AirConditioner/B/Leak/1/Status",
        "Alarm/AirConditioner/B/Leak/2/Status",
        "Alarm/AirConditioner/B/Status"
    };

    /// <summary>
    /// 對照表：{關鍵字, 告警訊息}
    /// </summary>
    private readonly Dictionary<string, string> alarmMessageSet = new Dictionary<string, string>()
    {
        { "RT", "溫度異常" },
        { "RH", "濕度異常" },
        { "Smoke", "偵測到煙霧" },
        { "Utility", "電壓異常" },
        { "UPS", "電壓異常" },
        { "PDU", "電壓異常" },
        { "Fire", "偵測到火災" },
        { "Leak", "偵測到漏水情況" },
        { "AirConditioner", "空氣品質異常" },
    };

    #endregion
}


/// <summary>
/// [抽像類別] - Demo資料處理器
/// </summary>
public abstract class DemoDataHandler : Module
{
    public bool isActivate = true;

    [Header(">>> [Receiver] - 資料接收器")] [SerializeField]
    private List<MonoBehaviour> receivers;

    protected string jsonDataString { get; set; }

    /// <summary>
    /// 發送JSON資料至各個接收器
    /// </summary>
    public virtual void InvokeJsonData()
    {
        if (isActivate)
        {
            receivers.OfType<IJsonParseReceiver>().ToList().ForEach(receiver => receiver.ParseJson(jsonDataString));
        }
    }

    private void OnValidate() => receivers = ObjectHandler.CheckTypoOfList<IJsonParseReceiver>(receivers);
}