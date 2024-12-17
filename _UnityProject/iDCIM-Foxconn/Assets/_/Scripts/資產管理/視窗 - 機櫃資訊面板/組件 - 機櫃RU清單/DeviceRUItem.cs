using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// [清單項目] 機櫃RU清單下的資料項目
/// </summary>
public class DeviceRUItem : MonoBehaviour
{
    [Header(">>> [資料項 ] 設備資訊")]
    [SerializeField] private Data_DeviceAsset _data;
    public Data_DeviceAsset data => _data;

    [Header(">>> 點擊項目時Invoke")]
    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    [Header(">>> UI組件")]
    [SerializeField] private Image imgDevice;
    [SerializeField] private TextMeshProUGUI txtLabel;
    [SerializeField] private Toggle toggle;
    [SerializeField] private RectTransform rectTrans;

    public bool isOn { set => toggle.isOn = value; }

    public void SetToggleWithoutNotify(bool isOn)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.isOn = isOn;
        Start();
    }

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    private void Start()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) OnClickItemEvent.Invoke(this);
        });
    }

    public void ShowData(Data_DeviceAsset data)
    {
        _data = data;
        UpdateUI();
    }

    private void UpdateUI()
    {
        name = _data.deviceName;
        txtLabel.SetText(_data.deviceName);

        Vector2 size = rectTrans.sizeDelta;
        size.y = _data.information.heightU * 30;
        rectTrans.sizeDelta = size;

        //位置
        Vector2 pos = rectTrans.localPosition;
        pos.y = (_data.rackLocation - 1) * 30 + 3;
        rectTrans.localPosition = pos;
    }
}