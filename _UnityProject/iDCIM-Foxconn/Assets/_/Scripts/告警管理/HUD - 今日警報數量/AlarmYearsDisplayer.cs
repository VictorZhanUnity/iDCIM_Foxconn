using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static AlarmHistoryDataManager;
using Debug = UnityEngine.Debug;

/// [Component] - 年度警報數量
public class AlarmYearsDisplayer : MonoBehaviour, IAlarmHistoryDataReceiver
{
    [Header(">>> [Receiver] - 接收器 - AlarmTypeDisplayer")] [SerializeField]
    private List<AlarmTypeDisplayer> receivers;

    [Header(">>> [Event] - 當分類項目被點擊時Invoke {告警記錄清單，標題}")]
    public UnityEvent<List<Data_AlarmHistoryData>, string> onItemClicked = new();

    public AlarmHistoryDataManager manager;
    
    /// 接收資料，需轉成JSON字串再重新解析成新的List，以避免變更到原始資料
    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        sourceDataJsonString = JsonConvert.SerializeObject(datas);
        UpdateUI();
    }

    private void GetRecordOfYear()
    {
        manager.GetAlarmRecordOfYear(SelectedYear, ReceiveData);
    }
    
    private void UpdateUI()
    {
        filteData = JsonConvert.DeserializeObject<List<Data_AlarmHistoryData>>(sourceDataJsonString);
        filteData.ForEach(data =>
        {
            data.alarms = data.alarms.Where(alarm =>
                DateTimeHandler.isDateInYear(DateTimeHandler.StrToLocalTime(alarm.alarmTime), SelectedYear)).ToList();
        });
        InvokeData();
    }

    /// 發送資料
    private void InvokeData() => receivers.ForEach(receiver => receiver.ReceiveData(filteData));

    #region [Initialize]
    private void OnEnable()
    {
        receivers.ForEach(receiver => receiver.OnItemClicked.AddListener((dataList, title)=>onItemClicked.Invoke(dataList, $"{SelectedYear}年告警記錄 - {title}")));
        DropdownYears.onValueChanged.AddListener((selectedIndex)=>GetRecordOfYear());
    }

    private void OnDisable()
    {
      receivers.ForEach(receiver => receiver.OnItemClicked.RemoveAllListeners());
      DropdownYears.onValueChanged.RemoveAllListeners();
    }
    #endregion
    
    #region [Components]
    private string sourceDataJsonString;
    [Header("[資料項] - 已過濾為年度的告警記錄")] 
    public List<Data_AlarmHistoryData> filteData;

    /// Dropdown目前所選年份
    private int SelectedYear =>int.Parse(DropdownYears.options[DropdownYears.value].text.Trim());
    
    private TMP_Dropdown DropdownYears => _dropdownYears ??=
        transform.Find("Dropdown年度選擇").GetComponent<TMP_Dropdown>();
    private TMP_Dropdown _dropdownYears;
    #endregion
}