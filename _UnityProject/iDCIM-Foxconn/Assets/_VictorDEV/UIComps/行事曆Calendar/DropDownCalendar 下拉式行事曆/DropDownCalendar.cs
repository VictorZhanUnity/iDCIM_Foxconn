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
            calendarManager.onSelectedDateRangeEvent.AddListener(onSelectedDateRangeEvent.Invoke);
        }

        public string EndDateTimeStr { set => label.SetText($"{startDateTimeStr} ~ {value}"); }

        public void SetDate_PastWeeks() => calendarManager.SetDate_PastWeeks();
    }
}
