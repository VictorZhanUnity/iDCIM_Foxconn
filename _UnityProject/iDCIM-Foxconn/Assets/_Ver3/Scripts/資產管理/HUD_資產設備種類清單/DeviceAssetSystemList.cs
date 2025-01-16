using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;


/// 資產設備種類列表
public class DeviceAssetSystemList : DeviceAssetDataReceiver
{
    [Header(">>> 點擊設備類型Toggle時Invoke")]
    public UnityEvent<List<Data_iDCIMAsset>> onClickDeviceSystem = new UnityEvent<List<Data_iDCIMAsset>>();

    /// 設定設備列表
    public override void ReceiveData(List<Data_ServerRackAsset> datas)
    {
        _rackList = datas; //DCR - 機櫃列表
        ReceiveData(_rackList.SelectMany(rack => rack.containers).ToList());
    }
    public override void ReceiveData(List<Data_DeviceAsset> datas)
    {
        List<Data_DeviceAsset> deviceList = datas;
        _serverList = new List<Data_DeviceAsset>(); //DCS - server列表
        _switchList = new List<Data_DeviceAsset>(); //DCN - switch列表
        _routerList = new List<Data_DeviceAsset>(); //DCN - router列表

        //進行設備分類
        deviceList.ForEach(device =>
        {
            if (device.system.Contains("DCS")) _serverList.Add(device);
            else if (device.devicePath.Contains("Switch")) _switchList.Add(device);
            else if (device.devicePath.Contains("Router")) _routerList.Add(device);
        });

        SetStringFormat(TxtNumOfRack, _rackList.Count);
        SetStringFormat(TxtNumOfServer, _serverList.Count);
        SetStringFormat(TxtNumOfSwitch, _switchList.Count);
        SetStringFormat(TxtNumOfRouter, _routerList.Count);
    }

    [ContextMenu("- 隨機資料測試")]
    private void ContextTest()
    {
        SetStringFormat(TxtNumOfRack, Random.Range(8,21));
        SetStringFormat(TxtNumOfServer, Random.Range(10,25));
        SetStringFormat(TxtNumOfSwitch, Random.Range(30,151));
    }

    private void SetStringFormat(TextMeshProUGUI txt, int count)
    {
        if (count > 0)
        {
            DotweenHandler.DoInt(txt, 0, count);
            //DotweenHandler.ToBlink(txt, $"共{count.ToString()}台", 0.2f, 0.2f, true);
        }
    }

    public void AddDeviceItem(Data_DeviceAsset device)
    {
        if (device.system.Contains("DCS"))
        {
            _serverList.Add(device);
            if (ToggleServer.isOn) onClickDeviceSystem?.Invoke(_serverList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Switch"))
        {
            _switchList.Add(device);
            if (ToggleServer.isOn) onClickDeviceSystem?.Invoke(_switchList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Router"))
        {
            _routerList.Add(device);
            if (ToggleServer.isOn) onClickDeviceSystem?.Invoke(_routerList.Cast<Data_iDCIMAsset>().ToList());
        }
        void SetStringFormat(TextMeshProUGUI txt, int count, Toggle toggle)
        {
            if (count > 0)
            {
                DotweenHandler.ToBlink(txt, $"共{count.ToString()}台", 0.2f, 0.2f, true);
            }
            toggle.gameObject.SetActive(count > 0); //若數量為0，則隱藏該項目
        }
        SetStringFormat(TxtNumOfServer, _serverList.Count, ToggleServer);
        SetStringFormat(TxtNumOfSwitch, _switchList.Count, ToggleSwitch);
        SetStringFormat(TxtNumOfRouter, _routerList.Count, ToggleRouter);
    }

    
    /// 上架設備完成後移除該項目
    public void RemoveDeviceData(ListItem_Device_RE item)
    {
         // 從列表上移除
        Data_DeviceAsset device = item.data as Data_DeviceAsset;
        if (device.system.Contains("DCS"))
        {
            _serverList.Remove(device);
            if (ToggleServer.isOn) onClickDeviceSystem?.Invoke(_serverList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Switch"))
        {
            _switchList.Remove(device);
            if (ToggleServer.isOn) onClickDeviceSystem?.Invoke(_switchList.Cast<Data_iDCIMAsset>().ToList());
        }
        else if (device.devicePath.Contains("Router"))
        {
            _routerList.Remove(device);
            if (ToggleServer.isOn) onClickDeviceSystem?.Invoke(_routerList.Cast<Data_iDCIMAsset>().ToList());
        }
        void SetStringFormat(TextMeshProUGUI txt, int count, Toggle toggle)
        {
            if (count > 0)
            {
                DotweenHandler.ToBlink(txt, $"共{count.ToString()}台", 0.2f, 0.2f, true);
            }
            toggle.gameObject.SetActive(count > 0); //若數量為0，則隱藏該項目
        }
        SetStringFormat(TxtNumOfServer, _serverList.Count, ToggleServer);
        SetStringFormat(TxtNumOfSwitch, _switchList.Count, ToggleSwitch);
        SetStringFormat(TxtNumOfRouter, _routerList.Count, ToggleRouter);
    }

    #region [Event Listener]
    private void OnEnable()
    {
        void OnToggleChanged(bool isOn, List<Data_iDCIMAsset> data)
        {
            if (isOn) onClickDeviceSystem?.Invoke(data);
        }
        ToggleRack.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _rackList.Cast<Data_iDCIMAsset>().ToList()));
        ToggleServer.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _serverList.Cast<Data_iDCIMAsset>().ToList()));
        ToggleSwitch.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _switchList.Cast<Data_iDCIMAsset>().ToList()));
        ToggleRouter.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _routerList.Cast<Data_iDCIMAsset>().ToList()));

        //預設機櫃為點選狀態
        if (ToggleRack.gameObject.activeSelf) ToggleRack.isOn = true;
        else ToggleServer.isOn = true;
        
        TxtNumOfRack.SetText("0");
        TxtNumOfServer.SetText("0");
        TxtNumOfSwitch.SetText("0");
    }
    private void OnDisable()
    {
        ToggleRack.onValueChanged.RemoveAllListeners();
        ToggleServer.onValueChanged.RemoveAllListeners();
        ToggleSwitch.onValueChanged.RemoveAllListeners();
        ToggleRouter.onValueChanged.RemoveAllListeners();
    }
    #endregion

    #region [Components]

    private List<Data_ServerRackAsset> _rackList = new List<Data_ServerRackAsset>();
    private List<Data_DeviceAsset> _serverList = new List<Data_DeviceAsset>();
    private List<Data_DeviceAsset> _switchList = new List<Data_DeviceAsset>();
    private List<Data_DeviceAsset> _routerList = new List<Data_DeviceAsset>();
    private Transform VLayout => _vLayout ??= transform.Find("VLayout");
    private Transform _vLayout ;
    private Toggle ToggleRack => _toggleRack ??= VLayout.GetChild(0).GetComponent<Toggle>();
    private Toggle _toggleRack ;
    private Toggle ToggleServer => _toggleServer ??= VLayout.GetChild(1).GetComponent<Toggle>();
    private Toggle _toggleServer ;
    private Toggle ToggleSwitch => _toggleSwitch ??= VLayout.GetChild(2).GetComponent<Toggle>();
    private Toggle _toggleSwitch ;
    private Toggle ToggleRouter => _toggleRouter ??= VLayout.GetChild(3).GetComponent<Toggle>();
    private Toggle _toggleRouter ;
    private TextMeshProUGUI TxtNumOfRack => _txtNumOfRack ??= ToggleRack.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtNumOfRack ;
    private TextMeshProUGUI TxtNumOfServer => _txtNumOfServer ??= ToggleServer.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtNumOfServer ;
    private TextMeshProUGUI TxtNumOfSwitch => _txtNumOfSwitch ??= ToggleSwitch.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtNumOfSwitch ;
    private TextMeshProUGUI TxtNumOfRouter => _txtNumOfRouter ??= ToggleRouter.transform.Find("txt數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtNumOfRouter ;
    #endregion
}
