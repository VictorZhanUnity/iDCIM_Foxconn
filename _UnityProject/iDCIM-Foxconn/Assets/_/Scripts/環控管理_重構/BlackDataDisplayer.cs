using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

/// <summary>
/// IAQ單項指數顯示器
/// </summary>
public class BlackDataDisplayer : BlackboxDataReceiver
{
    [Header(">>> [資料項]")]
    [SerializeField] protected List<Data_Blackbox> datas;

    [Header(">>>依Tag名稱取值")]
    public string tagName;
    public float value { get; private set; }
    [Header(">>> 點擊時Invoke")]
    public UnityEvent<BlackDataDisplayer> onClickButtonEvent = new UnityEvent<BlackDataDisplayer>();

    public override void ReceiveData(List<Data_Blackbox> blackBoxData)
    {
        GroupData(blackBoxData);
        if(txtValue != null) txtValue.SetText(value.ToString("0.#"));
    }
    protected void GroupData(List<Data_Blackbox> blackBoxData)
    {
        datas = blackBoxData.Where(data => data.tagName.Contains(tagName)).ToList();
        if(datas.Count > 0) value = datas.Average(data => data.value);
    }

    private void OnEnable() => btn?.onClick.AddListener(() => onClickButtonEvent?.Invoke(this));
    private void OnDisable() => btn?.onClick.RemoveAllListeners();

    #region [Components]
    private TextBlinker _txtValue { get; set; }
    protected TextBlinker txtValue => _txtValue ??= transform.Find("txtValue")?.GetComponent<TextBlinker>();
    private Button _btn { get; set; }
    protected Button btn => _btn ??= GetComponent<Button>();
    #endregion
}
