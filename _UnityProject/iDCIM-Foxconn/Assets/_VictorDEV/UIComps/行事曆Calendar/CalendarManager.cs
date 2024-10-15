using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using TMPro;

public class CalendarManager : MonoBehaviour
{
    public TextMeshProUGUI monthYearText; // 顯示月份與年份的 Text
    public DayItem dayPrefab; // 每一天的 UI 預置
    public Transform calendarGrid; // GridLayoutGroup 的容器

    private DateTime currentDate;

    private DateTime? startDate = null; // 儲存起始日期
    private DateTime? endDate = null;   // 儲存結束日期

    void Start()
    {
        currentDate = DateTime.Now; // 取得當前日期
        GenerateCalendar();
    }

    // 生成行事曆
    void GenerateCalendar()
    {
        CultureInfo englishCulture = new CultureInfo("en-US");
        monthYearText.text = currentDate.ToString("MMMM yyyy", englishCulture); // 使用英文格式顯示月份與年份

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

        // 生成空白格來對齊星期
        for (int i = 0; i < startDayOfWeek; i++)
        {
            Instantiate(dayPrefab, calendarGrid); // 空白格
        }

        // 生成每一天
        for (int day = 1; day <= daysInMonth; day++)
        {
            DayItem dayItem = Instantiate(dayPrefab, calendarGrid);
            TextMeshProUGUI dayText = dayItem.GetComponentInChildren<TextMeshProUGUI>();
            dayText.text = day.ToString();

            DateTime dayDate = new DateTime(currentDate.Year, currentDate.Month, day);

            // 設定顯示選擇範圍內的日期高亮
            if (IsDateInRange(dayDate))
            {
                dayText.color = Color.green; // 這裡你可以改變顏色或其他高亮效果
            }

            // 添加按鈕點擊事件
            int dayCopy = day; // 避免閉包問題
            dayItem.GetComponent<Button>().onClick.AddListener(() => OnDayClicked(dayDate));
        }
    }

    // 處理日期被點擊事件
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
                // 如果結束日期早於起始日期，則交換兩者
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

    // 判斷日期是否在選擇的範圍內
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
}
