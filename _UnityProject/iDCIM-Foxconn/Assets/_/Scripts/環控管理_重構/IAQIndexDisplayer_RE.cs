using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

/// <summary>
/// IAQ單項指數顯示器
/// </summary>
public class IAQIndexDisplayer_RE : BlackboxDataReceiver
{
    [Header(">>> IAQ指數名稱")]
    [SerializeField] private string columnName;
    public string ColumnName => columnName;
    public float value { get; private set; }
    [Header(">>> 點擊時Invoke")]
    public UnityEvent<IAQIndexDisplayer_RE> onClickButtonEvent = new UnityEvent<IAQIndexDisplayer_RE>();

    public override void ReceiveIAQData(List<Data_Blackbox> datas)
    {
        List<Data_Blackbox> filter = datas.Where(data => data.tagName.Contains(columnName)).ToList();
        value = filter.Average(data => data.value);
        txtValue.SetText(value.ToString("0.#"));
    }
    private void OnEnable() => btn?.onClick.AddListener(() => onClickButtonEvent?.Invoke(this));
    private void OnDisable() => btn?.onClick.RemoveAllListeners();

    #region [Components]
    private TextBlinker _txtValue { get; set; }
    protected TextBlinker txtValue => _txtValue ??= transform.Find("txtValue").GetComponent<TextBlinker>();
    private Button _btn { get; set; }
    protected Button btn => _btn ??= GetComponent<Button>();
    #endregion
}
