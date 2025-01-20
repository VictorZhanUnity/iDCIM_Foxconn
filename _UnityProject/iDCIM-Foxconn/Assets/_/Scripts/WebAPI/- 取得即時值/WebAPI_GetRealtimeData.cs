using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
using VictorDev.Parser;

public class WebAPI_GetRealtimeData : SingletonMonoBehaviour<WebAPI_GetRealtimeData>
{
    [Header("[資料項]")]
    [SerializeField] private List<Data_Blackbox> data;

    [Header("[WebAPI]")]
    [SerializeField] private WebAPI_Request request_GetRealtimeData;

    ///  [Event] - 取得環控即時資訊時Invoke
    public static UnityEvent<List<Data_Blackbox>> onGetRealtimeData { get; set; } = new UnityEvent<List<Data_Blackbox>>();


    [SerializeField]
    private List<string> sendTags = new List<string>()
    {
            "T/H-01/RT/Value", "T/H-01/RT/Status",
            "T/H-01/RH/Value", "T/H-01/RH/Status",
            "T/H-03/RT/Value", "T/H-03/RT/Status",
            "T/H-03/RH/Value", "T/H-03/RH/Status",
            "T/H-05/RT/Value", "T/H-05/RT/Status",
            "T/H-05/RH/Value", "T/H-05/RH/Status",
            "T/H-04/Smoke/Status", "PUE"
    };

    /// 取得即時環控資訊
    public static void GetRealtimeData(SendDataFormat sendData, Action<List<Data_Blackbox>> onSuccess, Action<long, string> onFailed)
    {
        Instance.request_GetRealtimeData.SetRawJsonData(JsonConvert.SerializeObject(sendData));
        WebAPI_LoginManager.CheckToken(Instance.request_GetRealtimeData);
        void onSuccessHandler(long responseCode, string jsonString)
        {
            Instance.data = JsonConvert.DeserializeObject<List<Data_Blackbox>>(jsonString);
            onSuccess?.Invoke(Instance.data);
            onGetRealtimeData?.Invoke(Instance.data);
        }
        WebAPI_Caller.SendRequest(Instance.request_GetRealtimeData, onSuccessHandler, onFailed);
    }

    [Serializable]
    public class SendDataFormat
    {
        public List<string> tags;
        public SendDataFormat(List<string> tags)
        {
            this.tags = tags;
        }
    }

    [ContextMenu("[WebAPI] - 取得即時環控資訊")]
    private void Test_GetRealtimeData()
    {
        SendDataFormat data = new SendDataFormat(sendTags);
        GetRealtimeData(data, (data) =>
        {
            JsonUtils.PrintJSONFormatting(JsonConvert.SerializeObject(data));
        }, null);
    }
}

