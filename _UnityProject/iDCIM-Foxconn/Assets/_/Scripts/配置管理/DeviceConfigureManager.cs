using System.Collections.Generic;
using UnityEngine;
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


    protected override void OnShowHandler()
    {
        GetAllStockDevice();
    }
    private void GetAllStockDevice()
    {
        void onFailed(long responseCode, string msg) { }
        dataHandler.GetAllStockDevice((data) => dataList = data, onFailed);
    }
    protected override void OnCloseHandler()
    {
    }

    private void OnEnable()
    {
        dataHandler.onGetAllStockDevices.AddListener(stockDeviceList.ShowData);
        stockDeviceList.onCreateTempDeviceModel.AddListener(deviceUploadInfoPanel.ShowData);
        deviceUploadInfoPanel.onUploadDeviceComplete.AddListener(stockDeviceList.UpdateList);
    }

    private void OnDisable()
    {
        dataHandler.onGetAllStockDevices.RemoveAllListeners();
        stockDeviceList.onCreateTempDeviceModel.RemoveAllListeners();
    }

    #region [>>> Components]
    private DeviceConfigure_DataHandler _dataHandler { get; set; }
    private DeviceConfigure_DataHandler dataHandler => _dataHandler ??= GetComponent<DeviceConfigure_DataHandler>();
    #endregion
}
