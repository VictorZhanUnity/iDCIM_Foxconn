using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.RevitUtils;

public class ListItem_Rack : MonoBehaviour
{
    [Header(">>> 點擊時Invoke")]
    public UnityEvent<ListItem_Rack> onClickItemEvent = new UnityEvent<ListItem_Rack>();
    [Header(">>> 機櫃模型")]
    [SerializeField] private Transform model;
    public Transform rackModel
    {
        get => model;
        set
        {
            model = value;
            string[] strings = value.name.Split("[")[0].Split("-");
            string label = strings[1] + strings[2];

            txtLabel.SetText(label);
            deviceId = RevitHandler.GetDeviceID(model.name);
        }
    }

    [Header(">>> UI組件")]
    [SerializeField] private TextMeshProUGUI txtLabel;
    [SerializeField] private Toggle toggle;
    public ToggleGroup toggleGroup { set => toggle.group = value; }

    public string deviceId { get; private set; }

    private void Start()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) onClickItemEvent.Invoke(this);
        });
    }
}