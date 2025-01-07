using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace _VictorDEV.DateTimeUtils
{
    /// 自動調整Options內容至目前的年份
    [RequireComponent(typeof(TMP_Dropdown))]
    public class AutoAdjustYearDetector : MonoBehaviour
    {
        private TMP_Dropdown Dropdown => _dropdown ??= GetComponent<TMP_Dropdown>();
        private TMP_Dropdown _dropdown;
        private void Awake()
        {
            int optionYear = int.Parse(Dropdown.options[0].text.Replace("年", ""));
            int currentYear = DateTime.Now.Year;
            // 動態增加未來的年份
            for (int year = optionYear + 1; year <= currentYear; year++)
            {
                if (!Dropdown.options.Exists(option => option.text == year.ToString()))
                {
                    Dropdown.options.Add(new TMP_Dropdown.OptionData(year.ToString() + "年"));
                }
            }
            Dropdown.options = Dropdown.options.OrderByDescending(item => int.Parse(item.text.Replace("年", ""))).ToList();
            Dropdown.options = Dropdown.options.OrderByDescending(item => int.Parse(item.text.Replace("年", ""))).ToList();
        }

        private void OnDisable() => Dropdown.value = 0;
    }
}