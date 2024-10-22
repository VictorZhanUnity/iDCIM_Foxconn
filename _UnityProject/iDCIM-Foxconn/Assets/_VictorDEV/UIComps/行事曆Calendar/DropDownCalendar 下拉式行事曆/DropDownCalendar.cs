using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

        private string startDateTimeStr;

        public string StartDateTimeStr
        {
            set
            { 
                startDateTimeStr = value;
                label.SetText($"{startDateTimeStr}");
            }
        }

        private void Start()
        {
           // SetDateTimeRange(DateTime.Today.AddDays(-7), DateTime.Today);
            calendarManager.onSelectedDateRangeEvent.AddListener(onSelectedDateRangeEvent.Invoke);
        }

        public string EndDateTimeStr { set => label.SetText($"{startDateTimeStr} ~ {value}"); }

        public void SetDateTimeRange(DateTime startDate, DateTime endDate) 
            => calendarManager.SetDateTimeRange(startDate, endDate);
    }
}
