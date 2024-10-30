using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// 視窗 - 機櫃資訊面板
/// </summary>
public class ServerRackInfoPanel : MonoBehaviour
{
    [Header(">>> [資料項 ] 設備清單選取項目")]
    [SerializeField] private ListItem_Device listItem;
    [SerializeField] private Data_ServerRackAsset data;

    [Header(">>> 點擊項目時Invoke")]
    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    [Header(">>> UI組件")]
    [SerializeField] private RackRUList rackRUList;
    [SerializeField] private DeviceInfoDisplay deviceInfoDisplay;
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private TextMeshProUGUI txtDeviceName;
    [SerializeField] private DeviceInfoPanel deviceInfoPanelPrefab;


    private void Start()
    {
        rackRUList.OnClickItemEvent.AddListener(OnClickItemEvent.Invoke);
    }

    /// <summary>
    /// 顯示資料
    /// </summary>
    public void ShowData(ListItem_Device target)
    {
        listItem = target;
        data = (Data_ServerRackAsset)listItem.data;

        DotweenHandler.ToBlink(txtDeviceName, data.deviceName);

        rackRUList.ShowRULayout(target);
        deviceInfoDisplay.ShowData(data);

        doTweenFadeController.FadeIn();
    }
    public void Close() => doTweenFadeController.FadeOut();



    // ===============================================

    private void ShowCOBieInfo(DeviceRUItem item)
    {
        /*GameManager.ToSelectTarget(item.DeviceModel);

        if (currentDeviceInfoPanel != null) currentDeviceInfoPanel.Close();
        currentDeviceInfoPanel = ObjectPoolManager.GetInstanceFromQueuePool<DeviceInfoPanel>(deviceInfoPanelPrefab, container_DeviceInfoPanel);
        currentDeviceInfoPanel.Show(item);*/
    }

    public void Show(Demo_Rack data)
    {
        /* txtLabel.SetText(RevitHandler.GetRackModelName(data.rack.name));
         rackRUList.ShowRULayout(data.Devices);
         doTweenFadeController.FadeIn();*/
    }

}