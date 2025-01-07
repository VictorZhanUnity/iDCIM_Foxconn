using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using static AlarmHistoryDataManager;

public class AlarmTodayDisplayer : MonoBehaviour, IAlarmHistoryDataReceiver
{
    [Header(">>> [Receiver] - 資料接收器 - IAlarmHistoryDataReceiver")]
    [SerializeField] private List<MonoBehaviour> receivers;

    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        datas.ForEach(data =>
        {
            data.alarms = data.alarms.Where(alarm => DateTimeHandler.isDateInToday(DateTimeHandler.StrToLocalTime(alarm.alarmTime))).ToList();
        });
        this.datas = datas;
    }

    private void OnValidate() => receivers = ObjectHandler.CheckTypoOfList<IAlarmHistoryDataReceiver>(receivers);

    #region [Components]
    public List<Data_AlarmHistoryData> datas;
    #endregion
}
