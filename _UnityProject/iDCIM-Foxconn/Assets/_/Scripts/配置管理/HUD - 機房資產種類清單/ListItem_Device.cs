using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// [清單資料項] - 機房現有設備清單
/// </summary>
public class ListItem_Device : MonoBehaviour
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_iDCIMAsset _data;

    [Header(">>> 點擊該資料項時Invoke")]
    public UnityEvent<ListItem_Device> onClickItemEvent = new UnityEvent<ListItem_Device>();

    [Header(">>> UI組件")]
    [SerializeField] private Toggle toggleRow;
    [SerializeField] private TextMeshProUGUI txtDevicePathName, txtWatt, txtWeight, txtHeightU;

    public Data_iDCIMAsset data => _data;

    public ToggleGroup toggleGroup { set => toggleRow.group = value; }

    private void Start() => toggleRow.onValueChanged.AddListener((isOn) =>
    {
        if (isOn) onClickItemEvent.Invoke(this);
    });

    public void ShowData(Data_iDCIMAsset data)
    {
        _data = data;
        txtDevicePathName.SetText(data.deviceName);
        txtWatt.SetText(data.information.watt.ToString());
        txtWeight.SetText(data.information.weight.ToString());
        txtHeightU.SetText(data.information.heightU.ToString());
    }
}