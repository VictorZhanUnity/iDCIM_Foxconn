using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using VictorDev.RevitUtils;

/// <summary>
/// 配置管理 - 資料載入
/// </summary>
public class DeviceConfigureDataManager : Module
{
    [Header(">>> [Receiver] - 資料接收器")]
    [SerializeField] private List<DeviceAssetDataReceiver> receivers;

    [Header(">>> [資料項] - 機櫃與設備清單")]
    [SerializeField] protected List<Data_ServerRackAsset> dataRack;

    [Header(">>> [WebAPI] - 取得機櫃與設備(現有 or 庫存)")]
    [SerializeField] private WebAPI_Request request;

    private Action onInitComplete { get; set; }
    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        GetDeviceAssets();
    }

    [ContextMenu("- 取得機櫃與設備")]
    protected void GetDeviceAssets()
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
        SetDeviceModel();

        if (EditorApplication.isPlaying) //Editor在Play狀態下才Invoke資料給各自receiver
        {
            //發送資料
            receivers.ForEach(receiver => receiver.ReceiveData(dataRack));
        }
    }

    /// <summary>
    /// 設置相對應模型
    /// </summary>
    protected virtual void SetDeviceModel()
    {
    }


    /// <summary>
    /// 依據模型名稱尋找相對應的資料項
    /// </summary>
    public Data_iDCIMAsset FindDataByModelName(Transform target)
    {
        Data_iDCIMAsset result;
        string deviceName = RevitHandler.GetDevicePath(target.name);
        deviceName = deviceName.Split(":")[1];
        Debug.Log($"deviceName: {deviceName}");

        result = dataRack.FirstOrDefault(rack => rack.devicePath.Contains(deviceName, StringComparison.OrdinalIgnoreCase));
        result ??= dataRack.SelectMany(rack => rack.containers).FirstOrDefault(device => device.devicePath.Contains(deviceName, StringComparison.OrdinalIgnoreCase));
        return result;
    }
}
