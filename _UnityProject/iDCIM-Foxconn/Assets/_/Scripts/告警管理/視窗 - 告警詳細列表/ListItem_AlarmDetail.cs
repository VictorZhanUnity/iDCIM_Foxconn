using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using static Data_Blackbox;
/// [組件] - 詳細告警視窗列表項目
public class ListItem_AlarmDetail : MonoBehaviour
{
    public void ReceiveAlaramData(Alarm data, string tagName)
    {
        _alarmData = data;
        _tagName = tagName;
        UpdateUI();
    }

    private void UpdateUI()
    {
        gameObject.SetActive(true);
        void ToTween(TextMeshProUGUI target, string value) =>DotweenHandler.ToBlink(target, value, 0.1f, 0.2f, true);
        ToTween(TxtDate, _alarmData.AlarmTime.ToString());
        ToTween(TxtTagName, _tagName);
        ToTween(TxtDescription, _alarmData.alarmMessage);
    }

    #region [Initialize]
    private void OnEnable() => Btn.onClick.AddListener(()=>OnItemClick?.Invoke(_alarmData));
    private void OnDisable() => Btn.onClick.RemoveAllListeners();
    #endregion
   
    #region [Components]
    [Header("[資料項] - 告警歷史記錄清單")]
    private Alarm _alarmData;
    
    /// [Event] - 點擊時Invoke {Data_AlarmHistoryData}
    public UnityEvent<Alarm> OnItemClick { get; set; } = new UnityEvent<Alarm>();

    private string _tagName;
    private Button Btn => _btn ??= GetComponent<Button>();
    private Button _btn;
    private TextMeshProUGUI TxtDate => _txtDate ??= transform.Find("txtDate").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtDate;
    private TextMeshProUGUI TxtTagName => _txtTagName ??= transform.Find("txtTagName").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtTagName;
    private TextMeshProUGUI TxtDescription => _txtDescription ??= transform.Find("txtDescription").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtDescription;
    #endregion
}
