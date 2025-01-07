using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    [Header(">>> [Receiver] - 接收器 - IAlarmHistoryDataReceiver")]
    [SerializeField] private List<MonoBehaviour> receivers;

    [Header(">>> [資料項] - 歷史資料")]
    [SerializeField] private List<Data_AlarmHistoryData> datas;

    [Header(">>> [WebAPI] - 查詢歷史告警記錄")]
    [SerializeField] private WebAPI_Request request;

    /// <summary>
    /// 依年份取得告警歷史記錄
    /// </summary>
    [ContextMenu("- 依年份取得告警歷史記錄")]
    public void GetAlarmRecordOfYears()
    {
        if (isGenerateDemoData) GetComponent<DemoDataHandler>().InvokeJsonData();
        else
        {
            WebAPI_LoginManager.CheckToken(request);
            WebAPI_Caller.SendRequest(request, onSuccessHandler, null);
            void onSuccessHandler(long responseCode, string jsonData) => ParseJson(jsonData);
        }
    }

    /// <summary>
    /// 解析JSON
    /// </summary>
    public void ParseJson(string jsonData)
    {
        datas = JsonConvert.DeserializeObject<List<Data_AlarmHistoryData>>(jsonData);
        InovkeData();
    }

    /// <summary>
    /// 發送資料
    /// </summary>
    private void InovkeData()
    {
        Debug.Log($">>> AlarmHistoryDataManager: 發送資料 to Receives...");
        receivers.OfType<IAlarmHistoryDataReceiver>().ToList().ForEach(receiver => receiver.ReceiveData(datas));
    }

    /// <summary>
    /// [接收器] 
    /// </summary>
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
        public List<Alarm> alarms;
    }
}
