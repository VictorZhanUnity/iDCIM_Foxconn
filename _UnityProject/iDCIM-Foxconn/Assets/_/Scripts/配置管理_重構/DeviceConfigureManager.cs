using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using Debug = VictorDev.Common.Debug;

public class DeviceConfigureManager : ModulePage
{
    #region [Components]
    public DeviceAssetDataManager deviceAssetDataManager;
    private List<Transform> _devices { get; set; }
    private List<Transform> devices => _devices ??= modelList.Where(model => (model.name.Contains("RACK") || model.name.Contains("ATEN")) == false).ToList();
    #endregion

    public override void OnInit(Action onInitComplete = null)
    {
        Debug.Log("DeviceAssetDataManager OnInit.");
        onInitComplete?.Invoke();
    }

    protected override void InitEventListener()
    {
        RaycastHitManager.onSelectObjectEvent.AddListener(OnClickDeviceHandler);
        RaycastHitManager.onDeselectObjectEvent.AddListener(OnDeselectDeviceHandler);
        deviceController.onClickRemoveDeviceEvent.AddListener(OnClickRemoveDeviceHandler);
    }

    protected override void OnCloseHandler()
    {
        deviceAssetDataManager.DataRack.ForEach(rack => rack.HideAvailableRuSpacer());
        devices.ForEach(model =>
        {
            model.GetComponent<Collider>().enabled = false;
            RaycastHitManager.CancellObjectSelected(model);
        });
    }

    protected override void OnShowHandler()
    {
        deviceAssetDataManager.DataRack.ForEach(rack => rack.ShowAvailableRuSpacer());

        modelList.Except(devices).ToList().ForEach(model => model.GetComponent<Collider>().enabled = false);
        devices.ForEach(model => model.GetComponent<Collider>().enabled = true);
    }

    protected override void RemoveEventListener()
    {
        RaycastHitManager.onSelectObjectEvent.RemoveListener(OnClickDeviceHandler);
        RaycastHitManager.onDeselectObjectEvent.RemoveListener(OnDeselectDeviceHandler);
        deviceController.onClickRemoveDeviceEvent.RemoveListener(OnClickRemoveDeviceHandler);
    }

    public DeviceAssetSystemList deviceSystemList;
    public DeviceEmptyRuCreator deviceEmptyRuCreator;
    public NotifyListItemTextMessage notifyPrefab;
    /// <summary>
    /// �I���U�[�]�Ʈ�
    /// </summary>
    private void OnClickRemoveDeviceHandler(Data_DeviceAsset data)
    {
        Transform rackModel = data.model.transform.parent;

        data.model.gameObject.SetActive(false);
        data.model.transform.parent = null;
        deviceSystemList.AddDeviceItem(data);
        deviceController.ToClose();

        NotifyListItemTextMessage notifyItem = NotificationManager.CreateNotifyMessage(notifyPrefab);
        notifyItem.ShowMessage("�]�Ƥw�U�[!", data.deviceName);

        //�إ�RU�Ů檫��
        for (int i = data.rackLocation; i < data.rackLocation + data.information.heightU; i++)
        {
            RackSpacer rackSpacer = deviceEmptyRuCreator.CreateRuSpace(data.rack, i);
            rackSpacer.gameObject.SetActive(true);
            data.rack.availableRackSpacerList.Add(rackSpacer);
        }
    }

    private Transform currentSelectModel;
    public ToolTip_DeivceController deviceController;
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
    private void OnDeselectDeviceHandler(Transform target) => deviceController.ToClose();
    private bool IsDevice(Transform targetModel)
    {
        return targetModel.name.Contains("Server")
            || targetModel.name.Contains("Switch")
            || targetModel.name.Contains("Router");
    }
}
