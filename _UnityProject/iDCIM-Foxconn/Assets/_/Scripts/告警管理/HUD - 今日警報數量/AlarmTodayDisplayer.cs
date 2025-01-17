using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using static AlarmHistoryDataManager;
using Debug = UnityEngine.Debug;

/// [Component] - 今日警報數量
public class AlarmTodayDisplayer : MonoBehaviour, IAlarmHistoryDataReceiver
{
    [Header(">>> [Receiver] - 接收器 - AlarmTypeDisplayer")] [SerializeField]
    private List<AlarmTypeDisplayer> receivers;

    [Header(">>> [Event] - 當分類項目被點擊時Invoke {分類後列表，標題}")]
    public UnityEvent<List<Data_AlarmHistoryData>, string> onItemClicked = new();

    /// 接收資料，需轉成JSON字串再重新解析成新的List，以避免變更到原始資料
    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        string jsonString = JsonConvert.SerializeObject(datas);
        filteData = JsonConvert.DeserializeObject<List<Data_AlarmHistoryData>>(jsonString);
        filteData.ForEach(data =>
        {
            data.alarms = data.alarms.Where(alarm =>
                DateTimeHandler.isDateInToday(DateTimeHandler.StrToLocalTime(alarm.alarmTime))).ToList();
        });
        int amountOfToday = filteData.SelectMany(data=>data.alarms).Count();
        DotweenHandler.DoInt(txtTodayAlertAmount, int.Parse(txtTodayAlertAmount.text), amountOfToday);
        InvokeData();
        
        //設定當月份告警數量
        int alertAmountOfThisMonth = datas.SelectMany(data=> data.alarms).Count(alarm => DateTimeHandler.IsDateInThisMonth(alarm.AlarmTime));
        DotweenHandler.DoInt(txtThisMonthAlertAmount, int.Parse(txtThisMonthAlertAmount.text), alertAmountOfThisMonth);
    }

    /// 發送資料
    private void InvokeData() => receivers.ForEach(receiver => receiver.ReceiveData(filteData));

    #region [Initialize]
    private void Awake()
    {
        txtThisMonth.SetText(DateTime.Today.Month.ToString());
        ResetUI();
    }
    private void OnEnable() => receivers.ForEach(receiver => receiver.OnItemClicked.AddListener((dataList, title)=>onItemClicked.Invoke(dataList, $"今日告警記錄 - {title}")));
    private void OnDisable()
    {
      receivers.ForEach(receiver => receiver.OnItemClicked.RemoveAllListeners());
      ResetUI();
    }

    private void ResetUI()
    {
        txtTodayAlertAmount.SetText("0");
        txtThisMonthAlertAmount.SetText("0");
    }
    #endregion
    
    #region [Components]
    [Header("[資料項] - 已過濾為今日的告警記錄")]
    private List<Data_AlarmHistoryData> filteData;

    private TextMeshProUGUI txtTodayAlertAmount => _txtTodayAlertAmount ??=
        transform.Find("Container").Find("txt今日警報數量").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtTodayAlertAmount;

    [Header("[Comps] - 這個月份的告警數量")]
    public TextMeshProUGUI txtThisMonth;
    public TextMeshProUGUI txtThisMonthAlertAmount;
    #endregion
}