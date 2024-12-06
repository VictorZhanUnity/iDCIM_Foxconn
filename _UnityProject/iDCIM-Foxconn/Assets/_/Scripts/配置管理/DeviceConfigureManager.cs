using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [Header(">>> [組件] HUD - 庫存設備清單")]
    [SerializeField] private Comp_ServerRackFilter serverRackFilter;

    [Header(">>> [組件] 視窗 - 上架設備輸入資訊")]
    [SerializeField] private Panel_StockDeviceUploadInfo deviceUploadInfoPanel;

    [Header(">>> [組件] RU空格物件")]
    [SerializeField] private RackSpacer rackSpacerPrefab;

    private bool isCreateRuSpacer { get; set; } = false;

    protected override void OnShowHandler()
    {
        GetAllStockDevice();

        if(isCreateRuSpacer == false)
        {
            //每個機櫃建立RU空格層數
            DeviceModelManager.RackDataList.ForEach(data => BuildRackAvailableRuSpacer(data));
            isCreateRuSpacer = true;
        }
    }
    private void GetAllStockDevice()
    {
        void onFailed(long responseCode, string msg) { }
        dataHandler.GetAllStockDevice((data) => dataList = data, onFailed);
    }
    protected override void OnCloseHandler()
    {
        serverRackFilter.ToClose();
        DeviceModelManager.HideAllRackAvailableRuSpacer();
    }

    private void OnEnable()
    {
        dataHandler.onGetAllStockDevices.AddListener(stockDeviceList.ShowData);
        stockDeviceList.onCreateTempDeviceModel.AddListener(deviceUploadInfoPanel.ShowData);
        stockDeviceList.onSelectDeviceModel.AddListener(serverRackFilter.ToFilterRack);
        deviceUploadInfoPanel.onUploadDeviceComplete.AddListener(stockDeviceList.UpdateList);

        RaycastHitManager.onSelectObjectEvent.AddListener(OnClickDeviceHandler);
        RaycastHitManager.onDeselectObjectEvent.AddListener((target) => deviceController.ToClose());

        deviceController.onClickMoveDeviceEvent.AddListener(OnClickMoveDeviceHandler);
        deviceController.onClickRemoveDeviceEvent.AddListener(OnClickRemoveDeviceHandler);
    }

    private void OnClickMoveDeviceHandler(Data_DeviceAsset data)
    {
    }

    /// <summary>
    /// 點擊下架設備時
    /// </summary>
    private void OnClickRemoveDeviceHandler(Data_DeviceAsset data)
    {
        Transform rackModel = data.model.transform.parent;

        data.model.gameObject.SetActive(false);
        data.model.transform.parent = null;
        stockDeviceList.AddStockDevice(data);
        deviceController.ToClose();

        NotificationManager.CreateNotifyMessage(notifyPrefab, "設備已下架!!", data);

        //建立RU空格物件
        Data_ServerRackAsset parentRackData = DeviceModelManager.RackDataList.FirstOrDefault(rack => rackModel.name.Contains(rack.deviceName));
        for (int i = data.rackLocation; i < data.rackLocation + data.information.heightU; i++)
        {
            CreateRuSpace(parentRackData, i);
        }
    }

    public NotifyListItem notifyPrefab;

    private void OnDisable()
    {
        dataHandler.onGetAllStockDevices.RemoveAllListeners();
        stockDeviceList.onCreateTempDeviceModel.RemoveAllListeners();

        RaycastHitManager.onSelectObjectEvent.RemoveAllListeners();
        RaycastHitManager.onDeselectObjectEvent.RemoveAllListeners();
    }

    /// <summary>
    /// 計算建立機櫃內的可用RU層，與建立RU層物件
    /// </summary>
    public void BuildRackAvailableRuSpacer(Data_ServerRackAsset dataRack)
    {
        List<int> availableRackLocationList = Enumerable.Range(1, 42).ToList();
        List<int> occupyLlst = new List<int>();

        //算出佔用的格數
        dataRack.containers.ForEach(device =>
        {
            int counter = 0;
            for (int i = device.rackLocation; i < device.rackLocation + device.information.heightU; i++)
            {
                occupyLlst.Add(i);
                counter++;
            }
            //計算每個RU空格的尺吋大小
            if (dataRack.eachSizeOfAvailableRU.Contains(counter) == false)
            {
                dataRack.eachSizeOfAvailableRU.Add(counter);
            }
        });

        //排除後取得可使用的RU層數
        availableRackLocationList = availableRackLocationList.Except(occupyLlst).ToList();
        //建立RuSpacer
        availableRackLocationList.ForEach(locaion => CreateRuSpace(dataRack, locaion));
    }
    /// <summary>
    /// 建立可用RU層物件
    /// </summary>
    public void CreateRuSpace(Data_ServerRackAsset dataRack, int ruIndex)
    {
        float perRUposY = 0.076f * 0.61f;
        RackSpacer ruSpacer = ObjectPoolManager.GetInstanceFromQueuePool(rackSpacerPrefab, dataRack.model);
        ruSpacer.RuIndex = ruIndex;
        ruSpacer.dataRack = dataRack;
        ruSpacer.transform.localPosition = new Vector3(0, perRUposY * ruIndex, 0);
        dataRack.availableRackSpacerList.Add(ruSpacer);
        ruSpacer.gameObject.SetActive(false);
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
