using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// 動態生成機櫃RU空格
/// </summary>
public class DeviceEmptyRuCreator : DeviceAssetDataReceiver
{
    public override void ReceiveData(List<Data_ServerRackAsset> datas)
    {
        CaculateAvailabeRu(datas);
    }

    /// <summary>
    /// 計算每個機櫃的可用RU層
    /// </summary>
    private void CaculateAvailabeRu(List<Data_ServerRackAsset> datas)
    {
        List<int> availableRackUList;   //可使用RU
        List<int> occupyLlst;                //已佔用RU
        datas.ForEach(dataRack =>
        {
            availableRackUList = Enumerable.Range(1, 42).ToList();
            occupyLlst = new List<int>();
            //算出佔用的格數
            dataRack.containers.ForEach(device =>
            {
                for (int i = device.rackLocation; i < device.rackLocation + device.information.heightU; i++)
                {
                    occupyLlst.Add(i);
                }
            });

            //排除後取得可使用的RU層數
            availableRackUList = availableRackUList.Except(occupyLlst).ToList();
            //建立RuSpacer
            availableRackUList.ForEach(locaion => CreateRuSpace(dataRack, locaion));

            //計算剩餘RU層空間有哪幾種高度尺吋
            dataRack.eachSizeOfAvailableRU = availableRackUList.OrderBy(ru => ru) // 排序
                        .Select((ru, index) => ru - index) // 計算偏移值：連續區間具有相同的偏移值
                        .GroupBy(offset => offset) // 按偏移值分組
                        .Select(group => group.Count()) // 計算每組的長度
                        .Distinct() // 獲取唯一的尺吋
                        .ToList();
        });
    }

    /// <summary>
    /// 建立可用RU層物件 {目標機櫃, 第幾層U}
    /// </summary>
    public void CreateRuSpace(Data_ServerRackAsset dataRack, int ruLocation)
    {
        float perRUposY = 0.076f * 0.61f;
        perRUposY = 0.04445f;
        RackSpacer ruSpacer = Instantiate(rackSpacerPrefab, dataRack.model);
        ruSpacer.RuLocation = ruLocation;
        ruSpacer.dataRack = dataRack;
        ruSpacer.transform.localPosition = new Vector3(0, perRUposY * ruLocation + 0.055f, 0);
        dataRack.availableRackSpacerList.Add(ruSpacer);
        ruSpacer.gameObject.SetActive(false);
    }

    public void UpdateDeviceComplete()
    {
        currentListItem = null;
    }

    /// <summary>
    /// 目前指向哪一個RackSpacer
    /// </summary>
    private List<RackSpacer> occupyRackSpacer { get; set; } = new List<RackSpacer>();
    public ListItem_Device_RE currentListItem { get; set; }

    /// <summary>
    /// 當Drag產生暫時的設備時
    /// </summary>
    public void OnCreateTempDeviceHandler(ListItem_Device_RE listItem, RackSpacer rackSpacer)
    {
        RaycastHitManager.RestoreSelectedObjects();
        CancellUploadDevice();

        currentListItem = listItem;

        Data_ServerRackAsset targetRack = rackSpacer.dataRack;
        //上架模型所佔用的RU層數
        occupyRackSpacer = targetRack.ShowRackSpacer(rackSpacer.RuLocation, listItem.data.information.heightU);

        //建立暫時設備模型
        currentListItem.createTempDeviceModel ??= Instantiate(currentListItem.data.model);
        currentListItem.createTempDeviceModel.transform.SetParent(rackSpacer.container, false);
        currentListItem.createTempDeviceModel.localPosition = Vector3.zero;
        currentListItem.createTempDeviceModel.localRotation = Quaternion.Euler(0, 90, 0);
        currentListItem.createTempDeviceModel.transform.localScale = Vector3.one;
        currentListItem.createTempDeviceModel.gameObject.SetActive(true);
        currentListItem.createTempDeviceModel.DOLocalMove(Vector3.zero, 0.3f).From(Vector3.left * 0.3f).SetEase(Ease.OutQuad).SetAutoKill(true);

        ShowPanel();
    }

    public Panel_StockDeviceUploadInfo deviceUploadInfoPanel;
    public DeviceAssetList_RE deviceAssetList_RE;
    private void ShowPanel()
    {
        deviceUploadInfoPanel.onUploadDeviceComplete.RemoveListener(deviceAssetList_RE.UpdateList);
        deviceUploadInfoPanel.ShowData(currentListItem);
        deviceUploadInfoPanel.onUploadDeviceComplete.AddListener(deviceAssetList_RE.UpdateList);
    }

    /// <summary>
    /// 取消上傳設備
    /// </summary>
    public void CancellUploadDevice()
    {
        if (currentListItem != null)
        {
            if (currentListItem.createTempDeviceModel != null)
            {
                currentListItem.createTempDeviceModel.gameObject.SetActive(false);
                currentListItem.createTempDeviceModel.transform.parent = null;
            }
            occupyRackSpacer.ForEach(rack => rack.isForceToShow = false);
            occupyRackSpacer.Clear();
        }
    }

    #region [Components]

    [Header(">>> [Prefab] RU空格Prefab")]

    [SerializeField] private RackSpacer rackSpacerPrefab;
    private DeviceConfigureDataManager _deviceConfigureDataManager { get; set; }
    private DeviceConfigureDataManager deviceConfigureDataManager => _deviceConfigureDataManager ??= GetComponent<DeviceConfigureDataManager>();
    #endregion
}
