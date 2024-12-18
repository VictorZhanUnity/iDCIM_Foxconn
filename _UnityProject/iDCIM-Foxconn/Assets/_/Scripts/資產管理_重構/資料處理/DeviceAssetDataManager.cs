using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using VictorDev.RevitUtils;

public class DeviceAssetDataManager : Module
{
    [Header(">>> [Receiver] - 資料接收器")]
    [SerializeField] private List<DeviceAssetDataReceiver> receivers;

    [Header(">>> [資料項] - 所有機櫃與設備")]
    [SerializeField] private List<Data_ServerRackAsset> dataRack;

    [Header(">>> [WebAPI] - 所有機櫃與設備")]
    [SerializeField] private WebAPI_Request request;

    [Header(">>> 資產管理器")]
    [SerializeField] private DeviceAssetManager deviceAssetManager;

    private Action onInitComplete { get; set; }

    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        GetAllDeviceAssets();
    }

    [ContextMenu("- 取得所有機櫃與設備")]
    private void GetAllDeviceAssets()
    {
        WebAPI_LoginManager.CheckToken(request);
        WebAPI_Caller.SendRequest(request, onSuccessHandler, null);

        void onSuccessHandler(long responseCode, string jsonString)
        {
            Debug.Log("DeviceAssetDataManager OnInit");
            dataRack = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonString);
            // 設置相對應模型
            dataRack.ForEach(rack =>
            {
                rack.model = deviceAssetManager.modelList.FirstOrDefault(model => model.name.Contains(rack.deviceName));
            });
            dataRack.SelectMany(rack => rack.containers).ToList().ForEach(device =>
            {
                device.model = deviceAssetManager.modelList.FirstOrDefault(model => model.name.Contains(device.deviceName));
            });
            //發送資料
            receivers.ForEach(receiver => receiver.ReceiveData(dataRack));
            onInitComplete?.Invoke();
        }
    }

    /// <summary>
    /// 依據模型名稱尋找相對應的資料項
    /// </summary>
    public Data_iDCIMAsset FindDataByModelName(Transform target)
    {
        Data_iDCIMAsset result;
        string deviceName = RevitHandler.GetDevicePath(target.name);
        result = dataRack.FirstOrDefault(rack => rack.devicePath.Equals(deviceName, StringComparison.OrdinalIgnoreCase));
        result ??= dataRack.SelectMany(rack => rack.containers).FirstOrDefault(device => device.devicePath.Equals(deviceName, StringComparison.OrdinalIgnoreCase));
        return result;
    }
}
