using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
using Debug = VictorDev.Common.Debug;

public class WebAPI_LoginManager : SingletonMonoBehaviour<WebAPI_LoginManager>
{
    [Header(">>> [Demo] - 是否強制登入? (token不為空即代表登入)")]
    [SerializeField] private bool isForceLogin = false;

    [Header(">>> [資料項]")]
    [SerializeField] private Data_LoginInfo loginInfo;
    public static Data_LoginInfo LoginInfo => Instance.loginInfo;

    [Header(">>> [WebAPI] - 帳密登入")]
    [SerializeField] private WebAPI_Request request_SignIn;

    private static string _account, _password;
    
    public static void UpdateToken(string accessToken, string refreshToken)
    {
        Instance.loginInfo.access_token = accessToken;
        Instance.loginInfo.refresh_token = refreshToken;
        Debug.Log("User Token 更新完成!");
    }
    
    public static void SignIn(string account, string password, Action<long, Data_LoginInfo> onSuccess, Action<long, string> onFailed)
    {
        _account = account;
        _password = password;
        
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
            Parse(jsonString);
            Debug.Log($"*** 登入成功!! / {account}");
            onSuccess?.Invoke(responseCode, Instance.loginInfo);
        }

#if UNITY_EDITOR
        //當Editor模式下，且Token不為空值時
        if (Instance.isForceLogin && string.IsNullOrEmpty(Instance.loginInfo.access_token) == false)
        {
            Debug.Log($"*** 登入成功!! / {account}");
            onSuccess?.Invoke(200, Instance.loginInfo);
        }
        else
        {
            WebAPI_Caller.SendRequest(Instance.request_SignIn, onSuccessHandler, onFailed);
        }
#else
        WebAPI_Caller.SendRequest(Instance.request_SignIn, onSuccessHandler, onFailed);
#endif
    }

    public static void Parse(string jsonString)
    {
        Instance.loginInfo = JsonConvert.DeserializeObject<Data_LoginInfo>(jsonString);
        Instance.loginInfo.account = _account;
        Instance.loginInfo.password = _password;
    }

    /// <summary>
    /// 檢查與設置Token
    /// </summary>
    public static bool CheckToken(WebAPI_Request request)
    {
        if (string.IsNullOrEmpty(LoginInfo.access_token))
        {
            Debug.Log($">>> [WebAPI] - 尚未登入取得Token!!");
            //Instance.Test_SignIn();
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
