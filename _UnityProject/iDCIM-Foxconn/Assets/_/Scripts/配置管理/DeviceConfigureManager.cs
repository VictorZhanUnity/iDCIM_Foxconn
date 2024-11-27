using System.Collections.Generic;
using UnityEngine;
using VictorDev.Common;
using static DeviceConfigure_DataHandler;

/// <summary>
/// [配置管理] DeviceConfigure
/// </summary>
[RequireComponent(typeof(DeviceConfigure_DataHandler))]
public class DeviceConfigureManager : iDCIM_ModuleManager
{
    [Header(">>> [資料項] 所有庫存設備資料")]
    [SerializeField] private List<StockDeviceSet> dataList;

    [Header(">>> [組件] HUD - 庫存設備清單")]
    [SerializeField] private Comp_StockDeviceList stockDeviceList;

    [Header(">>> [組件] 視窗 - 上架設備輸入資訊")]
    [SerializeField] private Panel_StockDeviceUploadInfo deviceUploadInfoPanel;

    [Header(">>> [Prefab] - 訊息通知")]
    [SerializeField] private NotifyListItem notifyPrefab;

    private DeviceConfigure_DataHandler _dataHandler { get; set; }
    private DeviceConfigure_DataHandler dataHandler => _dataHandler ??= GetComponent<DeviceConfigure_DataHandler>();

    protected override void OnShowHandler()
    {
        GetAllStockDevice();
    }

    protected override void OnCloseHandler()
    {
    }

    private void GetAllStockDevice()
    {
        void onFailed(long responseCode, string msg) { }
        dataHandler.GetAllStockDevice((data) => dataList = data, onFailed);
    }
    private void OnEnable()
    {
        dataHandler.onGetAllStockDevices.AddListener(stockDeviceList.ShowData);
        stockDeviceList.onDeployDeviceModel.AddListener(deviceUploadInfoPanel.ShowData);
        deviceUploadInfoPanel.onClickUploadDevice.AddListener(OnUploadStockDeviceHandler);
    }

    private void OnUploadStockDeviceHandler(StockDeviceSet deviceSet)
    {
        NotificationManager.CreateNotifyMessage(notifyPrefab, "設備上架成功!!", deviceSet.deviceAsset, null, null);
    }

    private void OnDisable()
    {
        dataHandler.onGetAllStockDevices.RemoveAllListeners();
        stockDeviceList.onDeployDeviceModel.RemoveAllListeners();
    }
}
