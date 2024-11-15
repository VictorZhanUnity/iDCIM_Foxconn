using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Net.WebAPI;

public class WebAPI_LoginManager : SingletonMonoBehaviour<WebAPI_LoginManager>
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_LoginInfo loginInfo;
    public static Data_LoginInfo LoginInfo => Instance.loginInfo;

    [Header(">>> [WebAPI] - 帳密登入")]
    [SerializeField] private WebAPI_Request request_SignIn;

    public static void SignIn(string account, string password, Action<long, Data_LoginInfo> onSuccess, Action<long, string> onFailed)
    {
        Debug.Log($">>> [帳密登入] WebAPI Call: {Instance.request_SignIn.url}");
        Dictionary<string, string> sendData = new Dictionary<string, string>()
        {
            { "grant_type", "password" },
            { "username", account },
            { "password", password },
            { "scope", "auto" },
        };
        Instance.request_SignIn.SetRawJsonData(JsonConvert.SerializeObject(sendData));

        void onSuccessHandler(long responseCode, string jsonString)
        {
            Instance.loginInfo = JsonConvert.DeserializeObject<Data_LoginInfo>(jsonString);
            Instance.loginInfo.account = account;
            Instance.loginInfo.password = password;

            Debug.Log($"*** 登入成功!! / {account}");
            onSuccess?.Invoke(responseCode, Instance.loginInfo);
        }
        WebAPI_Caller.SendRequest(Instance.request_SignIn, onSuccessHandler, onFailed);
    }

    /// <summary>
    /// 檢查與設置Token
    /// </summary>
    public static bool CheckToken(WebAPI_Request request)
    {
        if (string.IsNullOrEmpty(LoginInfo.access_token))
        {
            Debug.Log($">>> [WebAPI] - 尚未登入取得Token!!");
            return false;
        }
        request.token = LoginInfo.access_token;
        return true;
    }

    [ContextMenu(" - 帳號登入TCIT")]
    private void Test_SignIn() => SignIn("TCIT", "TCIT", null, WebAPI_Caller.WebAPI_OnFailed);

    [Serializable]
    public class Data_LoginInfo
    {
        public string account;
        public string password;
        [Space(10)]
        public string token_type;
        public string refresh_token;
        public int expire_in;
        [TextArea(1, 50)]
        [Header(">>> WebAPI 登入後取得的Token值")]
        public string access_token;
    }
}
