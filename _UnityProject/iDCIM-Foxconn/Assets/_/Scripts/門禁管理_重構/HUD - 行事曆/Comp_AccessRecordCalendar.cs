using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Calendar;
using VictorDev.Common;
using static Data_AccessRecord;

/// <summary>
/// [組件] 門禁管理首頁的行事曆
/// </summary>
public class Comp_AccessRecordCalendar : MonoBehaviour
{
    [Header(">>> [資料項] - 今年度門禁資料")]
    [SerializeField] private List<Data_AccessRecord> dataOfThisYear;

    [Header(">>> [Event] - 單選日期時Invoke {所選日期, 過瀘後資料}")]
    public UnityEvent<DateTime, PageData> onSelectedDateEvent = new UnityEvent<DateTime, PageData>();

    [Header(">>> 組件")]
    [SerializeField] private CalendarManager calendarManager;

    private void Start()
    {
        calendarManager.onSelectedDateEvent.AddListener(OnSelectedDateHandler);
    }

    private void OnSelectedDateHandler(DateTime selectedDate)
    {
        PageData pageDate = new PageData()
        {
            chartData = dataOfThisYear.SelectMany(record => record.pageData.chartData).Where(data => DateTimeHandler.isDateInDay(data.DateFrom, selectedDate)).ToList(),
            users = dataOfThisYear.SelectMany(record => record.pageData.users).Where(data => DateTimeHandler.isDateInDay(data.DateAccessTime, selectedDate)).ToList(),
        };
        onSelectedDateEvent.Invoke(selectedDate, pageDate);
    }

    public void SetDatas(List<Data_AccessRecord> data)
    {
        dataOfThisYear = data;

        //依日期群組化
        Dictionary<DateTime, int> filter = data.SelectMany(record => record.pageData.users).Select(data => data.DateAccessTime)
            .GroupBy(date => new DateTime(date.Year, date.Month, date.Day)).ToDictionary(group => group.Key, group => group.Count());

        calendarManager.CheckDateIsHaveData(filter.Keys.ToList());
        OnSelectedDateHandler(DateTime.Today);
    }
}
