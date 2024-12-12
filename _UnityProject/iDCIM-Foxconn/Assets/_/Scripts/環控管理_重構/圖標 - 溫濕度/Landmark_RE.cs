using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 圖標組件
/// <para>+ 直接掛載到目標UI上，再丟給LandmarkManager進行事件註冊即可</para>
/// </summary>
public class Landmark_RE : MonoBehaviour
{
    [Header(">>> 目標模型")]
    public Transform targetModel;

    [Header(">>> 地標高度")]
    public float offsetHeight = 0;

    /// <summary>
    /// Toggle狀態變更時Invoke {isOn, 目標模型}
    /// </summary>
    public UnityEvent<bool, Transform> onToggleValueChanged { get; set; } = new UnityEvent<bool, Transform>();

    public bool isOn { set => toggle.isOn = value; }

    [HideInInspector]
    public float distanceToCamera { get; set; } = 0;

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener((isOn) => onToggleValueChanged?.Invoke(isOn, targetModel));
    }

    private void OnDisable() => toggle.onValueChanged.RemoveAllListeners();

    /// <summary>
    /// 設定Toggle.isOn狀態
    /// <para>+ isInvokeEvent: 是否觸發Toggle事件</para>
    /// </summary>
    public void SetToggleStatus(bool isOn, bool isInvokeEvent = false)
    {
        if (isInvokeEvent == false) OnDisable();
        toggle.isOn = isOn;
        OnEnable();
    }

    #region [Components]
    private RectTransform _rectTransform { get; set; }
    public RectTransform rectTransform => _rectTransform ??= GetComponent<RectTransform>();
    private Toggle _toggle { get; set; }
    public Toggle toggle => _toggle ??= transform.Find("Toggle").GetComponent<Toggle>();
    public ToggleGroup toggleGroup { set => toggle.group = value; }
    #endregion
}

