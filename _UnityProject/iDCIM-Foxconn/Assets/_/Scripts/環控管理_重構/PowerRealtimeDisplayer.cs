using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.Common;
 
/// <summary>
/// 市電、UPS、PDU電力顯示器
/// </summary>
public class PowerRealtimeDisplayer : BlackboxDataDisplayer
{
    [Header(">>> 文字組件，組件名稱需為TagName")]
    [SerializeField] protected List<TextMeshProUGUI> txtCompList;

    [Header(">>> 警報時文字組件的顏色")]
    [SerializeField] protected Color alarmColor = ColorHandler.HexToColor(0xFF8237);

    public override void ReceiveData(List<Data_Blackbox> blackBoxData)
    {
        base.ReceiveData(blackBoxData);
        UpdateUI();
    }

    /// <summary>
    /// 更新UI 
    /// </summary>
    protected virtual void UpdateUI()
    {
        //將資料依Status與Value進行分組
        List<Data_Blackbox> statusList = datas.Where(data => data.tagName.Contains("Status")).ToList();
        List<Data_Blackbox> valueList = datas.Where(data => data.tagName.Contains("Value")).ToList();

        txtCompList.ForEach(txt =>
        {
            string keyword = txt.name.Trim();
            bool isAlarm = statusList.FirstOrDefault(data => data.tagName.Contains(keyword)).alarm != null;
            float value = (valueList.FirstOrDefault(data => data.tagName.Contains(keyword)).value ?? 2);
            //設定值
            if(float.Parse(txt.text.Trim()) != value)
            {
                DotweenHandler.ToBlink(txt, value.ToString("0.##"), 0.05f, 0.2f, true);
                //設定文字顏色(是否有警報)
                txt.DOColor((isAlarm) ? alarmColor : Color.white, 0.1f).SetEase(Ease.OutQuad);
            }
        });
    }
}

