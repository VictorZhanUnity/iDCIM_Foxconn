using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
using VictorDev.Parser;

public class WebAPIManager : SingletonMonoBehaviour<WebAPIManager>
{
    [Header(">>>帳密登入")]
    [SerializeField] private WebAPI_Request request_SignIn;
    [Header(">>>取得IAQ即時各項指數")]
    [SerializeField] private WebAPI_Request request_GetIAQRealTimeIndex;
    [Header(">>>取得IAQ指數歷史資料")]
    [SerializeField] private WebAPI_Request request_GetIAQIndexHistory;

    [Header(">>>取得所有DCR機櫃及內含設備")]
    [SerializeField] private WebAPI_Request request_GetAllDCRInfo;
    [Header(">>>取得設備的COBie資訊")]
    [SerializeField] private WebAPI_Request request_GetDeviceCOBie;

    [Space(20)]

    [TextArea(1, 5)]
    [Header(">>> WebAPI 登入後取得的Token值")]
    [SerializeField] private string token;

    [Header(">>> 取得所有DCR資料時觸發")]
    public UnityEvent<string> onGetAllDCRInfo;
    [Header(">>> 取得COBie時觸發")]
    public UnityEvent<string> onGetDeviceCOBie;

    public static void SignIn(string account, string password, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        Debug.Log($">>> [帳密登入] WebAPI Call: {Instance.request_SignIn.url}");
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "grant_type", "password" },
            { "username", account },
            { "password", password },
            { "scope", "auto" },
        };

        Instance.SendRequest(data, Instance.request_SignIn, (responseCode, data) =>
        {
            Instance.token = JsonUtils.ParseJson(data)["access_token"];
            Debug.Log($"*** 登入成功!! / {account}");
            onSuccess?.Invoke(responseCode, data);
        }, onFailed);
    }

    /// <summary>
    /// 取得IAQ即時各項指數
    /// </summary>
    public static void GetIAQRealTimeIndex(List<string> tags, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        Debug.Log($">>> [取得IAQ即時各項指數] WebAPI Call: {Instance.request_GetIAQRealTimeIndex.url}");
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "tags", tags.ToArray() },
        };
        Instance.SendRequest(data, Instance.request_GetIAQRealTimeIndex, onSuccess, onFailed);
    }
    /// <summary>
    /// 取得IAQ各項指數歷史資料
    /// </summary>
    public static void GetIAQIndexHistory(List<string> tags, DateTime from, DateTime to, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        Debug.Log($">>> [取得IAQ各項指數歷史資料] WebAPI Call: {Instance.request_GetIAQIndexHistory.url}");
        Debug.Log($">>> from: {from.ToString(DateTimeFormatter.FullDateTimeFormatWithT)} / to: {to.ToString(DateTimeFormatter.FullDateTimeFormatWithT)}");
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "tags", tags.ToArray() },
            { "from", from.ToString(DateTimeFormatter.FullDateTimeFormatWithT)},
            { "to", to.ToString(DateTimeFormatter.FullDateTimeFormatWithT)},
            { "option", new {
                isNumeric = true,
                 isArray= false,
                 interval= 60,//間隔60分鐘
                 unit= "min",
                 algorithm= "avg",
                 isFill= false,
                 fillMethod= "last"
            }},
        };
        Instance.SendRequest(data, Instance.request_GetIAQIndexHistory, onSuccess, onFailed);
    }

    private void SendRequest<T>(Dictionary<string, T> data, WebAPI_Request request, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        request.token = token;
        request.SetRawJsonData(PrintJSONFormatting(JsonConvert.SerializeObject(data)));
        WebAPI_Caller.SendRequest(request, (responseCode, jsonString) =>
        {
            onSuccess?.Invoke(responseCode, jsonString);
            Debug.Log($"*** WebAPI呼叫成功!!");
            PrintJSONFormatting(jsonString);
        }, onFailed);
    }

    /// <summary>
    /// 按照JSON格式列印出JSON資料
    /// </summary>
    private static string PrintJSONFormatting(string jsonString)
    {
        string result = null;
        try
        {
            JToken token = JToken.Parse(jsonString);

            // 嘗試解析為 JArray（陣列）
            if (token is JArray)
            {
                // 如果是 JArray，進行格式化並列印
                JArray jsonArray = (JArray)token;
                result = jsonArray.ToString(Formatting.Indented);
            }
            else if (token is JObject)
            {
                // 如果是 JObject，進行格式化並列印
                JObject jsonObject = (JObject)token;
                result = jsonObject.ToString(Formatting.Indented);
            }
            Debug.Log($">>> JSON資料:\n{result}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("JSON 格式錯誤: " + e.Message);
        }
        return result;
    }

    ///====================================

    [ContextMenu(" - 取得所有DCR機櫃及內含設備")]
    public void GetAllDCRInfo(Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        if (token == null)
        {
            Debug.LogWarning($"尚未事先取得Token!!");
            return;
        }

        request_GetAllDCRInfo.token = token;
        Debug.Log($">>> [取得所有DCR機櫃及內含設備] WebAPI Call: {request_GetAllDCRInfo.url}");
        WebAPI_Caller.SendRequest(request_GetAllDCRInfo, onSuccess, onFailed);
    }

    [ContextMenu(" - 取得設備的COBie資訊")]
    public static void GetCOBieByDeviceId(string deviceId, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        if (Instance.token == null)
        {
            Debug.LogWarning($"尚未事先取得Token!!");
            return;
        }

        Instance.request_GetDeviceCOBie.token = Instance.token;
        Instance.request_GetDeviceCOBie.SetFormData(new Dictionary<string, string>() { { "deviceId", deviceId } });

        Debug.Log($">>> [取得設備的COBie資訊] WebAPI Call: {Instance.request_GetDeviceCOBie.url} / deviceId: {deviceId}");
        WebAPI_Caller.SendRequest(Instance.request_GetDeviceCOBie, onSuccess, onFailed);
    }



    [ContextMenu(" - 管理員登入")]
    public void Test_SignIn()
    {
        SignIn("TCIT", "TCIT", null, null);
    }

    [ContextMenu(" - 取得IAQ即時各項指數")]
    public void Test_GetIAQRealTimeIndex()
    {
        List<string> tags = new List<string>()
        {
            "2132N0FF0238/CO",
        "2132N0FF0238/CO2",
        "2132N0FF0238/Formaldehyde",
        "2132N0FF0238/IAQ",
        "2132N0FF0238/Lit",
        "2132N0FF0238/Ozone",
        "2132N0FF0238/PM10",
        "2132N0FF0238/PM2.5",
        "2132N0FF0238/RF",
        "2132N0FF0238/RH",
        "2132N0FF0238/RT",
        "2132N0FF0238/RTO",
        "2132N0FF0238/VOCs",
        "2132N0FF0239/CO",
        "2132N0FF0239/CO2",
        "2132N0FF0239/Formaldehyde",
        "2132N0FF0239/IAQ",
        "2132N0FF0239/Lit",
        "2132N0FF0239/Ozone",
        "2132N0FF0239/PM10",
        "2132N0FF0239/PM2.5",
        "2132N0FF0239/RF",
        "2132N0FF0239/RH",
        "2132N0FF0239/RT",
        "2132N0FF0239/RTO",
        "2132N0FF0239/VOCs"
        };
        GetIAQRealTimeIndex(tags, null, null);
    }


    [ContextMenu(" - 取得IAQ各項指數歷史資料")]
    public void Test_GetIAQIndexHistory()
    {
        List<string> tags = new List<string>()
        {
            "2132N0FF0238/CO",
        "2132N0FF0238/CO2",
        "2132N0FF0238/Formaldehyde",
        "2132N0FF0238/IAQ",
        "2132N0FF0238/Lit",
        "2132N0FF0238/Ozone",
        "2132N0FF0238/PM10",
        "2132N0FF0238/PM2.5",
        "2132N0FF0238/RF",
        "2132N0FF0238/RH",
        "2132N0FF0238/RT",
        "2132N0FF0238/RTO",
        "2132N0FF0238/VOCs",
        "2132N0FF0239/CO",
        "2132N0FF0239/CO2",
        "2132N0FF0239/Formaldehyde",
        "2132N0FF0239/IAQ",
        "2132N0FF0239/Lit",
        "2132N0FF0239/Ozone",
        "2132N0FF0239/PM10",
        "2132N0FF0239/PM2.5",
        "2132N0FF0239/RF",
        "2132N0FF0239/RH",
        "2132N0FF0239/RT",
        "2132N0FF0239/RTO",
        "2132N0FF0239/VOCs"
        };
        DateTime to = DateTime.Today;
        DateTime from = to.AddMonths(-1);
        GetIAQIndexHistory(tags, from, to, null, null);
    }
}
