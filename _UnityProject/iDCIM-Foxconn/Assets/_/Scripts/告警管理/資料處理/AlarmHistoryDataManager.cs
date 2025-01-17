using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using static Data_Blackbox;
using Debug = VictorDev.Common.Debug;

/// <summary>
/// 告警歷史記錄 - 資料處理
/// </summary>
public class AlarmHistoryDataManager : Module, IJsonParseReceiver
{
    [Header(">>> [Demo] - 是否動態產生假資料")]
    public bool isGenerateDemoData = false;

    [Header(">>> [Receiver] - 傳送今年資料給接收器 - IAlarmHistoryDataReceiver")]
    [SerializeField] private List<MonoBehaviour> receivers;

    [Header(">>> [Event] - 讀取到資料後Invoke")]
    public UnityEvent<List<Data_AlarmHistoryData>> onReceiveData = new UnityEvent<List<Data_AlarmHistoryData>>();
    
    [Header(">>> [資料項] - 歷史資料")]
    private List<Data_AlarmHistoryData> datas;

    [Header(">>> [WebAPI] - 查詢歷史告警記錄")]
    [SerializeField] private WebAPI_Request request;
   
    /// 依年份取得告警歷史記錄
    [ContextMenu("- 取得今年份的告警歷史記錄")]
    public void GetAlarmRecordOfThisYear() => GetAlarmRecordOfYear(DateTime.Now.Year, (datas)=>InovkeData());

    /// 依照日期取得資料
    public void GetAlarmRecordOfYear(int year, Action<List<Data_AlarmHistoryData>> onSuccess, Action<string> onFailed = null)
    {
        if (isGenerateDemoData)
        {
            GetComponent<DemoDataHandler_AlarmHistoryData>().GenerateDemoData(year, (jsonString)=>onSuccessHandler(200, jsonString));
        }
        else
        {
            SendRawJsonFormatAlarmHistoryRecord rawJson = JsonConvert.DeserializeObject<SendRawJsonFormatAlarmHistoryRecord>(request.BodyJSON);
            rawJson.from = $"{year}-01-01T00:00:00";
            rawJson.to = $"{year+1}-01-01T00:00:00";
            request.SetRawJsonData(JsonConvert.SerializeObject(rawJson));
            Debug.Log($">>> 讀取資料時間區段: {rawJson.from} ~ {rawJson.to}");
            WebAPI_LoginManager.CheckToken(request);
            WebAPI_Caller.SendRequest(request, onSuccessHandler, null);
        }
        return;

        void onSuccessHandler(long responseCode, string jsonData)
        {
            ParseJson(jsonData);
            onSuccess.Invoke(datas);
            onReceiveData.Invoke(datas);
        }
    }
    /// 解析JSON
    public void ParseJson(string jsonData)
    {
        datas = JsonConvert.DeserializeObject<List<Data_AlarmHistoryData>>(jsonData);
    }

    /// 發送資料
    private void InovkeData()
    {
        Debug.Log($">>> AlarmHistoryDataManager: 發送今年記錄資料 to Receives...");
        receivers.OfType<IAlarmHistoryDataReceiver>().ToList().ForEach(receiver => receiver.ReceiveData(datas));
        onReceiveData?.Invoke(datas);
    }

    /// [接收器] 取得今年度資料
    public interface IAlarmHistoryDataReceiver
    {
        void ReceiveData(List<Data_AlarmHistoryData> datas);
    }

    #region [初始化]
    public override void OnInit(Action onInitComplete = null)
    {
        Debug.Log(">>> AlarmDataManager OnInit Complete.");
        onInitComplete?.Invoke();
    }
    private void OnValidate() => receivers = ObjectHandler.CheckTypoOfList<IAlarmHistoryDataReceiver>(receivers);
    #endregion

    [Serializable]
    public class Data_AlarmHistoryData
    {
        public string tagName;
        public List<Alarm> alarms = new List<Alarm>();
    }
    
    [Serializable]
    public class SendRawJsonFormatAlarmHistoryRecord
    {
        public string[] tags;
        public string from, to, level;
    }
}
