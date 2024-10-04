using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.DateTimeUtils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Clock : MonoBehaviour
    {
        [Header(">>> 是否為24小時制")]
        [SerializeField] private bool is24Hrs = true;

        [Header(">>> 是否顯示秒數")]
        [SerializeField] private bool isShowSec = false;

        [Header(">>> AM/PM語言")]
        [SerializeField] private enumLang lang = enumLang.英文;

        [Header(">>> Invoke 年/月/日")]
        public UnityEvent<string> onSetDateEvent = new UnityEvent<string>();
        [Header(">>> Invoke 星期幾")]
        public UnityEvent<string> onSetDayEvent = new UnityEvent<string>();

        [Space(10)]
        [SerializeField] private TextMeshProUGUI txtTime;

        private CultureInfo cultureInfo { get; set; }
        private bool jumper = true;

        private string langFormat
        {
            get
            {
                switch (lang)
                {
                    case enumLang.中文: return "zh-CN";
                    case enumLang.英文: return "en-US";
                }
                return "";
            }
        }

        private void Start()
        {
            IEnumerator enumerator()
            {
                while (true)
                {
                    UpdateClock();
                    yield return new WaitForSeconds(1f); // 每秒更新一次
                }
            }
            cultureInfo = new CultureInfo(langFormat);
            StartCoroutine(enumerator());
        }
        private void UpdateClock()
        {
            string symbol = jumper ? ":" : " ";
            string format = (is24Hrs) ? $"HH{symbol}mm" : $"tt hh{symbol}mm";
            if (isShowSec) format += $"{symbol}ss";

            DateTime dateTime = DateTime.Now;
            //dateTime = dateTime.AddDays(-50); //往回推 for Demo

            string time = dateTime.ToString(format, cultureInfo);
            string date = dateTime.ToString("yyyy/MM/dd", cultureInfo);
            string day = dateTime.ToString("ddd", cultureInfo);

            txtTime.SetText(time);
            onSetDateEvent?.Invoke(date);
            onSetDayEvent?.Invoke(day);
            jumper = !jumper;
        }

        private void OnValidate()
        {
            txtTime ??= GetComponent<TextMeshProUGUI>();
            if (txtTime != null)
            {
                cultureInfo = new CultureInfo(langFormat);
                UpdateClock();
            }
        }
        private enum enumLang { 中文, 英文 }
    }
}
