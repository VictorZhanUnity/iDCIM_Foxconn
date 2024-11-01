using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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

    [Header(">>> 點擊標題時Invoke")]
    public UnityEvent<ListItem_Device> OnClickRackTitleBar = new UnityEvent<ListItem_Device>();

    [Header(">>> 點擊RU項目時Invoke")]
    public UnityEvent<DeviceRUItem> OnClickRUItemEvent = new UnityEvent<DeviceRUItem>();

    [Header(">>> UI組件")]
    [SerializeField] private RackRUList rackRUList;
    [SerializeField] private Toggle toggleTitlebar;
    [SerializeField] private DeviceInfoDisplay deviceInfoDisplay;
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private TextMeshProUGUI txtDeviceName;
    [SerializeField] private DeviceInfoPanel deviceInfoPanelPrefab;

    private void Start()
    {
        toggleTitlebar.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) OnClickRackTitleBar.Invoke(listItem);
        });
        rackRUList.OnClickItemEvent.AddListener(OnClickRUItemEvent.Invoke);
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
}