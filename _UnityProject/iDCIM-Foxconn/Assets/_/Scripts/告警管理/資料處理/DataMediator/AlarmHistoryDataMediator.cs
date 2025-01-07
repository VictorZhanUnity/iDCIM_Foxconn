using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Managers;
using static AlarmHistoryDataManager;
using Debug = VictorDev.Common.Debug;

/// <summary>
/// [轉接器] - 告警歷史資料分類處理
/// </summary>
public class AlarmHistoryDataMediator : Module, IAlarmHistoryDataReceiver
{
    [Header(">>> [Receiver] - 資料接收器 - IAlarmHistoryDataReceiver")]
    [SerializeField] private List<MonoBehaviour> receivers;

    [Header(">>> 分類關鍵字")]
    [SerializeField] private List<string> keywords;
    [Header(">>> 排除關鍵字")]
    [SerializeField] private List<string> keywords_Exclude;

    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        List<Data_AlarmHistoryData> filteDatas = datas.Where(data =>
            keywords.Any(keywords => data.tagName.Contains(keywords, StringComparison.OrdinalIgnoreCase))
            && keywords_Exclude.All(keywords => data.tagName.Contains(keywords, StringComparison.OrdinalIgnoreCase)) == false
            ).ToList();

        //當資料不同時才發送資料
        if (alarmDatas.Equals(filteDatas) == false)
        {
            alarmDatas = filteDatas;
            // 發送資料
            receivers.OfType<IAlarmHistoryDataReceiver>().ToList().ForEach(receiver => receiver.ReceiveData(alarmDatas));
        }
    }

    #region [Components]
    [Header(">>> [資料項]")]
    public List<Data_AlarmHistoryData> alarmDatas;
    #endregion

    #region [初始化]
    public override void OnInit(Action onInitComplete = null)
    {
        Debug.Log(">>> AlarmDataManager OnInit Complete.");
        onInitComplete?.Invoke();
    }
    private void OnValidate()
    {
        receivers = ObjectHandler.CheckTypoOfList<IAlarmHistoryDataReceiver>(receivers);
        if (keywords.Count > 0) name = $"({alarmDatas.Count}) {keywords.First()}_{GetType().Name}";
    }
    #endregion
}
