using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Calendar;
using VictorDev.Common;
using static Data_AccessRecord_Ver2;

/// <summary>
/// [組件] 門禁管理首頁的行事曆
/// </summary>
public class Comp_AccessRecordCalendar_Ver2 : AccessRecordDataReceiver
{
    [Header(">>> [資料項] - 當年度門禁資料")]
    [SerializeField] private List<Data_AccessRecord_Ver2> dataOfYear;

    [Header(">>> [Event] - 單選日期時Invoke {所選日期, 過瀘後資料}")]
    public UnityEvent<DateTime, PageData> onSelectedDateEvent = new UnityEvent<DateTime, PageData>();

    [Header(">>> 組件")]
    [SerializeField] private CalendarManager calendarManager;

    /// <summary>
    /// 取得當年度門禁資料
    /// </summary>
    public override void ReceiveData(List<Data_AccessRecord_Ver2> datas)
    {
        dataOfYear = datas;
        //依日期群組化
        Dictionary<DateTime, int> filter = dataOfYear.SelectMany(record => record.pageData.users).Select(data => data.DateAccessTime)
            .GroupBy(date => new DateTime(date.Year, date.Month, date.Day)).ToDictionary(group => group.Key, group => group.Count());
        calendarManager.CheckDateIsHaveData(filter.Keys.ToList());
        OnSelectedDateHandler(DateTime.Today);
    }

    private void OnSelectedDateHandler(DateTime selectedDate)
    {
        PageData pageDate = new PageData()
        {
            chartData = dataOfYear.SelectMany(record => record.pageData.chartData).Where(data => DateTimeHandler.isDateInDay(data.DateFrom, selectedDate)).ToList(),
            users = dataOfYear.SelectMany(record => record.pageData.users).Where(data => DateTimeHandler.isDateInDay(data.DateAccessTime, selectedDate)).ToList(),
        };
        onSelectedDateEvent.Invoke(selectedDate, pageDate);
    }

    private void OnEnable() => calendarManager.onSelectedDateEvent.AddListener(OnSelectedDateHandler);
    private void OnDisable() => calendarManager.onSelectedDateEvent.RemoveAllListeners();
}
