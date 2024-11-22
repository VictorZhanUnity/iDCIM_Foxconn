using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using static DeviceConfigure_DataHandler;

/// <summary>
/// [清單資料項] - 機房現有設備清單
/// </summary>
public class StockDeviceListItem : MonoBehaviour
{
    [Header(">>> [資料項] - StockDeviceSet")]
    [SerializeField] private StockDeviceSet _data;

    [Header(">>> 點擊該資料項時Invoke")]
    public UnityEvent<StockDeviceListItem> onClickItemEvent = new UnityEvent<StockDeviceListItem>();

    #region [組件]
    private Toggle _toggle { get; set; }
    private Toggle toggle => _toggle ??= GetComponent<Toggle>();
    private TextMeshProUGUI _txtDeviceName, _txtWatt, _txtWeight, _txtHeightU;
    private TextMeshProUGUI txtDeviceName => _txtDeviceName ??= transform.GetChild(0).Find("txtDeviceName").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI txtWatt => _txtWatt ??= transform.GetChild(0).Find("HLayout").Find("txtWatt").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI txtWeight => _txtWeight ??= transform.GetChild(0).Find("HLayout").Find("txtWeight").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI txtHeightU => _txtHeightU ??= transform.GetChild(0).Find("HLayout").Find("txtHeightU").GetComponent<TextMeshProUGUI>();
    private Image _imgWatt, _imgWeight, _imgHeightU, _imgBorder;
    private Image imgWatt => _imgWatt ??= txtWatt.transform.Find("imgWatt").GetComponent<Image>();
    private Image imgWeight => _imgWeight ??= txtWeight.transform.Find("imgWeight").GetComponent<Image>();
    private Image imgHeightU => _imgHeightU ??= txtHeightU.transform.Find("imgHeightU").GetComponent<Image>();
    private Image imgBorder => _imgBorder ??= transform.GetChild(0).Find("imgBorder").GetComponent<Image>();
    #endregion

    /// <summary>
    /// 開/關 Toggle
    /// </summary>
    public bool isOn { set => toggle.isOn = value; }
    public ToggleGroup toggleGroup { set => toggle.group = value; }

    public void ShowData(StockDeviceSet data)
    {
        _data = data;
        txtDeviceName.SetText(data.deviceAsset.deviceName);
        txtWatt.SetText(data.deviceAsset.information.watt.ToString());
        txtWeight.SetText(data.deviceAsset.information.weight.ToString());
        txtHeightU.SetText(data.deviceAsset.information.heightU.ToString());
    }
    public void SetToggleWithoutNotify(bool isOn)
    {
        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.isOn = isOn;
        ChangeColor(isOn);

        OnEnable();
    }

    private void ChangeColor(bool isOn)
    {
        Color targetColor = (isOn) ? Color.white : ColorHandler.HexToColor(0x999999);
        txtDeviceName.color = txtWatt.color = txtWeight.color = txtHeightU.color = targetColor;
        imgWatt.color = imgWeight.color = imgHeightU.color = targetColor;
        imgBorder.color = targetColor;
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) onClickItemEvent.Invoke(this);
            ChangeColor(isOn);
        });
    }
    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
        onClickItemEvent.RemoveAllListeners();
    }
}