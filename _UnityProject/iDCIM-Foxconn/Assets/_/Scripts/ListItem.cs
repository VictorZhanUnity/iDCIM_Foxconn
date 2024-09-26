using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [Header(">>>模型點選器")]
    [SerializeField] SelectableObject selectableObject;

    [Header("--- UI")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI txtLabel;
    
    protected UnityEvent<bool> onToggled { get; set; } = new UnityEvent<bool>();

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    public bool isOn { set => toggle.isOn = value; }

    /// <summary>
    /// 設置選項標題
    /// </summary>
    public string label
    {
        get => txtLabel.text;
        set => txtLabel.SetText(value);
    }

    public void SetupSelectableObjectAndLandmark(SelectableObject obj, Landmark landmark)
    {
        selectableObject = obj;
        obj.listItem = this as ListItem_CCTV;
        landmark.listItem = this;
        txtLabel.SetText(selectableObject.title);
    }
    private void OnEnable()
    {
        onToggled.AddListener((isOn) => selectableObject.IsOn = isOn);
        toggle.onValueChanged.AddListener(onToggled.Invoke);
        selectableObject.onToggleEvent.AddListener(SetIsOnWithoutNotify);
    }

    private void OnDisable()
    {
        try
        {
            onToggled.RemoveListener((isOn) => selectableObject.IsOn = isOn);
            toggle.onValueChanged.RemoveAllListeners();
            selectableObject.onToggleEvent.RemoveListener(SetIsOnWithoutNotify);
        }
        catch (Exception e) { }
    }

    public virtual void SetIsOnWithoutNotify(bool isOn)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.isOn = isOn;
        toggle.onValueChanged.AddListener(onToggled.Invoke);
    }
}
