using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Net.WebAPI;
using static DataHandler_AccessRecord.SendRawJSON;

/// <summary>
/// [門禁管理] 資料處理
/// </summary>
[Serializable]
public class DataHandler_AccessRecord : MonoBehaviour
{
    [Header(">>> [資料項]")]
    [SerializeField] private List<Data_AccessRecord> _datas;
    public List<Data_AccessRecord> datas => _datas;

    [Header(">>> [WebAPI] 查詢門禁記錄")]
    [SerializeField] private WebAPI_Request requestAccessRecord;

    [Header(">>> [傳送資料項]")]
    [SerializeField] private SendRawJSON sendData;

    private DateTime today => DateTime.Today;


    /// <summary>
    /// [目前年份] 門禁記錄
    /// </summary>
    public void GetAccessRecordsOfThisYear(Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed)
    {
        DateTime from = new DateTime(today.Year, 1, 1);
        DateTime to = new DateTime(today.Year, 12, 31);
        GetAccessRecordsFromTimeInterval(from, to, onSuccess, onFailed);
    }

    /// <summary>
    /// [目前月份] 門禁記錄
    /// </summary>
    public void GetAccessRecordsOfThisMonth(Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed)
    {
        DateTime from = new DateTime(today.Year, today.Month, 1);
        DateTime to = from.AddMonths(1).AddDays(-1);
        GetAccessRecordsFromTimeInterval(from, to, onSuccess, onFailed);
    }

    /// <summary>
    /// [今天] 門禁記錄
    /// </summary>
    public void GetAccessRecordsOfToday(Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed) => GetAccessRecordsOfDay(today, onSuccess, onFailed);

    /// <summary>
    /// 取得某一天的門禁記錄
    /// </summary>
    public void GetAccessRecordsOfDay(DateTime day, Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed)
    {
        DateTime from = day.Date;
        DateTime to = day.Date.AddDays(1).AddMilliseconds(-1);
        GetAccessRecordsFromTimeInterval(from, to, onSuccess, onFailed);
    }
    /// <summary>
    /// 取得某一時段的門禁記錄
    /// </summary>
    public void GetAccessRecordsFromTimeInterval(DateTime from, DateTime to, Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed)
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
        requestAccessRecord.SetRawJsonData(JsonConvert.SerializeObject(datas));

        void onSuccessHandler(long responseCode, string jsonString)
        {
            Data_AccessRecord result = JsonConvert.DeserializeObject<Data_AccessRecord>(jsonString);
            _datas = new List<Data_AccessRecord> { result };
            onSuccess?.Invoke(_datas);
        }

#if UNITY_EDITOR
        onSuccessHandler(200, DataForDemo.AccessRecord);
#else
        WebAPI_Caller.SendRequest(requestAccessRecord, onSuccessHandler, onFailed);
#endif
    }

    #region [>>> 傳送RawJSON資料格式]
    [Serializable]
    public class SendRawJSON
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

    [ContextMenu("- [目前年份] 門禁記錄")] private void Test_GetYear() => GetAccessRecordsOfThisYear(null, null);
    [ContextMenu("- [目前月份] 門禁記錄")] private void Test_GetMonth() => GetAccessRecordsOfThisMonth(null, null);
    [ContextMenu("- [今天] 門禁記錄")] private void Test_GetToday() => GetAccessRecordsOfToday(null, null);
}