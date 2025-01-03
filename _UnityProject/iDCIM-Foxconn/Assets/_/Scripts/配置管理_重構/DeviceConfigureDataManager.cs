using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;

/// <summary>
/// 配置管理 - 資料載入
/// </summary>
public class DeviceConfigureDataManager : Module, IJsonParseReceiver
{
    [Header(">>> [Receiver] - 資料接收器")]
    [SerializeField] private List<DeviceAssetDataReceiver> receivers;

    [Header(">>> [資料項] - 庫存機櫃與設備清單")]
    [SerializeField] private List<Data_DeviceAsset> stockDevices;

    [Header(">>> [WebAPI] - 取得庫存機櫃與設備")]
    [SerializeField] private WebAPI_Request request;

    public DeviceModelManager deviceModelManager;

    private Action onInitComplete { get; set; }
    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        GetStockDeviceAssets();
    }

    [ContextMenu("- 取得庫存機櫃與設備")]
    private void GetStockDeviceAssets()
    {
        WebAPI_LoginManager.CheckToken(request);
        WebAPI_Caller.SendRequest(request, onSuccessHandler, null);

        void onSuccessHandler(long responseCode, string jsonData)
        {
            Debug.Log($"{name} OnInit");
            ParseJson(jsonData);
            onInitComplete?.Invoke();
        }
    }
    /// <summary>
    /// 解析JSON資料
    /// </summary>
    public void ParseJson(string jsonData)
    {
        stockDevices.Clear();
        //全部設備
        List<Data_DeviceAsset> allDeviceData = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonData).SelectMany(rack => rack.containers).ToList();
        // 設置相對應模型
        //庫存
        deviceModelManager.allDeviceModel.ForEach(stockDevice =>
        {
            Data_DeviceAsset device = allDeviceData.FirstOrDefault(device => device.deviceName.Contains(stockDevice.name));
            device.model = stockDevice.model;
            stockDevices.Add(device);
        });

        //發送資料
        receivers.ForEach(receiver => receiver.ReceiveData(stockDevices));
    }
}
