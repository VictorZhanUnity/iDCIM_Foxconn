using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

/// BlackBox資料數值顯示器
public class BlackboxDataDisplayer : BlackboxDataReceiver
{
    public float value { get; private set; }
    [Header(">>> 點擊時Invoke")]
    public UnityEvent<BlackboxDataDisplayer> onClickButtonEvent = new UnityEvent<BlackboxDataDisplayer>();
    
    [Header(">>>依Tag名稱取值")]
    public string tagName;
    
    public override void ReceiveData(List<Data_Blackbox> blackBoxData)
    {
        GroupData(blackBoxData);
        if(TxtValue != null) TxtValue.SetText(value.ToString("0.#"));
    }
    protected void GroupData(List<Data_Blackbox> blackBoxData)
    {
        DataList = blackBoxData.Where(data => data.tagName.Contains(tagName)).ToList();
        if(DataList.Count > 0) value = DataList.Average(data => data.value ?? 0) ;
    }

    #region [Initialize]
    private void OnEnable() => Btn?.onClick.AddListener(() => onClickButtonEvent?.Invoke(this));
    private void OnDisable() => Btn?.onClick.RemoveAllListeners();
    #endregion

    #region [Components]

    [Header(">>> [資料項]")]
    protected List<Data_Blackbox> DataList;
    
    protected TextBlinker TxtValue => _txtValue ??= transform.Find("txtValue")?.GetComponent<TextBlinker>();
    private TextBlinker _txtValue;
    protected Button Btn => _btn ??= GetComponent<Button>();
    private Button _btn;
    #endregion
}
