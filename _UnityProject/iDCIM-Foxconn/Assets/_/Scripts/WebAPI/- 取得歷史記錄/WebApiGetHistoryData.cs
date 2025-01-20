using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Net.WebAPI;

/// [Historian文件2.1] 查詢歷史數據
public class WebApiGetHistoryData : SingletonMonoBehaviour<WebApiGetHistoryData>
{
    [Header("[Receiver] - IReceiveTodayData")] [SerializeField]
    private List<MonoBehaviour> receivers;
    
    [Header("[WebAPI] - [Historian文件2.1] 查詢歷史數據")] [SerializeField]
    private WebAPI_Request request;

    /// 取得今日歷史資料 {回傳JsonString}
    public static void GetHistoryDataBetweenToday(string[] sendTagNames)
    {
        GetHistoryData(sendTagNames, DateTime.Today, DateTime.Today.AddDays(1), 60, 
            (datas)=> Instance.InvokeData(), null);
    }

    private void InvokeData() => receivers.OfType<IReceiveTodayData>().ToList()
        .ForEach(target => target.ReceiveTodayData(receivedData));

    /// 取得今日歷史資料 {回傳JsonString}
    public static void GetHistoryData(string[] sendTagNames, DateTime from, DateTime to, int intervalMins, Action<List<ReceiveJsonData>> onSuccess,
        Action<long, string> onFailed = null)
    {
        string rawJson = Instance.request.BodyJSON;
        SendRawJson sendData = JsonConvert.DeserializeObject<SendRawJson>(rawJson);
        sendData.from = $"{from:yyyy-MM-dd}T00:00:00";
        sendData.to = $"{to:yyyy-MM-dd}T00:00:00";
        sendData.option.interval = intervalMins;
        Instance.request.SetRawJsonData(JsonConvert.SerializeObject(sendData));

        WebAPI_LoginManager.CheckToken(Instance.request);

        void onSuccessHandler(long responseCode, string jsonString)
        {
            Instance.receivedData = JsonConvert.DeserializeObject<List<ReceiveJsonData>>(jsonString);
            onSuccess?.Invoke(Instance.receivedData);
        }

        WebAPI_Caller.SendRequest(Instance.request, onSuccessHandler, onFailed);
    }

    #region Initialize
    protected override void OnValidateAfter()
    {
        receivers = ObjectHandler.CheckTypoOfList<IReceiveTodayData>(receivers);
    }
    #endregion

    #region ContextMenu
 
    [ContextMenu("[WebAPI] - 查詢歷史數據 - 溫濕度")]
    public static void ContextMenu_GetHistoryData_RTRH()
    {
        string[] tagNames = new[]
        {
            "T/H-01/RT/Value",
            "T/H-01/RH/Value",
            "T/H-03/RT/Value",
            "T/H-03/RH/Value",
            "T/H-05/RT/Value",
            "T/H-05/RH/Value"
        };
        GetHistoryDataBetweenToday(tagNames);
    }
    #endregion

    #region Components

    [Header("[資料項] - 取得的資料")]
    public List<ReceiveJsonData> receivedData;

    public interface IReceiveTodayData
    {
        void ReceiveTodayData(List<ReceiveJsonData> receivedData);
    }
    
    [Serializable]
    public class SendRawJson
    {
        public string[] tags;
        public string from, to;
        public Option option = new Option();

        [Serializable]
        public class Option
        {
            public bool isNumeric;
            public bool isArray;

            /// 間隔時間(分)
            public int interval;

            public string unit;
            public string algorithm;
            public bool isFill;
            public string fillMethod;
        }
    }

    [Serializable]
    public class ReceiveJsonData
    {
        public string key;
        public Value[] value;
        
        public float Average => value.Select(item=>item.value).Average();

        [Serializable]
        public class Value
        {
            public string timestamp;
            public float value;
            public DateTime Timestamp;
        }
    }

    #endregion
}