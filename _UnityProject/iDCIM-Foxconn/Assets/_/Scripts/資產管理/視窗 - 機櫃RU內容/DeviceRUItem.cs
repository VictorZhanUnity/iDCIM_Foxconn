using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.RevitUtils;

public class DeviceRUItem : MonoBehaviour
{
    [Header(">>> 裝置模型")]
    [SerializeField] private Transform deviceModel;

    [Header(">>> UI組件")]
    [SerializeField] private Image imgDevice;
    [SerializeField] private TextMeshProUGUI txtLabel;
    [SerializeField] private Toggle toggle;

    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    public string deviceName { get; private set; }
    public string system { get; private set; }
    public int heightU { get; private set; }

    public Transform DeviceModel
    {
        get => deviceModel;
        set
        {
            deviceModel = value;
            UpdateUI();
        }
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) OnClickItemEvent.Invoke(this);
        });
    }

    private void UpdateUI()
    {
        name += $" ({deviceModel.name})";
        deviceName = RevitHandler.GetDeviceModelName(deviceModel.name);
        system = RevitHandler.GetDeviceModelSystem(deviceModel.name);
        heightU = RevitHandler.GetDeviceModelHeightU(deviceModel.name);
        txtLabel.SetText(deviceName);

        RectTransform rectTrans = transform.GetComponent<RectTransform>();
        Vector2 size = rectTrans.sizeDelta;
        size.y = heightU * 30;
        rectTrans.sizeDelta = size;
    }
}