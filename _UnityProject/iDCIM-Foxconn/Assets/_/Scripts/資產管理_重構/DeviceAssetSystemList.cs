using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeviceAssetSystemList : DeviceAssetDataReceiver
{
    [Header(">>> 點擊Server機櫃Toggle時Invoke")]
    public UnityEvent<List<Data_ServerRackAsset>> onClickDeviceSystem_Rack = new UnityEvent<List<Data_ServerRackAsset>>();
    [Header(">>> 點擊設備Toggle時Invoke")]
    public UnityEvent<List<Data_DeviceAsset>> onClickDeviceSystem_Device = new UnityEvent<List<Data_DeviceAsset>>();

    [Header(">>> 組件")]
    [SerializeField] private TextMeshProUGUI txtNumOfRack, txtNumOfServer, txtNumOfSwitch, txtNumOfRouter;
    [SerializeField] private Toggle toggleRack, toggleServer, toggleSwitch, toggleRouter;

    public List<Data_ServerRackAsset> rackList;
    private List<Data_DeviceAsset> deviceList;
    public List<Data_DeviceAsset> serverList, switchList, routerList;

    public override void ReceiveData(List<Data_ServerRackAsset> datas)
    {
        rackList = datas;
        deviceList = rackList.SelectMany(rack => rack.containers).ToList();

        serverList = new List<Data_DeviceAsset>();
        switchList = new List<Data_DeviceAsset>();
        routerList = new List<Data_DeviceAsset>();

        deviceList.ForEach(device =>
        {
            if (device.system.Equals("DCR")) serverList.Add(device);
            else if (device.devicePath.Contains("Switch")) switchList.Add(device);
            else if (device.devicePath.Contains("Router")) routerList.Add(device);
        });

        void SetStringFormat(TextMeshProUGUI txt, int count, Toggle toggle)
        {
            txt.SetText($"共{count.ToString()}台");
            toggle.gameObject.SetActive(count > 0);
        }
        SetStringFormat(txtNumOfRack, rackList.Count, toggleRack);
        SetStringFormat(txtNumOfServer, serverList.Count, toggleServer);
        SetStringFormat(txtNumOfSwitch, switchList.Count, toggleSwitch);
        SetStringFormat(txtNumOfRouter, routerList.Count, toggleRouter);
    }

    private void OnEnable()
    {
        toggleRack.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) onClickDeviceSystem_Rack?.Invoke(rackList);
        });
        void OnToggle(bool isOn, List<Data_DeviceAsset> data)
        {
            if (isOn) onClickDeviceSystem_Device?.Invoke(data);
        }
        toggleServer.onValueChanged.AddListener((isOn) => OnToggle(isOn, serverList));
        toggleSwitch.onValueChanged.AddListener((isOn) => OnToggle(isOn, switchList));
        toggleRouter.onValueChanged.AddListener((isOn) => OnToggle(isOn, routerList));
    }
    private void OnDisable()
    {
        toggleRack.onValueChanged.RemoveAllListeners();
        toggleServer.onValueChanged.RemoveAllListeners();
        toggleSwitch.onValueChanged.RemoveAllListeners();
        toggleRouter.onValueChanged.RemoveAllListeners();
    }
}
