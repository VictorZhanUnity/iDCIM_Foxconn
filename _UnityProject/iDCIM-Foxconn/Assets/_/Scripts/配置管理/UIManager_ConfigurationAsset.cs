using System.Linq;
using UnityEngine;
using VictorDev.CameraUtils;

/// <summary>
/// [配置管理]
/// </summary>
public class UIManager_ConfigurationAsset : MonoBehaviour
{
    [Header(">>> UI組件")]
    [SerializeField] private DeviceAssetList deviceList;
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
        deviceList.onClickListItemEvent.AddListener(OnClickDeviceItemHandler);
    }

    private void OnClickDeviceItemHandler(ListItem_Device target)
    {
        Transform targetModel = deviceModelVisualizer.ModelList.FirstOrDefault(model => model.name.Contains(target.data.deviceName));
        OrbitCamera.MoveTargetTo(targetModel); //運鏡
        GameManager.ToSelectTarget(targetModel); //選取目標模型

        switch (target.data.system)
        {
            case "DCR": serverRackInfoPanel.ShowData(target); break;
            case "DCN":
            case "DCS":
                //deviceInfoPanel.ShowData(target);
                break;
        }
    }
}
