using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using _VictorDEV.DateTimeUtils;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using static AccessRecord_DataHandler;
using static AccessRecord_DataHandler.SendRawJSON;

public class AccessRecordDataManager : Module
{
    [Header(">>> [���U��] �����줵�~�ת��T��Ƶo�e���U�ﹳ�ե�")]
    [SerializeField] private List<AccessRecordDataReceiver> receivers;

    [Header(">>> [Event] �����줵�~�ת��T��Ʈ�Invoke")]
    public UnityEvent<List<Data_AccessRecord_Ver2>> onGetAccessRecordOfThisYear = new UnityEvent<List<Data_AccessRecord_Ver2>>();

    [Header(">>> [��ƶ�] - �ثe�d�ߪ��T���")]
    [SerializeField] private List<Data_AccessRecord_Ver2> datas;

    [Header(">>> [WebAPI] - �d�ߪ��T�O��")]
    [SerializeField] private WebAPI_Request request;

    private Action onInitComplete { get; set; }

    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
        GetAccessRecordsOfThisYear();
    }

    [ContextMenu("- ���o���~�ת��T�O��")]
    /// <summary>
    /// ���o���~�ת��T�O��
    /// </summary>
    private void GetAccessRecordsOfThisYear()
    {
        DateTime from = new DateTime(today.Year, 1, 1);
        DateTime to = from.AddYears(1).AddDays(-1);
        void onSuccess(List<Data_AccessRecord_Ver2> result)
        {
            datas = result;
            receivers.ForEach(target => target.ReceiveData(datas));
            onGetAccessRecordOfThisYear?.Invoke(datas);

            onInitComplete?.Invoke();
        }
        GetAccessRecordsFromTimeInterval(from, to, onSuccess, null);
    }

    /// <summary>
    /// ���o�Y�@�ɬq�����T�O��
    /// </summary>
    public void GetAccessRecordsFromTimeInterval(DateTime from, DateTime to, Action<List<Data_AccessRecord_Ver2>> onSuccess, Action<long, string> onFailed)
    {
        //�]�w�ǰe���
        sendData = new SendRawJSON()
        {
            filter = new Filter()
            {
                from = from.ToString(DateTimeHandler.FullDateTimeFormatWithT),
                to = to.ToString(DateTimeHandler.FullDateTimeFormatWithT),
            }
        };
        request.SetRawJsonData(JsonConvert.SerializeObject(datas));

        void onSuccessHandler(long responseCode, string jsonString)
        {
            Data_AccessRecord_Ver2 result = JsonConvert.DeserializeObject<Data_AccessRecord_Ver2>(jsonString);
            datas = new List<Data_AccessRecord_Ver2> { result };
            onSuccess?.Invoke(datas);
        }

#if UNITY_EDITOR
        onSuccessHandler(200, DataForDemo.AccessRecord);
#else
        WebAPI_LoginManager.CheckToken(request);
        WebAPI_Caller.SendRequest(request, onSuccessHandler, onFailed);
#endif
    }

    #region[Components]
    private SendRawJSON sendData { get; set; }
    private DateTime today => DateTime.Today;
    #endregion
}
