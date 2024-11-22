using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TableHeaderColumn : MonoBehaviour
{
    [Header(">>> [Event] - 點選時Invoke (是否降幕排序)")]
    public UnityEvent<bool, string> onClickEvent = new UnityEvent<bool, string>();

    private Toggle _toggle { get; set; }
    private Toggle toggle => _toggle ??= GetComponent<Toggle>();
    private TextMeshProUGUI _txt { get; set; }
    private TextMeshProUGUI txt => _txt ??= transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    public ToggleGroup toggleGroup { set => toggle.group = value; }
    /// <summary>
    /// 設定標題
    /// </summary>
    public string label
    {
        get => txt.text;
        set => txt.SetText(value.Trim());
    }
    /// <summary>
    /// 設定欄寬
    /// </summary>
    public float width
    {
        set
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 size = rect.sizeDelta;
            size.x = value;
            rect.sizeDelta = size;
        }
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener((isOn) => onClickEvent.Invoke(isOn, label));
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
        onClickEvent.RemoveAllListeners();
    }
}
