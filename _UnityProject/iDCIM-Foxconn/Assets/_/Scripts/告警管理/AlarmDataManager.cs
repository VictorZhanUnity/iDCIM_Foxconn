using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using static Data_Blackbox;

/// <summary>
/// �iĵ�޲z
/// </summary>
public class AlarmDataManager : Module
{
    [Header(">>> [Receiver] - ��Ʊ�����")]
    [SerializeField] private List<MonoBehaviour> receivers;

    [Header(">>> [��ƶ�] - �iĵ�O��")]
    [SerializeField] private List<Data_AlarmRecord> datas;

    [Header(">>> [WebAPI] - ���oĵ�i�ƶq")]
    [SerializeField] private WebAPI_Request request;

    public override void OnInit(Action onInitComplete = null)
    {
        onInitComplete?.Invoke();
    }

    [ContextMenu("- ���o���~��ĵ�i")]
    private void GetAlarmsOfThisYear()
    {
        WebAPI_LoginManager.CheckToken(request);
        WebAPI_Caller.SendRequest(request, onSuccessHandler, null);
        void onSuccessHandler(long responseCode, string jsonData)
        {
         
        }
    }

    /// <summary>
    /// �ѪRJSON���
    /// </summary>
    public void ParseJson(string jsonData)
    {
        datas = JsonConvert.DeserializeObject<List<Data_AlarmRecord>>(jsonData);

   

        //�o�e���
        receivers.OfType<IAlarmReceiver>().ToList().ForEach(receiver => receiver.ReceiveData(datas));
    }


    private void OnValidate()
    {
        receivers = ObjectHandler.CheckTypoOfList<IAlarmReceiver>(receivers);
    }

    public interface IAlarmReceiver
    {
        void ReceiveData(List<Data_AlarmRecord> datas);
    }



    [Serializable]
    public class Data_AlarmRecord
    {
        public string tagName;
        public Alarm alarm;
    }
}
