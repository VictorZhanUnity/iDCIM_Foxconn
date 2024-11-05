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
    [Header(">>> 帳密登入")]
    [SerializeField] private WebAPI_Request request_SignIn;
    [Header(">>> [IAQ] 取得IAQ即時各項指數")]
    [SerializeField] private WebAPI_Request request_GetIAQRealTimeIndex;
    [Header(">>> [IAQ] 取得IAQ指數歷史資料")]
    [SerializeField] private WebAPI_Request request_GetIAQIndexHistory;

    [Header(">>> [資產管理] 取得所有DCR機櫃及內含設備")]
    [SerializeField] private WebAPI_Request request_GetAllDCRContainer;
    [Header(">>> [資產管理] 取得設備的基本資訊與COBie")]
    [SerializeField] private WebAPI_Request request_GetDeviceInfo;

    [Space(20)]

    [TextArea(1, 5)]
    [Header(">>> WebAPI 登入後取得的Token值")]
    [SerializeField] private string token;

    [Header(">>> 取得所有DCR資料時觸發")]
    public UnityEvent<string> onGetAllDCRInfo;
    [Header(">>> 取得COBie時觸發")]
    public UnityEvent<string> onGetDeviceCOBie;

    /// <summary>
    /// 管理者登入
    /// </summary>
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
    /// [環境控制] 取得IAQ即時各項指數
    /// </summary>
    public static void GetIAQRealTimeIndex(List<string> tags, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        if (Instance.CheckToken(Instance.request_GetIAQRealTimeIndex) == false) return;

        Debug.Log($">>> [取得IAQ即時各項指數] WebAPI Call: {Instance.request_GetIAQRealTimeIndex.url}");
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "tags", tags.ToArray() },
        };
        Instance.SendRequest(data, Instance.request_GetIAQRealTimeIndex, onSuccess, onFailed);
    }
    /// <summary>
    /// [環境控制] 取得IAQ各項指數歷史資料
    /// </summary>
    public static void GetIAQIndexHistory(List<string> tags, DateTime from, DateTime to, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        if (Instance.CheckToken(Instance.request_GetIAQIndexHistory) == false) return;

     /*   Debug.Log($">>> [取得IAQ各項指數歷史資料] WebAPI Call: {Instance.request_GetIAQIndexHistory.url}");
        Debug.Log($">>> from: {from.ToString(DateTimeHandler.FullDateTimeFormatWithT)} / to: {to.ToString(DateTimeHandler.FullDateTimeFormatWithT)}");*/
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "tags", tags.ToArray() },
            { "from", from.ToString(DateTimeHandler.FullDateTimeFormatWithT)},
            { "to", to.ToString(DateTimeHandler.FullDateTimeFormatWithT)},
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

    /// <summary>
    /// [資產管理] 取得所有DCR機櫃及內含設備
    /// </summary>
    public static void GetAllDCRContainer(Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        if (Instance.CheckToken(Instance.request_GetAllDCRContainer) == false) return;

        Debug.Log($">>> [取得所有DCR機櫃及內含設備] WebAPI Call: {Instance.request_GetAllDCRContainer.url}");
        WebAPI_Caller.SendRequest(Instance.request_GetAllDCRContainer, onSuccess, onFailed);
    }

    /// <summary>
    /// [資產管理] 取得設備的COBie資訊，依據設備自身的containerID 
    /// <para>+ 需先抓所有機櫃與設備清單以取得各台設備的containerID</para>
    /// </summary>
    public static void GetCOBieByContainerID(string containerID, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        if (Instance.CheckToken(Instance.request_GetDeviceInfo) == false) return;

        Instance.request_GetDeviceInfo.SetTextAfterURL(containerID);
        Debug.Log($">>> [取得設備的COBie資訊] WebAPI Call: {Instance.request_GetDeviceInfo.url} \n containerID: {containerID}");
        WebAPI_Caller.SendRequest(Instance.request_GetDeviceInfo, onSuccess, onFailed);
    }

    /// <summary>
    /// 傳送資料項 {欄位名, 值(型態)}
    /// </summary>
    private void SendRequest<T>(Dictionary<string, T> data, WebAPI_Request request, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        //PrintJSONFormatting(JsonConvert.SerializeObject(data));
        request.SetRawJsonData(JsonConvert.SerializeObject(data));
        WebAPI_Caller.SendRequest(request, (responseCode, jsonString) =>
        {
            onSuccess?.Invoke(responseCode, jsonString);
           // Debug.Log($"*** WebAPI呼叫成功!!");
            //PrintJSONFormatting(jsonString);
        }, onFailed);
    }

    /// <summary>
    /// 檢查是否已取得Token
    /// </summary>
    private bool CheckToken(WebAPI_Request request)
    {
        if (Instance.token == null)
        {
            Debug.LogWarning($"尚未事先取得Token!!");
            return false;
        }
        request.token = token;
        return true;
    }

    /// <summary>
    /// 按照JSON格式列印出JSON資料
    /// </summary>
    public static string PrintJSONFormatting(string jsonString)
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
            Debug.Log($"---> JSON資料:\n{result}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("JSON 格式錯誤: " + e.Message);
        }
        return result;
    }

    #region [ContextMenu 測試API]
    [ContextMenu("[帳號] 管理員登入")]
    private void Test_SignIn() => SignIn("TCIT", "TCIT", null, null);

    [ContextMenu("[IAQ] 取得IAQ即時各項指數")]
    private void Test_GetIAQRealTimeIndex()
    {
        List<string> tags = new List<string>()
        {
           "FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+4/RT",
            "FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+4/RH",
            "FIT+TPE+DC+03F+1+WE+GarrisonJP 溫度感應偵測器: GarrisonJP 溫度感應偵測器+1/Smoke",
            "FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+5/RT",
            "FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+5/RH",
            "FIT+TPE+DC+03F+1+WE+GarrisonJP 溫度感應偵測器: GarrisonJP 溫度感應偵測器+2/Smoke",
            "FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+6/RT",
            "FIT+TPE+DC+03F+1+WE+co2_溫濕度三合一感測器(天花): co2_溫濕度三合一感測器(天花)+6/RH",
            "FIT+TPE+DC+03F+1+WE+GarrisonJP 溫度感應偵測器: GarrisonJP 溫度感應偵測器+3/Smoke"
        };
        GetIAQRealTimeIndex(tags, null, null);
    }

    [ContextMenu("[IAQ] 取得IAQ各項指數歷史資料")]
    private void Test_GetIAQIndexHistory()
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

    [ContextMenu("[資產管理] 取得所有DCR機櫃及內含設備")]
    private void GetAllDCRInfo()
    {
        void onSuccess(long responseCode, string jsonString)
        {
            PrintJSONFormatting(jsonString);
        }
        GetAllDCRContainer(onSuccess, null);
    }

    [ContextMenu("[資產管理] 取得設備基本資訊與COBie")]
    private void GetCOBieByContainerID()
    {
        void onSuccess(long responseCode, string jsonString)
        {
            PrintJSONFormatting(jsonString);
        }
        string containerID = "b8ef823e-4e1f-49c4-9f44-e37e8c0548d1";
        GetCOBieByContainerID(containerID, onSuccess, null);
    }
    #endregion
}
