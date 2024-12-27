using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;

/// <summary>
/// �ʺA�ͦ����dRU�Ů�
/// </summary>
public class DeviceEmptyRuCreator : DeviceAssetDataReceiver
{

    public override void ReceiveData(List<Data_ServerRackAsset> datas)
    {
        CaculateAvailabeRu(datas);
    }

    /// <summary>
    /// �p��C�Ӿ��d���i��RU�h
    /// </summary>
    private void CaculateAvailabeRu(List<Data_ServerRackAsset> datas)
    {
        List<int> availableRackUList;   //�i�ϥ�RU
        List<int> occupyLlst;                //�w����RU
        datas.ForEach(dataRack =>
        {
            availableRackUList = Enumerable.Range(1, 42).ToList();
            occupyLlst = new List<int>();
            //��X���Ϊ����
            dataRack.containers.ForEach(device =>
            {
                //�i�ΪŮ檺Size�ئT
                int availableRuSize = 0;
                for (int i = device.rackLocation; i < device.rackLocation + device.information.heightU; i++)
                {
                    occupyLlst.Add(i);
                    availableRuSize++;
                }
                //�p��C��RU�Ů檺�ئT�j�p
                if (dataRack.eachSizeOfAvailableRU.Contains(availableRuSize) == false)
                {
                    dataRack.eachSizeOfAvailableRU.Add(availableRuSize);
                }
            });

            //�ư�����o�i�ϥΪ�RU�h��
            availableRackUList = availableRackUList.Except(occupyLlst).ToList();
            //�إ�RuSpacer
            availableRackUList.ForEach(locaion => CreateRuSpace(dataRack, locaion));
        });
    }

    /// <summary>
    /// �إߥi��RU�h���� {�ؼо��d, �ĴX�hU}
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
    /// �ثe���V���@��RackSpacer
    /// </summary>
    private List<RackSpacer> occupyRackSpacer { get; set; } = new List<RackSpacer>();
    public ListItem_Device_RE currentListItem { get; set; }

    /// <summary>
    /// ��Drag���ͼȮɪ��]�Ʈ�
    /// </summary>
    public void OnCreateTempDeviceHandler(ListItem_Device_RE listItem, RackSpacer rackSpacer)
    {
        RaycastHitManager.RestoreSelectedObjects();
        CancellUploadDevice();

        currentListItem = listItem;

        Data_ServerRackAsset targetRack = rackSpacer.dataRack;
        //�W�[�ҫ��Ҧ��Ϊ�RU�h��
        occupyRackSpacer = targetRack.ShowRackSpacer(rackSpacer.RuLocation, listItem.data.information.heightU);

        //�إ߼Ȯɳ]�Ƽҫ�
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
    /// �����W�ǳ]��
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

    [Header(">>> [Prefab] RU�Ů�Prefab")]

    [SerializeField] private RackSpacer rackSpacerPrefab;
    private DeviceConfigureDataManager _deviceConfigureDataManager { get; set; }
    private DeviceConfigureDataManager deviceConfigureDataManager => _deviceConfigureDataManager ??= GetComponent<DeviceConfigureDataManager>();
    #endregion
}
