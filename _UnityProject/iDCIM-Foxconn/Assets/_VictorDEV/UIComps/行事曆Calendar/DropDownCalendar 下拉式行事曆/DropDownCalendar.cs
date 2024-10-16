using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.Calendar
{
    /// <summary>
    /// 下拉式行事曆
    /// </summary>
    public class DropDownCalendar : MonoBehaviour
    {
        [Header(">>> 選取區間日期時Invoke")]
        public UnityEvent<DateTime, DateTime> onSelectDateRange = new UnityEvent<DateTime, DateTime>();

        [SerializeField] private TextMeshProUGUI label;

        private DateTime startDateTime, endDateTime;
        private string startDateTimeStr;

        public string StartDateTimeStr
        {
            set
            {
                startDateTimeStr = value;
                label.SetText($"{startDateTimeStr}");
            }
        }

        public string EndDateTimeStr { set => label.SetText($"{startDateTimeStr} ~ {value}"); }

        public void SetDateTimeRange(DateTime startDate, DateTime endDate)
        {
            startDateTime = startDate;
            endDateTime = endDate;
            onSelectDateRange.Invoke(startDate, endDate);
        }
    }
}
