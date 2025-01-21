using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.Revit;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using Debug = VictorDev.Common.Debug;

public class AlarmHistoryManager : ModulePage, IIAQDataReceiver
{
    public UnityEvent onShowEvent = new UnityEvent();
    public UnityEvent<Data_Blackbox.Alarm> onClickAlarmMessage = new UnityEvent<Data_Blackbox.Alarm>();
    
    public void ReceiveData(List<Data_Blackbox> datas)
    {
      List<Data_Blackbox.Alarm> alarmList= datas.Where(data => data.tagName.Contains("Status", StringComparison.OrdinalIgnoreCase) && data.alarm != null)
           .Select(data=>data.alarm).ToList();
      
      alarmList.ForEach(alarmData =>
      {
          NotifyListItemTextMessage item = NotificationManager.CreateNotifyMessage(notifyItemPrefab);
          Config_iDCIM.AlarmSystemSetting setting = Config_iDCIM.GetAlarmSystemSetting(alarmData.tagName);
          item.Icon = setting.icon;
          item.ShowMessage(setting.label, alarmData.alarmMessage, ()=>
          {
              onClickAlarmMessage.Invoke(alarmData);
              item.Close();
          });
      });
    }


    #region Components

    [Header("[資料項]")] public List<Data_Blackbox> alarmData;
    [Header("[Prefab]")] [SerializeField] private NotifyListItemTextMessage notifyItemPrefab;
    #endregion
    
    public override void OnInit(Action onInitComplete = null)
    {
    }

    protected override void InitEventListener()
    {
    }

    protected override void OnCloseHandler()
    {
    }

    protected override void OnShowHandler()
    {
        
        onShowEvent.Invoke();
    }

    protected override void RemoveEventListener()
    {
    }


}