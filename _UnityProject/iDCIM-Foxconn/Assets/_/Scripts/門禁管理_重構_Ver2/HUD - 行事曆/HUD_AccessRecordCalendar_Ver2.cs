using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Calendar;
using VictorDev.Common;
using static DataAccessRecord;

/// <summary>
/// [組件] 門禁管理首頁的行事曆
/// </summary>
public class HUD_AccessRecordCalendar_Ver2 : AccessRecordDataReceiver
{
    [Header(">>> [Event] - 單選日期時Invoke {所選日期, 過瀘後資料}")]
    public UnityEvent<DateTime, PageData> onSelectedDateEvent = new UnityEvent<DateTime, PageData>();

    /// 取得當年度門禁資料
    public override void ReceiveData(DataAccessRecord datas)
    {
        dataOfYear = datas;
        //依日期群組化
        Dictionary<DateTime, int> filter = dataOfYear.pageData.users.Select(data => data.DateAccessTime)
            .GroupBy(date => new DateTime(date.Year, date.Month, date.Day)).ToDictionary(group => group.Key, group => group.Count());
        calendarManager.CheckDateIsHaveData(filter.Keys.ToList());
        OnSelectedDateHandler(DateTime.Today);
    }

    private void OnSelectedDateHandler(DateTime selectedDate)
    {
        PageData pageDate = new PageData()
        {
            chartData = dataOfYear.pageData.chartData.Where(data => DateTimeHandler.isDateInDay(data.DateFrom, selectedDate)).ToList(),
            users = dataOfYear.pageData.users.Where(data => DateTimeHandler.isDateInDay(data.DateAccessTime, selectedDate)).ToList(),
        };
        onSelectedDateEvent.Invoke(selectedDate, pageDate);
    }

    #region [Initialize]
    private void OnEnable()
    {
        calendarManager.onSelectedDateEvent.AddListener(OnSelectedDateHandler);
    }

    private void OnDisable() => calendarManager.onSelectedDateEvent.RemoveAllListeners();
    #endregion

    #region [Components]

    [Header(">>> [資料項] - 當年度門禁資料")]
    private DataAccessRecord dataOfYear;
    [Header(">>> 組件")]
    [SerializeField] private CalendarManager calendarManager;
    #endregion
}
