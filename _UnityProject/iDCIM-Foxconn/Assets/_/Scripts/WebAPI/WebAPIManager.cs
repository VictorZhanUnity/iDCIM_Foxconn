using Newtonsoft.Json;
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
    [Header(">>>取得所有DCR機櫃及內含設備")]
    [SerializeField] private WebAPI_Request request_GetAllDCRInfo;
    [Header(">>>取得設備的COBie資訊")]
    [SerializeField] private WebAPI_Request request_GetDeviceCOBie;

    [TextArea(1, 5)]
    [Header(">>> WebAPI 登入後取得的Token值")]
    [SerializeField] private string token;

    [Header(">>> 取得所有DCR資料時觸發")]
    public UnityEvent<string> onGetAllDCRInfo;
    [Header(">>> 取得COBie時觸發")]
    public UnityEvent<string> onGetDeviceCOBie;

    [ContextMenu(" - 帳密登入")]
    public static void Login(string account, string password, Action<long, string> onSuccess, Action<long, string> onFailed)
    {
        Debug.Log($">>> [帳密登入] WebAPI Call: {Instance.request_SignIn.url}");
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "grant_type", "password" },
            { "username", account },
            { "password", password },
            { "scope", "auto" },
        };
        Instance.request_SignIn.SetRawJsonData(JsonConvert.SerializeObject(data));
        WebAPI_Caller.SendRequest(Instance.request_SignIn, (responseCode, token) =>
        {
            Instance.token = JsonUtils.ParseJson(token)["access_token"];
            Debug.Log($"*** 登入成功!! / {account}");
            //Instance.GetAllDCRInfo(onSuccess, onFailed);
        }, onFailed);
    }

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
}
