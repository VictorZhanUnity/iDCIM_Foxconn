using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.Net.WebAPI;

/// <summary>
/// [門禁管理] 資料處理
/// </summary>
[Serializable]
public class AccessRecord_DataHandler : MonoBehaviour
{
    [Header(">>> [資料項] - 今年度門禁資料")]
    [SerializeField] private List<Data_AccessRecord> _datas;
    public List<Data_AccessRecord> datas => _datas;

    [Header(">>> [Event] - 擷取到今年度門禁資料時Invoke")]
    public UnityEvent<List<Data_AccessRecord>> onGetAccessRecordOfThisYear = new UnityEvent<List<Data_AccessRecord>>();

    [Header(">>> [WebAPI] - 查詢門禁記錄")]
    [SerializeField] private WebAPI_Request requestAccessRecord;

    public Data_AccessRecord GetDataByDevicePath(string targetModelName)
         => _datas.FirstOrDefault(data => targetModelName.Contains(data.DevicePath));

    /// [目前年份] 門禁記錄
    public void GetAccessRecordsOfThisYear(Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed)
    {
        DateTime from = new DateTime(DateTime.Today.Year, 1, 1);
        DateTime to = from.AddYears(1);
        onSuccess += (dataList) => onGetAccessRecordOfThisYear.Invoke(dataList);
        GetAccessRecordsFromTimeInterval(from, to, onSuccess, onFailed);
    }

    /// <summary>
    /// [目前月份] 門禁記錄
    /// </summary>
    public void GetAccessRecordsOfThisMonth(Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed)
    {
        DateTime from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime to = from.AddMonths(1).AddDays(-1);
        GetAccessRecordsFromTimeInterval(from, to, onSuccess, onFailed);
    }

    /// <summary>
    /// [今天] 門禁記錄
    /// </summary>
    public void GetAccessRecordsOfToday(Action<List<Data_AccessRecord>> onSuccess, Action<long, string> onFailed) => GetAccessRecordsOfDay(DateTime.Today,onSuccess, onFailed);

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
        /*
        //設定傳送資料
        sendData = new SendRawJSON()
        {
            filter = new Filter()
            {
                from = from.ToString(DateTimeHandler.FullDateTimeFormatWithT),
                to = to.ToString(DateTimeHandler.FullDateTimeFormatWithT),
            }
        };
        requestAccessRecord.SetRawJsonData(JsonConvert.SerializeObject(sendData));
        */

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

    

    [ContextMenu("- [目前年份] 門禁記錄")] private void Test_GetYear() => GetAccessRecordsOfThisYear(null, null);
    [ContextMenu("- [目前月份] 門禁記錄")] private void Test_GetMonth() => GetAccessRecordsOfThisMonth(null, null);
    [ContextMenu("- [今天] 門禁記錄")] private void Test_GetToday() => GetAccessRecordsOfToday(null, null);
}