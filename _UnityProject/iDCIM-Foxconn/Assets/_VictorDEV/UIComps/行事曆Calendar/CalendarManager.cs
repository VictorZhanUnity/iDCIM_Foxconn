using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using static VictorDev.CONFIG;

public class CalendarManager : MonoBehaviour
{
    public EnumLanguage lang = EnumLanguage.en_US; // 用於選擇語言 (中文或英文)


    public TextMeshProUGUI txtMonth; // 顯示月份的 TextMeshProUGUI
    public TextMeshProUGUI txtYear;  // 顯示年份的 TextMeshProUGUI
    public DayItem dayPrefab;     // 每一天的 DayItem 預置
    public Transform calendarGrid;   // GridLayoutGroup 的容器
    private CultureInfo selectedCulture => new CultureInfo(lang.ToString().Replace("_", "-"));

    private DateTime currentDate;
    private DateTime? startDate = null; // 儲存起始日期
    private DateTime? endDate = null;   // 儲存結束日期

    void Start() => ShowToday();

    /// <summary>
    /// 顯示當前月份與日期
    /// </summary>
    public void ShowToday()
    {
        currentDate = DateTime.Now;
        GenerateCalendar();
    }

    /// <summary>
    /// 生成行事曆
    /// </summary>
    void GenerateCalendar()
    {
        // 根據當前語言設置月份與年份顯示
        txtMonth.text = currentDate.ToString("MMMM", selectedCulture);
        txtYear.text = currentDate.Year.ToString();

        // 清空先前生成的天數
        foreach (Transform child in calendarGrid)
        {
            Destroy(child.gameObject);
        }

        // 取得該月份的天數
        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

        // 計算本月第一天是星期幾
        int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

        // 計算上個月的天數
        DateTime previousMonth = firstDayOfMonth.AddMonths(-1);
        int daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);

        // 生成空白格來對齊星期 - 改為生成上個月的日期
        for (int i = 0; i < startDayOfWeek; i++)
        {
            DateTime prevMonthDate = new DateTime(previousMonth.Year, previousMonth.Month, daysInPreviousMonth - (startDayOfWeek - 1) + i);
            CreateDayItem(prevMonthDate); // 生成上個月日期，並標記為非本月
        }

        // 生成當前月份的每一天
        for (int day = 1; day <= daysInMonth; day++)
        {
            DateTime dayDate = new DateTime(currentDate.Year, currentDate.Month, day);
            CreateDayItem(dayDate); // 生成本月日期，並標記為本月
        }

        // 生成下個月的日期，填滿剩餘的格子
        int totalGridSlots = 42; // 假設一個日曆的總格子數是 42（6 行 7 列）
        int generatedDays = startDayOfWeek + daysInMonth;
        int remainingDays = totalGridSlots - generatedDays;

        DateTime nextMonth = firstDayOfMonth.AddMonths(1);
        for (int day = 1; day <= remainingDays; day++)
        {
            DateTime nextMonthDate = new DateTime(nextMonth.Year, nextMonth.Month, day);
            CreateDayItem(nextMonthDate); // 生成下個月日期，並標記為非本月
        }
    }

    /// <summary>
    /// 創建並初始化一個 DayItem
    /// </summary>
    void CreateDayItem(DateTime date)
    {
        DayItem dayItem = Instantiate(dayPrefab, calendarGrid);
        dayItem.date = date; // 設置日期
        dayItem.isSelectedMonth = date.Month == currentDate.Month;

        // 設定顯示選擇範圍內的日期高亮
        if (IsDateInRange(date))
        {
            dayItem.GetComponentInChildren<TextMeshProUGUI>().color = Color.green; // 這裡你可以改變顏色或其他高亮效果
        }

        // 添加點擊事件
        dayItem.onClickEvent.AddListener(() => OnDayClicked(date));
    }

    /// <summary>
    /// 處理日期被點擊事件
    /// </summary>
    void OnDayClicked(DateTime clickedDate)
    {
        if (startDate == null) // 如果尚未選擇起始日期
        {
            startDate = clickedDate;
            Debug.Log($"選擇起始日期: {startDate}");
        }
        else if (endDate == null) // 已選擇起始日期但尚未選擇結束日期
        {
            endDate = clickedDate;
            Debug.Log($"選擇結束日期: {endDate}");

            // 確保結束日期大於起始日期
            if (endDate < startDate)
            {
                DateTime temp = startDate.Value;
                startDate = endDate;
                endDate = temp;
            }

            Debug.Log($"範圍：{startDate} 到 {endDate}");
        }
        else
        {
            // 如果已經選擇了起始和結束日期，則重新選擇，從頭開始
            startDate = clickedDate;
            endDate = null;
            Debug.Log($"重新選擇起始日期: {startDate}");
        }

        // 更新行事曆以顯示新的範圍
        GenerateCalendar();
    }

    /// <summary>
    /// 判斷日期是否在選擇的範圍內
    /// </summary>
    bool IsDateInRange(DateTime date)
    {
        if (startDate != null && endDate != null)
        {
            return date >= startDate && date <= endDate;
        }
        return false;
    }

    // 切換到上個月
    public void PreviousMonth()
    {
        currentDate = currentDate.AddMonths(-1); // 日期退一個月
        GenerateCalendar();
    }

    // 切換到下個月
    public void NextMonth()
    {
        currentDate = currentDate.AddMonths(1); // 日期加一個月
        GenerateCalendar();
    }

    // 切換到上一年
    public void PreviousYear()
    {
        currentDate = currentDate.AddYears(-1); // 日期退一年
        GenerateCalendar();
    }

    // 切換到下一年
    public void NextYear()
    {
        currentDate = currentDate.AddYears(1); // 日期加一年
        GenerateCalendar();
    }

    // 語言變更時的處理
    public void ChangeLanguage(EnumLanguage lang)
    {
        this.lang = lang;
        GenerateCalendar();
    }

}
