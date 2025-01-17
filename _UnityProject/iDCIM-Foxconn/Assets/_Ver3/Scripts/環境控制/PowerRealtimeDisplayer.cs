﻿using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.ColorUtils;
using VictorDev.Common;

/// 市電、UPS、PDU電力顯示器
public class PowerRealtimeDisplayer : BlackboxDataDisplayer
{
    [Header(">>> 當警報狀態有改變時Invoke{是否有警報}")]
    public UnityEvent<bool> onAlarmStatusChanged = new UnityEvent<bool>();

    [Header(">>> 文字組件，組件名稱需為TagName")]
    [SerializeField] protected List<TextMeshProUGUI> txtCompList;

    /// 目前警報的狀態
    private bool isCurrentHaveAlarm { get; set; } = false;

    /// 警報時文字組件的顏色
    protected Color alarmTextColor => ColorHandler.HexToColor(0xFF3636);
    /// 警報時背景漸層的顏色
    public override void ReceiveData(List<Data_Blackbox> blackBoxData)
    {
        base.ReceiveData(blackBoxData);
        UpdateUI();
    }

    /// 更新UI 
    protected virtual void UpdateUI()
    {
        //將資料依Status與Value進行分組
        List<Data_Blackbox> statusList = DataList.Where(data => data.tagName.Contains("Status")).ToList();
        List<Data_Blackbox> valueList = DataList.Where(data => data.tagName.Contains("Value")).ToList();

        bool isHaveAlarm = false;
        txtCompList.ForEach(txt =>
        {
            string keyword = txt.name.Trim();
            bool isAlarm = statusList.FirstOrDefault(data => data.tagName.Contains(keyword)).alarm != null;
            float value = (valueList.FirstOrDefault(data => data.tagName.Contains(keyword)).value ?? 2);
            //設定值
            if (float.Parse(txt.text.Trim()) != value)
            {
                DotweenHandler.ToBlink(txt, value.ToString("0.##"), 0.05f, 0.2f, true);
                //設定文字顏色(是否有警報)
                txt.DOColor((isAlarm) ? alarmTextColor : Color.white, 0.2f).SetEase(Ease.OutQuad);
            }
            if (isAlarm) isHaveAlarm = true;
        });

        if (isCurrentHaveAlarm != isHaveAlarm)
        {
            isCurrentHaveAlarm = isHaveAlarm;
            onAlarmStatusChanged?.Invoke(isCurrentHaveAlarm);
        }
    }

    private void Awake()
    {
        txtCompList.ForEach(txt => txt.SetText("0"));
    }
}
