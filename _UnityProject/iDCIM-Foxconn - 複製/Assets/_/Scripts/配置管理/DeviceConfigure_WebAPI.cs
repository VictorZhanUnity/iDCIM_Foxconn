using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Net.WebAPI;
using static VictorDev.RevitUtils.RevitHandler;

[Serializable]
public class DeviceConfigure_WebAPI
{
    [Header(">>> [配置管理] 取得所有庫存設備")]
    [SerializeField] private WebAPI_Request request_GetStockDevice;

    public void GetAllStockDevice()
    {
        void onSuccess(long responseCode, string jsonData)
        {
            List<Data_iDCIMAsset> data = JsonConvert.DeserializeObject<List<Data_iDCIMAsset>>(jsonData);
        }

        void onFailed(long responseCode, string msg)
        {
        }

        WebAPI_Caller.SendRequest(request_GetStockDevice, onSuccess, onFailed);
    }
}
