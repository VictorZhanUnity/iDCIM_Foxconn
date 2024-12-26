using TMPro;
using VictorDev.Common;

/// <summary>
/// [HUD - 訊息通知視窗] - 訊息列表項目 - 上架設備
/// </summary>
public class NotifyListItem_UploadDevice : NotifyListItem
{
    private TextMeshProUGUI _txtDeviceName { get; set; }
    private TextMeshProUGUI txtDeviceName => _txtDeviceName ??= container.Find("txtDeviceName").GetComponent<TextMeshProUGUI>();

    protected override void OnShowData(INotifyData notifyData)
    {
        if (notifyData is Data_DeviceAsset data) txtDeviceName.SetText(data.deviceName);
    }
}
