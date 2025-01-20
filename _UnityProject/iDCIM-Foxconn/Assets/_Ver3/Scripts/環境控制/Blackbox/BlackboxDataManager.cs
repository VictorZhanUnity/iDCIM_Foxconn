using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Managers;
using Debug = VictorDev.Common.Debug;

/// 環控資料管理器
public class BlackboxDataManager : Module
{
    [Header(">>> 接收到資料發送給各對像組件")]
    [SerializeField] private List<BlackboxDataReceiver> receivers;

    [Header(">>> 接收到資料時Invoke")]
    public UnityEvent<List<Data_Blackbox>> onGetBlockboxData = new UnityEvent<List<Data_Blackbox>>();

    [Header(">>> [資料項] - 目前環控資料")]
    [SerializeField] private List<Data_Blackbox> datas;


    [Header(">>> Tag名稱列表")] [SerializeField]
    private List<string> tagNames = new List<string>();

    [Header(">>> 每隔幾秒讀取一次WebAPI")]
    [SerializeField] private int internvalSec = 10;

    [Header(">>> [Event] 更新時間")]
    public UnityEvent<string> onUpdateTimeEvent = new UnityEvent<string>();

    private WaitForSeconds _waitForSeconds;
    
    #region [>>> Initialize]
    private Coroutine coroutine { get; set; }
    public override void OnInit(Action onInitComplete = null)
    {
        IEnumerator GetData_Coroutine()
        { 
            while (true)
            {
                GetIAQRealtimeData();
                yield return _waitForSeconds;
            }
        }
        _waitForSeconds ??= new WaitForSeconds(internvalSec);
        coroutine = StartCoroutine(GetData_Coroutine());
        Debug.Log(">>> BlackboxDataManager OnInit");
        onInitComplete?.Invoke();
    }
    #endregion

    [ContextMenu("- 取得即時環控資料")]
    public void GetIAQRealtimeData() => WebAPI_GetRealtimeData.GetRealtimeData(new WebAPI_GetRealtimeData.SendDataFormat(tagNames), OnRefreshDataHandler, null);
    /// 取得資料後發送給各個接收器
    private void OnRefreshDataHandler(List<Data_Blackbox> result)
    {
        datas = result;
        //發送資料
        receivers.ForEach(receiver => receiver?.ReceiveData(datas));
        onGetBlockboxData?.Invoke(datas);
        //更新時間
        DateTime updateDateTime = DateTime.Now;
        onUpdateTimeEvent?.Invoke(updateDateTime.ToString(DateTimeHandler.FullDateTimeFormat));
    }


    /// <summary>
    /// 新增Tag至列表上
    /// </summary>
    public void AddTags(List<string> tags)
    {
        tags.ForEach(tag =>
        {
            if (tagNames.Contains(tag) == false) tagNames.AddRange(tags);
        });

    }

    private void OnDestroy()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }
    #region [設定TagNames]
    [ContextMenu("- 設定Tag Names")]
    private void SetupTagNames()
    {
        tagNames.Clear();
        
        tagNames = new List<string>()
        {
            "T/H-01/RT/Value", "T/H-01/RT/Status",
            "T/H-01/RH/Value", "T/H-01/RH/Status",
            "T/H-03/RT/Value", "T/H-03/RT/Status",
            "T/H-03/RH/Value", "T/H-03/RH/Status",
            "T/H-05/RT/Value", "T/H-05/RT/Status",
            "T/H-05/RH/Value", "T/H-05/RH/Status",
            "T/H-04/Smoke/Status",
        };
        
        AddTags(Dashboard_TagName.TagName_Status);
        AddTags(Dashboard_TagName.TagName_Value);
    }

    private static class Dashboard_TagName
    {
        private static string KeyName_Value => "/Value";
        private static string KeyName_Status => "/Status";

        public static List<string> TagName_Value
        {
            get
            {
                List<string> result = new List<string>();
                List<string> tagCityPower = cityPower.Select(tag => string.Concat(tag, KeyName_Value)).ToList();
                List<string> tagUPS = ups.Select(tag => string.Concat(tag, KeyName_Value)).ToList();
                List<string> tagPDU = pdu.Select(tag => string.Concat(tag, KeyName_Value)).ToList();
                result.Add("PUE");
                result.AddRange(tagCityPower);
                result.AddRange(tagUPS);
                result.AddRange(tagPDU);
                return result;
            }
        }

        public static List<string> TagName_Status
        {
            get
            {
                List<string> result = new List<string>();
                List<string> tagStatus = statusTag.Select(tag => string.Concat(tag, KeyName_Status)).ToList();
                List<string> tagCityPower = cityPower.Select(tag => string.Concat(tag, KeyName_Status)).ToList();
                List<string> tagUPS = ups.Select(tag => string.Concat(tag, KeyName_Status)).ToList();
                tagUPS.Add("UPS/PowerSupply/Status");
                List<string> tagPDU = pdu.Select(tag => string.Concat(tag, KeyName_Status)).ToList();
                result.AddRange(tagCityPower);
                result.AddRange(tagUPS);
                result.AddRange(tagPDU);
                return result;
            }
        }

        private static List<string> statusTag = new List<string>()
        {
            "Alarm/Fire/Main",
            "Alarm/Fire/First",
            "Alarm/Fire/Secondary",
            "Alarm/Fire/Gas/Release",

            "Alarm/AirConditioner/A/Leak/1",
            "Alarm/AirConditioner/A/Leak/2",
            "Alarm/AirConditioner/B/Leak/1",
            "Alarm/AirConditioner/B/Leak/2",
            "Alarm/AirConditioner/A",
            "Alarm/AirConditioner/B",
        };

        private static List<string> cityPower = new List<string>()
        {
            "Utility/L1/Input/Voltage", "Utility/L2/Input/Voltage","Utility/L3/Input/Voltage",
            "Utility/L1/Output/Voltage", "Utility/L2/Output/Voltage","Utility/L3/Output/Voltage",
            "Utility/TotalPower",   "Utility/TotalFrequency",    "Utility/PowerFactor",
            "Utility/L1/Output/Current", "Utility/L2/Output/Current","Utility/L3/Output/Current",
        };
        private static List<string> ups = new List<string>()
        {
            "UPS/L1/Output/Voltage", "UPS/L2/Output/Voltage", "UPS/L3/Output/Voltage",
            "UPS/L1/Output/Current", "UPS/L2/Output/Current","UPS/L3/Output/Current",
            "UPS/Quantity"
        };

        private static List<string> pdu = new List<string>()
        {
            "PDU/A01-1", "PDU/A01-2", "PDU/A02-1", "PDU/A02-2",
            "PDU/B01-1", "PDU/B01-2", "PDU/B02-1", "PDU/B02-2", "PDU/B03-1", "PDU/B03-2",
        };
    }
    #endregion
}
