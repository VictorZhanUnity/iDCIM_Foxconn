using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
using VictorDev.Parser;

/// <summary>
/// [DataSync���2.5] ���o�Y�ɭ�
/// </summary>
public class WebAPI_GetRealtimeData : SingletonMonoBehaviour<WebAPI_GetRealtimeData>
{
    [Header(">>> [��ƶ�] - �ثe��IAQ��T��")]
    [SerializeField] private List<Data_Blackbox> data;

    [Header(">>> [DataSync���2.5] ���o�Y�ɭ�")]
    [SerializeField] private WebAPI_Request request_GetRealtimeData;

    /// <summary>
    ///  [Event] - ���o�ثe��IAQ��T��Invoke
    /// </summary>
    public static UnityEvent<List<Data_Blackbox>> onGetRealtimeData { get; set; } = new UnityEvent<List<Data_Blackbox>>();


    [Header(">>> [����] - �o�e�����")]
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

    /// <summary>
    /// [DataSync���2.5] ���o�Y�ɭ�
    /// </summary>
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

    [ContextMenu("[WebAPI] - ���o�Y�ɭ�")]
    private void Test_GetRealtimeData()
    {
        SendDataFormat data = new SendDataFormat(sendTags);
        GetRealtimeData(data, (data) =>
        {
            JsonUtils.PrintJSONFormatting(JsonConvert.SerializeObject(data));
        }, null);
    }
}

