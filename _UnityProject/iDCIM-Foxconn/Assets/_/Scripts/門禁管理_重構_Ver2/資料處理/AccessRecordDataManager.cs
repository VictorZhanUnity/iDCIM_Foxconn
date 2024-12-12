using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;
using static AccessRecord_DataHandler;
using static AccessRecord_DataHandler.SendRawJSON;

public class AccessRecordDataManager : Module
{
    [Header(">>> [註冊器] 接收到今年度門禁資料發送給各對像組件")]
    [SerializeField] private List<AccessRecordDataReceiver> receivers;

    [Header(">>> [Event] 接收到今年度門禁資料時Invoke")]
    public UnityEvent<List<Data_AccessRecord_Ver2>> onGetAccessRecordOfThisYear = new UnityEvent<List<Data_AccessRecord_Ver2>>();

    [Header(">>> [資料項] - 目前查詢門禁資料")]
    [SerializeField] private List<Data_AccessRecord_Ver2> datas;

    [Header(">>> [WebAPI] - 查詢門禁記錄")]
    [SerializeField] private WebAPI_Request request;

    public override void OnInit(Action onInitComplete = null)
    {
        GetAccessRecordsOfThisYear();
        onInitComplete?.Invoke();
    }

    [ContextMenu("- 取得今年度門禁記錄")]
    /// <summary>
    /// 取得今年度門禁記錄
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
        }
        GetAccessRecordsFromTimeInterval(from, to, onSuccess, null);
    }

    /// <summary>
    /// 取得某一時段的門禁記錄
    /// </summary>
    public void GetAccessRecordsFromTimeInterval(DateTime from, DateTime to, Action<List<Data_AccessRecord_Ver2>> onSuccess, Action<long, string> onFailed)
    {
        //設定傳送資料
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
        WebAPI_Caller.SendRequest(requestAccessRecord, onSuccessHandler, onFailed);
#endif
    }

    #region[Components]
    private SendRawJSON sendData { get; set; }
    private DateTime today => DateTime.Today;
    #endregion
}
