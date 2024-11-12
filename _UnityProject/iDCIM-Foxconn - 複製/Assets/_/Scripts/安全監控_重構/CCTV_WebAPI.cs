using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Net.WebAPI;

[Serializable]
public class CCTV_WebAPI
{
    [Header(">>> [資料項] CCTV所有設備資料")]
    [SerializeField] private SearchDeviceFormat data;

    [Header(">>> [配置管理] 取得所有CCTV設備資訊")]
    [SerializeField] private WebAPI_Request getAllCCTVInfo;

    /// <summary>
    /// 取得所有CCTV設備資訊
    /// </summary>
    public void GetAllCCTVInfo()
    {
        void onSuccess(long responseCode, string jsonData)
        {
            data = JsonConvert.DeserializeObject<SearchDeviceFormat>(jsonData);
        }

        void onFailed(long responseCode, string msg)
        {
        }

        if (string.IsNullOrEmpty(getAllCCTVInfo.token))
        {
            getAllCCTVInfo.token = WebAPIManager.Token;
        }
        WebAPI_Caller.SendRequest(getAllCCTVInfo, onSuccess, onFailed);
    }
}
