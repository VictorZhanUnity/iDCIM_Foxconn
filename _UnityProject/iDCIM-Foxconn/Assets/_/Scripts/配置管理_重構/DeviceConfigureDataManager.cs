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
/// �t�m�޲z - ��Ƹ��J
/// </summary>
public class DeviceConfigureDataManager : Module
{
    [Header(">>> [Receiver] - ��Ʊ�����")]
    [SerializeField] private List<DeviceAssetDataReceiver> receivers;

    [Header(">>> [��ƶ�] - ���d�P�]�ƲM��")]
    [SerializeField] protected List<Data_ServerRackAsset> dataRack;

    [Header(">>> [WebAPI] - ���o���d�P�]��(�{�� or �w�s)")]
    [SerializeField] private WebAPI_Request request;

    private Action onInitComplete { get; set; }
    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        GetDeviceAssets();
    }

    [ContextMenu("- ���o���d�P�]��")]
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
    /// �ѪRJSON���
    /// </summary>
    public void ParseJson(string jsonData)
    {
        dataRack = JsonConvert.DeserializeObject<List<Data_ServerRackAsset>>(jsonData);
        SetDeviceModel();

        if (EditorApplication.isPlaying) //Editor�bPlay���A�U�~Invoke��Ƶ��U��receiver
        {
            //�o�e���
            receivers.ForEach(receiver => receiver.ReceiveData(dataRack));
        }
    }

    /// <summary>
    /// �]�m�۹����ҫ�
    /// </summary>
    protected virtual void SetDeviceModel()
    {
    }


    /// <summary>
    /// �̾ڼҫ��W�ٴM��۹�������ƶ�
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
