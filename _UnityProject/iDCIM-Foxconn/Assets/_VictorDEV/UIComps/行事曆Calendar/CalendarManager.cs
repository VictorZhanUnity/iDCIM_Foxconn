using System;
using System.Collections.Generic;
using System.Globalization;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using static VictorDev.CONFIG;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.Calendar
{
    public class CalendarManager : MonoBehaviour
    {
        public enum EnumSelectStyle { 單一日期, 起迄日期 }

        [Header(">>> 日期選取類型")]
        public EnumSelectStyle selectStyle = EnumSelectStyle.起迄日期;

        [Header(">>> [單一日期] 點選單一日期時發送")]
        public UnityEvent<DateTime> onSelectedDateEvent = new UnityEvent<DateTime>();
        public UnityEvent<string> onSelectedDateStrEvent = new UnityEvent<string>();

        [Header(">>> [起迄日期] 開始日期時發送")]
        public UnityEvent<DateTime> onSelectedStartDateEvent = new UnityEvent<DateTime>();
        public UnityEvent<string> onSelectedStartDateStrEvent = new UnityEvent<string>();
        [Header(">>> [起迄日期] 結束日期時發送")]
        public UnityEvent<DateTime> onSelectedEndDateEvent = new UnityEvent<DateTime>();
        public UnityEvent<string> onSelectedEndDateStrEvent = new UnityEvent<string>();
        [Header(">>> [起迄日期] 選擇起迄日期時發送 {起, 迄}")]
        public UnityEvent<DateTime, DateTime> onSelectedDateRangeEvent = new UnityEvent<DateTime, DateTime>();
        [Header(">>> [起迄日期] 是否允許選擇同一天")]
        public bool isAllowSelectSameDay = false;

        [Header(">>> 選擇語言 (中文或英文)")]
        public EnumLanguage enumLang = EnumLanguage.en_US;

        [Header(">>> UI組件")]
        [SerializeField] DayItem dayPrefab;     // 每一天的 DayItem 預置
        [SerializeField] TextMeshProUGUI txtTodayMonth, txtTodayDay, txtTodayWeek; // 顯示當日日期
        [SerializeField] TextMeshProUGUI txtMonth; // 顯示月份的 TextMeshProUGUI
        [SerializeField] TextMeshProUGUI txtYear;  // 顯示年份的 TextMeshProUGUI
        [SerializeField] Transform calendarGrid;   // GridLayoutGroup 的容器
        private CultureInfo lang => new CultureInfo(enumLang.ToString().Replace("_", "-"));

        private DateTime currentDate;
        private DateTime? startDate = null; // 儲存起始日期
        private DateTime? endDate = null;   // 儲存結束日期
        private DateTime result; //用來startDate與endData的轉型

        public DateTime StartDateTime => (DateTime)startDate;
        public DateTime EndDateTime => (DateTime)endDate;

        private List<DayItem> dayItemList { get; set; } = new List<DayItem>();

        void Awake() => ShowToday();

        private List<DateTime> dateWithData { get; set; }
        public void CheckDateIsHaveData(List<DateTime> dateWithData)
        {
            this.dateWithData = dateWithData;
            dayItemList.ForEach(item => item.isHaveData = dateWithData.Contains(item.date));
        }


        /// <summary>
        /// 設定日期{過去N週}
        /// </summary>
        public void SetDate_PastWeeks(int pastWeek = 1)
        {
            DateTime dateTime = DateTime.Today;
            SetDateTimeRange(dateTime.AddDays(pastWeek * -7), dateTime);
        }

        /// <summary>
        /// 設定日期{過去N月}
        /// </summary>
        public void SetDate_PastMonths(int pastMonth = 1)
        {
            DateTime dateTime = DateTime.Today;
            SetDateTimeRange(dateTime.AddMonths(-pastMonth), dateTime);
        }

        /// <summary>
        /// 設定日期{過去N年}
        /// </summary>
        public void SetDate_PastYears(int pastYears = 1)
        {
            DateTime dateTime = DateTime.Today;
            SetDateTimeRange(dateTime.AddYears(-pastYears), dateTime);
        }

        /// <summary>
        /// 設定日期{起、迄}
        /// </summary>
        public void SetDateTimeRange(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate.AddDays(1).AddTicks(-1);
            currentDate = startDate;
            GenerateCalendar();
            onSelectedStartDateStrEvent.Invoke(startDate.ToString(DateTimeHandler.FullDateFormat));
            onSelectedEndDateStrEvent.Invoke(endDate.ToString(DateTimeHandler.FullDateFormat));
            onSelectedDateRangeEvent.Invoke(startDate, endDate);
        }

        /// <summary>
        /// 顯示當前月份與日期
        /// </summary>
        public void ShowToday()
        {
            currentDate = DateTime.Now;
            GenerateCalendar();
            UpdateTodayInfo();
        }

        private void UpdateTodayInfo()
        {
            txtTodayMonth.SetText(currentDate.ToString("M ", lang));
            txtTodayDay.SetText(currentDate.ToString("d ", lang));
            txtTodayWeek.SetText(currentDate.ToString("dddd", lang));
        }

        /// <summary>
        /// 生成行事曆
        /// </summary>
        void GenerateCalendar()
        {
            // 根據當前語言設置月份與年份顯示
            txtMonth.text = currentDate.ToString("MMMM", lang);
            txtYear.text = currentDate.Year.ToString();

            // 清空先前生成的天數
            foreach (Transform child in calendarGrid)
            {
                Destroy(child.gameObject);
            }
            dayItemList.Clear();

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
            dayItem.isCurrentSelectedMonth = date.Month == currentDate.Month;

            dayItem.isInclude = IsDateInRange(date);
            dayItem.isSelectedDate = (date == startDate || date == endDate);

            if (startDate.HasValue && endDate.HasValue && startDate != endDate)
            {
                if (date == startDate) dayItem.isStartDate = true;
                else dayItem.isEndDate = (date.Date == endDate.Value.Date);
            }

            // 添加點擊事件
            dayItem.onClickEvent.AddListener(OnDayClicked);

            if (dateWithData != null) dayItem.isHaveData = dateWithData.Contains(date);

            dayItemList.Add(dayItem);
        }

        /// <summary>
        /// 處理日期被點擊事件
        /// </summary>
        void OnDayClicked(DayItem dayItem)
        {
            switch (selectStyle)
            {
                case EnumSelectStyle.單一日期: SelectSingleDayHandler(dayItem); break;
                case EnumSelectStyle.起迄日期: SelectRangeDateHandler(dayItem); break;
            }
            // 更新行事曆以顯示新的範圍
            GenerateCalendar();
        }

        /// <summary>
        /// 選取單一日期處理
        /// </summary>
        private void SelectSingleDayHandler(DayItem dayItem)
        {
            endDate = null;
            startDate = dayItem.date;
            result = (DateTime)startDate;
            onSelectedDateEvent.Invoke(result);
            onSelectedDateStrEvent.Invoke(result.ToString(DateTimeHandler.FullDateFormat));
            onSelectedEndDateStrEvent.Invoke("");
        }

        /// <summary>
        /// 選取起迄日期處理
        /// </summary>
        private void SelectRangeDateHandler(DayItem dayItem)
        {
            if (startDate == null) // 如果尚未選擇起始日期
            {
                startDate = dayItem.date;
                Debug.Log($"選擇起始日期: {startDate}");
                result = (DateTime)startDate;
                onSelectedStartDateEvent.Invoke(result);
                onSelectedStartDateStrEvent.Invoke(result.ToString(DateTimeHandler.FullDateFormat));
            }
            else if (endDate == null && (isAllowSelectSameDay || dayItem.date != startDate)) // 已選擇起始日期但尚未選擇結束日期
            {
                endDate = dayItem.date.AddDays(1).AddTicks(-1); //設定為23:59:59
                Debug.Log($"選擇結束日期: {endDate}");
                result = (DateTime)endDate;
                onSelectedEndDateEvent.Invoke(result);
                onSelectedEndDateStrEvent.Invoke(result.ToString(DateTimeHandler.FullDateFormat));

                // 確保結束日期大於起始日期
                if (endDate < startDate)
                {
                    DateTime temp = startDate.Value;
                    startDate = endDate;
                    endDate = temp.AddDays(1).AddTicks(-1); //設定為23:59:59;

                    result = (DateTime)startDate;
                    onSelectedStartDateEvent.Invoke(result);
                    onSelectedStartDateStrEvent.Invoke(result.ToString(DateTimeHandler.FullDateFormat));

                    result = (DateTime)endDate;
                    onSelectedEndDateEvent.Invoke(result);
                    onSelectedEndDateStrEvent.Invoke(result.ToString(DateTimeHandler.FullDateFormat));
                }

                Debug.Log($"範圍：{startDate} 到 {endDate}");
                onSelectedDateRangeEvent.Invoke((DateTime)startDate, (DateTime)endDate);
            }
            else
            {
                // 如果已經選擇了起始和結束日期，則重新選擇，從頭開始
                startDate = dayItem.date;
                endDate = null;
                result = (DateTime)startDate;
                onSelectedStartDateEvent.Invoke(result);
                onSelectedStartDateStrEvent.Invoke(result.ToString(DateTimeHandler.FullDateFormat));
                onSelectedEndDateStrEvent.Invoke("");
                Debug.Log($"重新選擇起始日期: {startDate}");
            }
        }

        /// <summary>
        /// 判斷日期是否在選擇的範圍內
        /// </summary>
        bool IsDateInRange(DateTime date)
        {
            if (startDate != null && endDate != null)
            {
                return date > startDate && date < endDate;
            }
            return false;
        }

        /// <summary>
        /// 切換到上個月
        /// </summary>
        public void PreviousMonth()
        {
            currentDate = currentDate.AddMonths(-1); // 日期退一個月
            GenerateCalendar();
        }

        /// <summary>
        /// 切換到下個月
        /// </summary>
        public void NextMonth()
        {
            currentDate = currentDate.AddMonths(1); // 日期加一個月
            GenerateCalendar();
        }

        /// <summary>
        /// 切換到上一年
        /// </summary>
        public void PreviousYear()
        {
            currentDate = currentDate.AddYears(-1); // 日期退一年
            GenerateCalendar();
        }

        /// <summary>
        /// 切換到下一年
        /// </summary>
        public void NextYear()
        {
            currentDate = currentDate.AddYears(1); // 日期加一年
            GenerateCalendar();
        }

        /// <summary>
        /// 語言變更時的處理
        /// </summary>
        public void ChangeLanguage(EnumLanguage lang)
        {
            enumLang = lang;
            UpdateTodayInfo();
            GenerateCalendar();
        }

        [ContextMenu("- 清除選取日期")]
        public void ClearSelectDate()
        {
            startDate = null; endDate = null;
            if (currentDate != default) GenerateCalendar();
        }
    }
}
