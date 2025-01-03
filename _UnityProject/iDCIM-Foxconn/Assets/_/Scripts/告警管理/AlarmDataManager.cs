using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using static Data_Blackbox;

/// <summary>
/// 告警管理
/// </summary>
public class AlarmDataManager : Module
{
    [Header(">>> [Receiver] - 資料接收器")]
    [SerializeField] private List<MonoBehaviour> receivers;

    [Header(">>> [資料項] - 告警記錄")]
    [SerializeField] private List<Data_AlarmRecord> datas;

    [Header(">>> [WebAPI] - 取得警告數量")]
    [SerializeField] private WebAPI_Request request;

    public override void OnInit(Action onInitComplete = null)
    {
        onInitComplete?.Invoke();
    }

    [ContextMenu("- 取得今年度警告")]
    private void GetAlarmsOfThisYear()
    {
        WebAPI_LoginManager.CheckToken(request);
        WebAPI_Caller.SendRequest(request, onSuccessHandler, null);
        void onSuccessHandler(long responseCode, string jsonData)
        {
         
        }
    }

    /// <summary>
    /// 解析JSON資料
    /// </summary>
    public void ParseJson(string jsonData)
    {
        datas = JsonConvert.DeserializeObject<List<Data_AlarmRecord>>(jsonData);

   

        //發送資料
        receivers.OfType<IAlarmReceiver>().ToList().ForEach(receiver => receiver.ReceiveData(datas));
    }


    private void OnValidate()
    {
        receivers = ObjectHandler.CheckTypoOfList<IAlarmReceiver>(receivers);
    }

    public interface IAlarmReceiver
    {
        void ReceiveData(List<Data_AlarmRecord> datas);
    }



    [Serializable]
    public class Data_AlarmRecord
    {
        public string tagName;
        public Alarm alarm;
    }
}
