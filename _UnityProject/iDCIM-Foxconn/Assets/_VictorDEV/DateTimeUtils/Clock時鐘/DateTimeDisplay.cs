using System;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using static VictorDev.DateTimeUtils.Clock;

namespace VictorDev.DateTimeUtils
{
    /// <summary>
    /// 日期時間顯示器
    /// </summary>

    public class DateTimeDisplay : MonoBehaviour, IClockReceiver
    {
        [Header(">>> 日期時間字串格式 {MM/dd ddd} =>")]
        [SerializeField] private string format = "MM/dd ddd";

        [Header(">>> 是否為英文版")]
        [SerializeField] private bool isEng = true;

        [Header(">>> [組件]，若無指定則從本身擷取")]
        [SerializeField] private TextMeshProUGUI txt;

        public void OnReceive(DateTime dateNow)
        {
            txt ??= GetComponent<TextMeshProUGUI>();
            txt.SetText(dateNow.ToString(format, DateTimeHandler.GetCulture(isEng)));
        }
    }
}
