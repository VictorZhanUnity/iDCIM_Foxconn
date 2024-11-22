using System.Linq;
using UnityEngine;
using VictorDev.CameraUtils;
using VictorDev.Common;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// [資產管理]
/// </summary>
public class UIManager_DeviceAsset : MonoBehaviour
{
    [Header(">>> UI組件")]
    [SerializeField] private DeviceAssetList deviceAssetList;
    [SerializeField] private ServerRackInfoPanel serverRackInfoPanel;
    [SerializeField] private DeviceInfoPanel deviceInfoPanel;
    [SerializeField] private GameObject uiObj;
    [SerializeField] private DeviceModelVisualizer deviceModelVisualizer;

    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;
            uiObj.SetActive(value);
        }
    }

    private void Start()
    {
        deviceAssetList.onClickListItemEvent.AddListener(OnClickDeviceItemHandler);
        serverRackInfoPanel.OnClickRUItemEvent.AddListener(deviceInfoPanel.ShowData);
        serverRackInfoPanel.OnClickRackTitleBar.AddListener(deviceInfoPanel.ShowData);

        WebAPIManager.GetAllDCRContainer(deviceAssetList.WebAPI_onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string msg)
    {
    }

    private void OnClickDeviceItemHandler(ListItem_Device target)
    {
        Transform targetModel = deviceModelVisualizer.ModelList.FirstOrDefault(model => model.name.Contains(target.data.deviceName));
        OrbitCamera.MoveTargetTo(targetModel); //運鏡
        RaycastHitManager.ToSelectTarget(targetModel); //選取目標模型

        switch (target.data.system)
        {
            case "DCR":
                serverRackInfoPanel.ShowData(target);
                deviceInfoPanel.Close();
                break;
            case "DCN":
            case "DCS":
                deviceInfoPanel.ShowData(target);
                serverRackInfoPanel.Close();
                break;
        }
    }
    /// <summary>
    /// [Inspector]  當點擊模型物件時Invoke (For GameManager)
    /// </summary>
    public void OnSelectDeviceAsset(Transform target)
    {
        Data_iDCIMAsset data = SearchDeviceAssetByModel(target);
        if (data != null) //List清單上所沒有顯示的
        {
            deviceInfoPanel.ShowData(data);
            print($"Data_iDCIMAsset: {data.deviceName} / ");
        }
    }

    public Data_iDCIMAsset SearchDeviceAssetByModel(Transform target)
         => deviceAssetList.SearchDeviceAssetByModel(target);
}


