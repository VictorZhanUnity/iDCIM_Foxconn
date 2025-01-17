using System;
using System.Collections.Generic;
using _VictorDEV.DateTimeUtils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;

public class AccessRecordDataManager : Module
{
    public bool isGenerateDemoData = false;
    
    [Header(">>> [接收器] AccessRecordDataReceiver")]
    [SerializeField] private List<AccessRecordDataReceiver> receivers;

    [Header(">>> [Event] 取得今年門禁資料Invoke")]
    public UnityEvent<DataAccessRecord> onGetAccessRecordOfThisYear = new UnityEvent<DataAccessRecord>();

    [Header(">>> [資料項]")]
    [SerializeField] private DataAccessRecord dataAccessRecord;

    [Header(">>> [WebAPI] - [Door文件] 查詢門禁進出記錄，需在機房測試")]
    [SerializeField] private WebAPI_Request request;

    private Action onInitComplete { get; set; }

    public override void OnInit(Action onInitComplete = null)
    {
        this.onInitComplete = onInitComplete;
    }

    [ContextMenu("- 取得今年門禁資料")]
    public void GetAccessRecordsOfThisYear()
    {
        if (isGenerateDemoData)
        {
            GetComponent<DemoDataHandlerAccessRecord>().GenerateDemoData(DateTime.Today.Year, onSuccessHandler);
        }
        else
        {
            DateTime from = new DateTime(DateTime.Today.Year, 1, 1);
            DateTime to = from.AddYears(1).AddDays(-1);
           
            GetAccessRecordsFromTimeInterval(from, to, onSuccessHandler);
        }
        return;
        
        
        void onSuccessHandler(string jsonString)
        {
            Parse(jsonString);
            
            receivers.ForEach(target => target.ReceiveData(dataAccessRecord));
            onGetAccessRecordOfThisYear?.Invoke(dataAccessRecord);
        }
    }

    private void Parse(string jsonString)
    {
        dataAccessRecord = JsonConvert.DeserializeObject<DataAccessRecord>(jsonString);
    }

    public void GetAccessRecordsFromTimeInterval(DateTime from, DateTime to, Action<string> onSuccess, Action<long, string> onFailed = null)
    {
        string jsonString = request.BodyJSON;
        SendRawJSON_AccessRecord data = JsonConvert.DeserializeObject<SendRawJSON_AccessRecord>(jsonString);
        data.filter.from = from.ToString(DateTimeHandler.FullDateTimeFormatWithT);
        data.filter.to = to.ToString(DateTimeHandler.FullDateTimeFormatWithT);
        request.SetRawJsonData(JsonConvert.SerializeObject(data));
        WebAPI_LoginManager.CheckToken(request);
        WebAPI_Caller.SendRequest(request, (responseCode, jsonString)=>onSuccess.Invoke(jsonString), onFailed);
    }

    #region[Components]
    #endregion
    
    #region [>>> 傳送RawJSON資料格式]
    [Serializable]
    public class SendRawJSON_AccessRecord
    {
        public int page = 0;
        public int pageItemCount = 1000;
        public Filter filter;

        [Serializable]
        public class Filter
        {
            //鴻騰機房入口門編號為10
            public string doorId = "10";
            public string from;
            public string to;
        }
    }
    #endregion
}
