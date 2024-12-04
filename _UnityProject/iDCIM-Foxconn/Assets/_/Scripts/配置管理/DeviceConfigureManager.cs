using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [Header(">>> [組件] HUD - 庫存設備清單")]
    [SerializeField] private Comp_ServerRackFilter serverRackFilter;

    [Header(">>> [組件] 視窗 - 上架設備輸入資訊")]
    [SerializeField] private Panel_StockDeviceUploadInfo deviceUploadInfoPanel;

    public RackSpacer rackSpacerPrefab;

    protected override void OnShowHandler()
    {
        GetAllStockDevice();
        DeviceModelManager.ShowRackAvailableRuSpacer();
    }
    private void GetAllStockDevice()
    {
        void onFailed(long responseCode, string msg) { }
        dataHandler.GetAllStockDevice((data) => dataList = data, onFailed);
    }
    protected override void OnCloseHandler()
    {
        serverRackFilter.ToClose();
        DeviceModelManager.HideAvailableRuSpacer();
    }

    private void OnEnable()
    {
        dataHandler.onGetAllStockDevices.AddListener(stockDeviceList.ShowData);
        stockDeviceList.onCreateTempDeviceModel.AddListener(deviceUploadInfoPanel.ShowData);
        stockDeviceList.onSelectDeviceModel.AddListener(serverRackFilter.ToFilterRack);
        deviceUploadInfoPanel.onUploadDeviceComplete.AddListener(stockDeviceList.UpdateList);

        RaycastHitManager.onSelectObjectEvent.AddListener(OnClickDeviceHandler);
        RaycastHitManager.onDeselectObjectEvent.AddListener((target)=> deviceController.ToClose());

        deviceController.onClickMoveDeviceEvent.AddListener(OnClickMoveDeviceHandler);
        deviceController.onClickRemoveDeviceEvent.AddListener(OnClickRemoveDeviceHandler);
    }

    private void OnClickMoveDeviceHandler(Data_DeviceAsset data)
    {
      
    }

    private void OnClickRemoveDeviceHandler(Data_DeviceAsset data)
    {
        Transform rackModel = data.model.transform.parent;

        data.model.gameObject.SetActive(false);
        data.model.transform.parent = null;
        stockDeviceList.AddStockDevice(data);
        deviceController.ToClose();

        for (int i = data.rackLocation; i < data.rackLocation+data.information.heightU; i++)
        {
            float perRUposY = 0.076f * 0.61f;
            RackSpacer ruSpacer = ObjectPoolManager.GetInstanceFromQueuePool(rackSpacerPrefab, rackModel);
            ruSpacer.RuIndex = i;
            ruSpacer.transform.localPosition = new Vector3(0, perRUposY * i, 0);
        }
        NotificationManager.CreateNotifyMessage(notifyPrefab, "設備已下架!!", data);
    }

    public NotifyListItem notifyPrefab;

    private void OnDisable()
    {
        dataHandler.onGetAllStockDevices.RemoveAllListeners();
        stockDeviceList.onCreateTempDeviceModel.RemoveAllListeners();

        RaycastHitManager.onSelectObjectEvent.RemoveAllListeners();
        RaycastHitManager.onDeselectObjectEvent.RemoveAllListeners();
    }

    #region [>>> Components]
    private DeviceConfigure_DataHandler _dataHandler { get; set; }
    private DeviceConfigure_DataHandler dataHandler => _dataHandler ??= GetComponent<DeviceConfigure_DataHandler>();
    #endregion


    // =======================

    public ToolTip_DeivceController deviceController;

    private bool IsDevice(Transform targetModel)
    {
        return targetModel.name.Contains("Server")
            || targetModel.name.Contains("Switch")
            || targetModel.name.Contains("Router");
    }

    private Transform currentSelectModel { get; set; }

    private void OnClickDeviceHandler(Transform targetModel)
    {
        if (IsDevice(targetModel))
        {
            if (currentSelectModel == targetModel)
            {
                deviceController.ToClose();
                currentSelectModel = null;
            }
            else
            {
                deviceController.ShowData(targetModel);
            }
        }
    }
}
