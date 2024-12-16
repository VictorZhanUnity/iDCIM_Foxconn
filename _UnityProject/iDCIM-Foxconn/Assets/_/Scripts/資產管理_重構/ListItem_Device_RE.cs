using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// [清單資料項] - 機房現有設備清單
/// </summary>
public class ListItem_Device_RE : MonoBehaviour
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_iDCIMAsset _data;
    public Data_iDCIMAsset data => _data;

    [Header(">>> 點擊該資料項時Invoke")]
    public UnityEvent<ListItem_Device_RE> onClickItemEvent = new UnityEvent<ListItem_Device_RE>();

    [Header(">>> UI組件")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI txtDevicePathName, txtWatt, txtWeight, txtHeightU;
    public Data_DeviceAsset dataDevice;

    /// <summary>
    /// 開/關 Toggle
    /// </summary>
    public bool isOn { set => toggle.isOn = value; }
    public ToggleGroup toggleGroup { set => toggle.group = value; }

    public void ShowData(Data_iDCIMAsset data)
    {
        _data = data;
        UpdateUI(data);
    }

    private void UpdateUI(Data_iDCIMAsset data)
    {
        txtDevicePathName.SetText(data.deviceName);
        txtWatt.SetText(data.information.watt.ToString());
        txtWeight.SetText(data.information.weight.ToString());
        txtHeightU.SetText(data.information.heightU.ToString());
    }

    public void SetToggleWithoutNotify(bool isOn)
    {
        OnDisable();
        toggle.isOn = isOn;
        OnEnable();
    }
    private void OnEnable()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) onClickItemEvent.Invoke(this);
        });
    }
    private void OnDisable() => toggle.onValueChanged.RemoveAllListeners();
}