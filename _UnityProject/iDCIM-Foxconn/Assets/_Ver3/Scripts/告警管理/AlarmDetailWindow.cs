using _VictorDEV.Revit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

public class AlarmDetailWindow : MonoBehaviour
{
    public void ReceiveData(Data_Blackbox.Alarm alarmData)
    {
        Config_iDCIM.AlarmSystemSetting setting = Config_iDCIM.GetAlarmSystemSetting(alarmData.tagName);
        ImgIcon.sprite = setting.icon;
        DotweenHandler.ToBlink(TxtTagName, alarmData.tagName, 0.2f, 0.2f, true);
        DotweenHandler.ToBlink(TxtSystem, $"{setting.label}告警", 0.2f, 0.2f, true);
        DotweenHandler.ToBlink(TxtAlarmTime, alarmData.AlarmTime.ToLocalTime().ToString(), 0.2f, 0.2f, true);
        DotweenHandler.ToBlink(TxtAlarmMessage, alarmData.alarmMessage, 0.2f, 0.2f, true);
        gameObject.SetActive(true);
    }

    #region Components
    private TextMeshProUGUI TxtTagName =>
        _txtTagName ??= transform.Find("Panel").Find("Header").Find("txtTagName").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtTagName;
    private Transform Container => _container ??= transform.Find("Panel").Find("Container");
    private Transform _container;
    private TextMeshProUGUI TxtSystem =>_txtSystem ??= Container.Find("txtSystem").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtSystem;
    private Image ImgIcon => _imgIcon ??= TxtSystem.transform.Find("imgICON").GetComponent<Image>();
    private Image _imgIcon;
    private TextMeshProUGUI TxtAlarmTime =>_txtAlarmTime ??= Container.Find("txtAlarmTime").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtAlarmTime;
    private TextMeshProUGUI TxtAlarmMessage => _txtAlarmMessage ??=Container.Find("txtAlarmMessage").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtAlarmMessage;
    #endregion
}