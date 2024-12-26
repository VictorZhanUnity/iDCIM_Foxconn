using System;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// �]�ưt�m�޲z��
/// </summary>
public class DeviceAssetManager : ModulePage
{
    [Header(">>> ���� - ���d��T���O ")]
    [SerializeField] private ServerRackInfoPanel_RE rackInfoPanel;

    [Header(">>> ���� - �]�Ƹ�T���O(�]�� or ���d) ")]
    [SerializeField] private DeviceInfoPanel_RE deviceInfoPanel;

    public override void OnInit(Action onInitComplete = null)
    {
        Debug.Log("DeviceAssetManager OnInit.");
        onInitComplete?.Invoke();
    }

    protected override void InitEventListener()
    {
        RaycastHitManager.onSelectObjectEvent.AddListener(OnClickModelHandler);
    }

    private void OnClickModelHandler(Transform target)
    {
        Data_iDCIMAsset data = deviceAssetDataManager.FindDataByModelName(target);
        if (data.system.Equals("DCR", StringComparison.OrdinalIgnoreCase))
        {
            rackInfoPanel.ShowData(data);
            deviceInfoPanel.gameObject.SetActive(false);
        }
        else
        {
            rackInfoPanel.HideToBackground();
            deviceInfoPanel.ShowData(data);
        }
    }

    protected override void RemoveEventListener()
    {
        RaycastHitManager.onSelectObjectEvent.RemoveListener(OnClickModelHandler);
    }
    protected override void OnShowHandler()
    {
        InitEventListener();
        modelList.ForEach(model => model.GetComponent<Collider>().enabled = true);
    }

    protected override void OnCloseHandler()
    {
        RemoveEventListener();
        modelList.ForEach(model => model.GetComponent<Collider>().enabled = false);
        RaycastHitManager.RestoreSelectedObjects();
    }

    #region [Components]
    private DeviceAssetDataManager _deviceAssetDataManager { get; set; }
    private DeviceAssetDataManager deviceAssetDataManager =>
        _deviceAssetDataManager ??= transform.parent.Find("�겣�]�Ƹ�ƺ޲z��").GetComponent<DeviceAssetDataManager>();
    #endregion
}
