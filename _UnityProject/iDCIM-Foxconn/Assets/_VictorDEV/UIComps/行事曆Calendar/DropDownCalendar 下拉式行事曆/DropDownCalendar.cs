using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;

namespace VictorDev.Calendar
{
    /// <summary>
    /// 下拉式行事曆
    /// </summary>
    public class DropDownCalendar : MonoBehaviour
    {
        [Header(">>> 選取區間日期時Invoke")]
        public UnityEvent<DateTime, DateTime> onSelectedDateRangeEvent = new UnityEvent<DateTime, DateTime>();

        [SerializeField] private CalendarManager calendarManager;
        [SerializeField] private TextMeshProUGUI label;

        public DateTime StartDateTime => calendarManager.StartDateTime;
        public DateTime EndDateTime => calendarManager.EndDateTime;

        private string startDateTimeStr { get; set; }

        /// <summary>
        /// [Inspector] 設定起始日期Label
        /// </summary>
        public string StartDateTimeStr
        {
            set
            {
                startDateTimeStr = value;
                label.SetText($"{startDateTimeStr}");
            }
        }
        /// <summary>
        /// [Inspector] 設定結束日期Label
        /// </summary>
        public string EndDateTimeStr { set => label.SetText($"{startDateTimeStr} ~ {value}"); }


        private void Start()
        {
            calendarManager.onSelectedDateRangeEvent.AddListener((startDate, endDate) =>
            {
                onSelectedDateRangeEvent.Invoke(startDate, endDate);
                DotweenHandler.ToBlink(label);
            });
        }

        /// <summary>
        /// 設定日期{過去N週}
        /// </summary>
        public void SetDate_PastWeeks(int pastWeek = 1) => calendarManager.SetDate_PastWeeks(pastWeek);
        /// <summary>
        /// 設定日期{過去N月}
        /// </summary>
        public void SetDate_PastMonths(int pastMonth = 1) => calendarManager.SetDate_PastMonths(pastMonth);
        /// <summary>
        /// 設定日期{過去N年}
        /// </summary>
        public void SetDate_PastYears(int pastYears = 1) => calendarManager.SetDate_PastYears(pastYears);
    }
}
