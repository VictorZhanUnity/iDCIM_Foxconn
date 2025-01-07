using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using static AlarmHistoryDataManager;

/// [Component] - 今日警報數量
public class AlarmTodayDisplayer : MonoBehaviour, IAlarmHistoryDataReceiver
{
    [Header(">>> [Receiver] - 接收器 - AlarmTypeDisplayer")] [SerializeField]
    private List<AlarmTypeDisplayer> receivers;

    [Header(">>> [Event] - 當分類項目被點擊時Invoke")]
    public UnityEvent<List<Data_AlarmHistoryData>> onItemClicked = new();

    /// 接收資料，需轉成JSON字串再重新解析成新的List，以避免變更到原始資料
    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        string jsonString = JsonConvert.SerializeObject(datas);
        filteData = JsonConvert.DeserializeObject<List<Data_AlarmHistoryData>>(jsonString);
        filteData.ForEach(data =>
        {
            data.alarms = data.alarms.Where(alarm =>
                DateTimeHandler.isDateInToday(DateTimeHandler.StrToLocalTime(alarm.alarmTime))).ToList();
        });
        InvokeData();
    }

    /// 發送資料
    private void InvokeData() => receivers.ForEach(receiver => receiver.ReceiveData(filteData));

    private void Awake()
    {
        onItemClicked.AddListener((datas) => Debug.Log(datas[0].tagName));
    }

    #region [Components]

    [Header("[資料項] - 已過濾為今日的告警記錄")] [SerializeField]
    private List<Data_AlarmHistoryData> filteData;

    #endregion

    #region [Initialize]

    private void OnEnable() => receivers.ForEach(receiver => receiver.onItemClicked.AddListener(onItemClicked.Invoke));
    private void OnDisable() => receivers.ForEach(receiver => receiver.onItemClicked.RemoveAllListeners());

    #endregion
}