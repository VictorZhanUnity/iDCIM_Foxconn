using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

/// <summary>
/// 資產設備種類列表
/// </summary>
public class DeviceAssetSystemList : DeviceAssetDataReceiver
{
    [Header(">>> 點擊設備類型Toggle時Invoke")]
    public UnityEvent<List<Data_iDCIMAsset>> onClickDeviceSystem = new UnityEvent<List<Data_iDCIMAsset>>();

    public List<Data_ServerRackAsset> rackList;
    public List<Data_DeviceAsset> serverList;
    public List<Data_DeviceAsset> switchList;
    public List<Data_DeviceAsset> routerList;

    /// <summary>
    /// 設定設備列表
    /// </summary>
    public override void ReceiveData(List<Data_ServerRackAsset> datas)
    {
        rackList = datas; //DCR - 機櫃列表
        ReceiveData(rackList.SelectMany(rack => rack.containers).ToList());
    }
    public override void ReceiveData(List<Data_DeviceAsset> datas)
    {
        List<Data_DeviceAsset> deviceList = datas;
        serverList = new List<Data_DeviceAsset>(); //DCS - server列表
        switchList = new List<Data_DeviceAsset>(); //DCN - switch列表
        routerList = new List<Data_DeviceAsset>(); //DCN - router列表

        //進行設備分類
        deviceList.ForEach(device =>
        {
            if (device.system.Contains("DCS")) serverList.Add(device);
            else if (device.devicePath.Contains("Switch")) switchList.Add(device);
            else if (device.devicePath.Contains("Router")) routerList.Add(device);
        });

        void SetStringFormat(TextMeshProUGUI txt, int count, Toggle toggle)
        {
            if (count > 0)
            {
                DotweenHandler.ToBlink(txt, $"共{count.ToString()}台", 0.2f, 0.2f, true);
            }
            toggle.gameObject.SetActive(count > 0); //若數量為0，則隱藏該項目
        }
        SetStringFormat(txtNumOfRack, rackList.Count, toggleRack);
        SetStringFormat(txtNumOfServer, serverList.Count, toggleServer);
        SetStringFormat(txtNumOfSwitch, switchList.Count, toggleSwitch);
        SetStringFormat(txtNumOfRouter, routerList.Count, toggleRouter);
    }

    public void AddDeviceItem(Data_DeviceAsset device)
    {
        if (device.system.Contains("DCS"))
        {
            serverList.Add(device);
            if (toggleServer.isOn) onClickDeviceSystem?.Invoke(serverList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Switch"))
        {
            switchList.Add(device);
            if (toggleServer.isOn) onClickDeviceSystem?.Invoke(switchList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Router"))
        {
            routerList.Add(device);
            if (toggleServer.isOn) onClickDeviceSystem?.Invoke(routerList.Cast<Data_iDCIMAsset>().ToList());
        }
        void SetStringFormat(TextMeshProUGUI txt, int count, Toggle toggle)
        {
            if (count > 0)
            {
                DotweenHandler.ToBlink(txt, $"共{count.ToString()}台", 0.2f, 0.2f, true);
            }
            toggle.gameObject.SetActive(count > 0); //若數量為0，則隱藏該項目
        }
        SetStringFormat(txtNumOfServer, serverList.Count, toggleServer);
        SetStringFormat(txtNumOfSwitch, switchList.Count, toggleSwitch);
        SetStringFormat(txtNumOfRouter, routerList.Count, toggleRouter);
    }

    /// <summary>
    /// 上架設備完成後移除該項目
    /// </summary>
    public void RemoveDeviceData(ListItem_Device_RE item)
    {
         // 從列表上移除
        Data_DeviceAsset device = item.data as Data_DeviceAsset;
        if (device.system.Contains("DCS"))
        {
            serverList.Remove(device);
            if (toggleServer.isOn) onClickDeviceSystem?.Invoke(serverList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Switch"))
        {
            switchList.Remove(device);
            if (toggleServer.isOn) onClickDeviceSystem?.Invoke(switchList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Router"))
        {
            routerList.Remove(device);
            if (toggleServer.isOn) onClickDeviceSystem?.Invoke(routerList.Cast<Data_iDCIMAsset>().ToList());
        }
        void SetStringFormat(TextMeshProUGUI txt, int count, Toggle toggle)
        {
            if (count > 0)
            {
                DotweenHandler.ToBlink(txt, $"共{count.ToString()}台", 0.2f, 0.2f, true);
            }
            toggle.gameObject.SetActive(count > 0); //若數量為0，則隱藏該項目
        }
        SetStringFormat(txtNumOfServer, serverList.Count, toggleServer);
        SetStringFormat(txtNumOfSwitch, switchList.Count, toggleSwitch);
        SetStringFormat(txtNumOfRouter, routerList.Count, toggleRouter);
    }

    #region [Event Listener]
    private void OnEnable()
    {
        void OnToggleChanged(bool isOn, List<Data_iDCIMAsset> data)
        {
            if (isOn) onClickDeviceSystem?.Invoke(data);
        }
        toggleRack.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, rackList.Cast<Data_iDCIMAsset>().ToList()));
        toggleServer.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, serverList.Cast<Data_iDCIMAsset>().ToList()));
        toggleSwitch.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, switchList.Cast<Data_iDCIMAsset>().ToList()));
        toggleRouter.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, routerList.Cast<Data_iDCIMAsset>().ToList()));

        //預設機櫃為點選狀態
        if (toggleRack.gameObject.activeSelf) toggleRack.isOn = true;
        else toggleServer.isOn = true;
    }
    private void OnDisable()
    {
        toggleRack.onValueChanged.RemoveAllListeners();
        toggleServer.onValueChanged.RemoveAllListeners();
        toggleSwitch.onValueChanged.RemoveAllListeners();
        toggleRouter.onValueChanged.RemoveAllListeners();
    }
    #endregion

    #region [Components]
    private Transform _vLayout { get; set; }
    private Transform vLayout => _vLayout ??= transform.Find("VLayout");
    private Toggle _toggleRack { get; set; }
    private Toggle toggleRack => _toggleRack ??= vLayout.GetChild(0).GetComponent<Toggle>();
    private Toggle _toggleServer { get; set; }
    private Toggle toggleServer => _toggleServer ??= vLayout.GetChild(1).GetComponent<Toggle>();
    private Toggle _toggleSwitch { get; set; }
    private Toggle toggleSwitch => _toggleSwitch ??= vLayout.GetChild(2).GetComponent<Toggle>();
    private Toggle _toggleRouter { get; set; }
    private Toggle toggleRouter => _toggleRouter ??= vLayout.GetChild(3).GetComponent<Toggle>();
    private TextMeshProUGUI _txtNumOfRack { get; set; }
    private TextMeshProUGUI txtNumOfRack => _txtNumOfRack ??= toggleRack.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtNumOfServer { get; set; }
    private TextMeshProUGUI txtNumOfServer => _txtNumOfServer ??= toggleServer.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtNumOfSwitch { get; set; }
    private TextMeshProUGUI txtNumOfSwitch => _txtNumOfSwitch ??= toggleSwitch.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtNumOfRouter { get; set; }
    private TextMeshProUGUI txtNumOfRouter => _txtNumOfRouter ??= toggleRouter.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    #endregion
}
