using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using static DataAccessRecord;
using Debug = VictorDev.Common.Debug;

/// <summary>
/// [組件] 指定某日的進場人數Bar圖
/// </summary>
public class Chart_DayCount : MonoBehaviour
{
    [Header(">>> [資料項] - 指定某日的進場人數")]
    [SerializeField] private PageData pageData;

    [Header(">>> 組件")]
    [SerializeField] private XChartDataHandler chartHandler;
    [SerializeField] private TextMeshProUGUI txtDateDisplay, txtCount;

    private string serieName => "進場人數";
    private string numericFormat => "0人";
    private int splitNumber => 24;

    private DateTime selectedDate { get; set; }

    private void Start()
    {
        txtDateDisplay.SetText(DateTimeHandler.FullDateFormatWithWeekDay(DateTime.Today));

        chartHandler.SetYAxisRangeAndFormat(0, 10, numericFormat);
        chartHandler.SetTooltipFormat(numericFormat);

        chartHandler.SetSeriesName(serieName);
        chartHandler.SetXAxis(DateTimeHandler.hoursOfDay);
        chartHandler.XAxisSplitNumber = splitNumber;

        chartHandler.SetDataZoom(splitNumber);
    }

    public void ShowData(DateTime date, PageData data)
    {
        selectedDate = date;
        pageData = data;
        string stringDate = DateTimeHandler.FullDateFormatWithWeekDay(selectedDate);
        string stringCount = pageData.users.Count.ToString();

        if (gameObject.activeSelf)
        {
            DotweenHandler.ToBlink(txtDateDisplay, stringDate);
            DotweenHandler.ToBlink(txtCount, stringCount, 0.1f, 0.3f);
        }
        else
        {
            txtDateDisplay.SetText(stringDate);
            txtCount.SetText(stringCount);
        }

        AddDataToSeries();
    }

    private void Awake()
    {
        txtDateDisplay.SetText("{日期}");
    }

    private void OnEnable()
    {
        DotweenHandler.ToBlink(txtDateDisplay);
    }

    /// <summary>
    /// 設定Chart
    /// </summary>
    private void AddDataToSeries()
    {
        // 建立整點列表 (00:00 到 23:00)
        List<DateTime> hourlyTimeSlots = Enumerable.Range(0, 24).Select(hour => selectedDate.Date.AddHours(hour)).ToList();

        // 解析 accessTime 並按整點分組
        Dictionary<DateTime, int> groupedData = pageData.users.Select(user => user.DateAccessTime)
            .GroupBy(accessTime => new DateTime(accessTime.Year, accessTime.Month, accessTime.Day, accessTime.Hour, 0, 0)) // 按小時分組
            .ToDictionary(group => group.Key, group => group.Count());

        // 合併整點列表與資料，填充沒有記錄的時段
        List<float> hourlyCounts = hourlyTimeSlots
            .Select(hour => groupedData.ContainsKey(hour) ? (float)groupedData[hour] : 0f)
            .ToList();

        chartHandler.SetSeriesData(serieName, hourlyCounts);
    }
}
