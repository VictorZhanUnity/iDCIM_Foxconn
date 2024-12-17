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

    /// <summary>
    /// 點擊該資料項時Invoke
    /// </summary>
    public UnityEvent<ListItem_Device_RE> onClickItemEvent { get; set; } = new UnityEvent<ListItem_Device_RE>();

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
        txtDeviceName.SetText(data.deviceName);
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

    #region [Event Listener]
    private void OnEnable() => InitListener();
    private void OnDisable() => RemoveListener();
    private void InitListener()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) onClickItemEvent.Invoke(this);
        });
    }
    private void RemoveListener() => toggle.onValueChanged.RemoveAllListeners();
    #endregion

    #region [Components]
    private Toggle _toggle { get; set; }
    private Toggle toggle => _toggle ??= GetComponent<Toggle>();
    private Transform _hLayout { get; set; }
    private Transform hLayout => _hLayout ??= transform.Find("Container").Find("HLayout");
    private TextMeshProUGUI _txtDeviceName { get; set; }
    private TextMeshProUGUI txtDeviceName => _txtDeviceName ??= hLayout.parent.Find("txtDeviceName").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtWatt { get; set; }
    private TextMeshProUGUI txtWatt => _txtWatt ??= hLayout.Find("txtWatt").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtWeight { get; set; }
    private TextMeshProUGUI txtWeight => _txtWeight ??= hLayout.Find("txtWeight").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtHeightU { get; set; }
    private TextMeshProUGUI txtHeightU => _txtHeightU ??= hLayout.Find("txtHeightU").GetComponent<TextMeshProUGUI>();
    #endregion
}