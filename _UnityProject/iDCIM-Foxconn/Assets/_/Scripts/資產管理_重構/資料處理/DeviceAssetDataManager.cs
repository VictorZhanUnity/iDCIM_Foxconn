using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using VictorDev.RevitUtils;
using Random = UnityEngine.Random;

/// <summary>
/// 資產管理 - 資料載入
/// </summary>
public class DeviceAssetDataManager : Module
{
    [Header(">>> [Receiver] - 資料接收器")]
    [SerializeField] private List<DeviceAssetDataReceiver> receivers;

    [Header(">>> [資料項] - 現有機櫃與設備清單")]
    [SerializeField] private List<Data_ServerRackAsset> dataRack;

    [Header(">>> [WebAPI] - 取得現有機櫃與設備")]
    [SerializeField] private WebAPI_Request request;

    [Header(">>> 資產管理器")]
    [SerializeField] private DeviceAssetManager deviceAssetManager;

    private Action onInitComplete { get; set; }
    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        GeAlltDeviceAssets();
    }

    [ContextMenu("- 取得現有機櫃與設備")]
    private void GeAlltDeviceAssets()
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
        dataRack = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonData);
        List<Data_DeviceAsset> deviceList = dataRack.SelectMany(rack => rack.containers).ToList();

        // 隱藏不在JSON資料內的設備
        deviceAssetManager.modelList.Where(model => (model.name.Contains("RACK") || model.name.Contains("ATEN")) == false)
        .ToList().ForEach(model =>
        {
            bool isHaveData = deviceList.Any(dataDevice => model.name.Contains(dataDevice.deviceName));
            model.gameObject.SetActive(isHaveData);
        });

        // 設置相對應模型
        dataRack.ForEach(rack =>
        {
            rack.model = deviceAssetManager.modelList.FirstOrDefault(model => model.name.Contains(rack.deviceName));
#if true
            // Demo - 隨機移除機櫃裡的某些設備
            List<Data_DeviceAsset> toHideDevices = new List<Data_DeviceAsset>();
            for (int i = 0; rack.containers.Count > i; i++)
            {
                bool isToRemove = Random.Range(0, 11) < 3;
                if (isToRemove)
                {
                    deviceAssetManager.modelList.FirstOrDefault(model => model.name.Contains(rack.containers[i].deviceName)).gameObject.SetActive(false);
                    toHideDevices.Add(rack.containers[i]);
                }
            }
            rack.containers = rack.containers.Except(toHideDevices).ToList();
#endif
        });

        // 設定device屬於哪個rack
        dataRack.ForEach(rack =>
        {
            rack.containers.ForEach(device =>
            {
                device.model = deviceAssetManager.modelList.FirstOrDefault(model => model.name.Contains(device.deviceName));
                device.rack = rack;
                device.model.parent = rack.model;
            });
        });

        //發送資料
        receivers.ForEach(receiver => receiver.ReceiveData(dataRack));
    }
    /// <summary>
    /// 依據模型名稱尋找相對應的資料項
    /// </summary>
    public Data_iDCIMAsset FindDataByModelName(Transform target)
    {
        Data_iDCIMAsset result;
        string deviceName = RevitHandler.GetDevicePath(target.name);
        deviceName = deviceName.Split(":")[1];

        result = dataRack.FirstOrDefault(rack => rack.devicePath.Contains(deviceName, StringComparison.OrdinalIgnoreCase));
        result ??= dataRack.SelectMany(rack => rack.containers).FirstOrDefault(device => device.devicePath.Contains(deviceName, StringComparison.OrdinalIgnoreCase));
        return result;
    }
}
