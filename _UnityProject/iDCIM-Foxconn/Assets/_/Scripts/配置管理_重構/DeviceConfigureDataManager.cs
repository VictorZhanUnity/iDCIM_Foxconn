using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;

/// <summary>
/// �t�m�޲z - ��Ƹ��J
/// </summary>
public class DeviceConfigureDataManager : Module, IJsonParseReceiver
{
    [Header(">>> [Receiver] - ��Ʊ�����")]
    [SerializeField] private List<DeviceAssetDataReceiver> receivers;

    [Header(">>> [��ƶ�] - �w�s���d�P�]�ƲM��")]
    [SerializeField] private List<Data_DeviceAsset> stockDevices;

    [Header(">>> [WebAPI] - ���o�w�s���d�P�]��")]
    [SerializeField] private WebAPI_Request request;

    public DeviceModelManager deviceModelManager;

    private Action onInitComplete { get; set; }
    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        GetStockDeviceAssets();
    }

    [ContextMenu("- ���o�w�s���d�P�]��")]
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
    /// �ѪRJSON���
    /// </summary>
    public void ParseJson(string jsonData)
    {
        stockDevices.Clear();
        //�����]��
        List<Data_DeviceAsset> allDeviceData = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonData).SelectMany(rack => rack.containers).ToList();
        // �]�m�۹����ҫ�
        //�w�s
        deviceModelManager.allDeviceModel.ForEach(stockDevice =>
        {
            Data_DeviceAsset device = allDeviceData.FirstOrDefault(device => device.deviceName.Contains(stockDevice.name));
            device.model = stockDevice.model;
            stockDevices.Add(device);
        });

        //�o�e���
        receivers.ForEach(receiver => receiver.ReceiveData(stockDevices));
    }
}
