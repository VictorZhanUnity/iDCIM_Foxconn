using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using XCharts.Runtime;

public class IAQ_LineChartPanel : MonoBehaviour
{
    [Header(">>> IAQ 資料欄位名稱")]
    [SerializeField] private string columnName = "RT";

    [Header(">>> [資料項]")]
    [SerializeField] private List<KeyValueData> historyData;

    [Space(20)]
    [SerializeField] private UIManager_IAQ uiManager_IAQ;
    [SerializeField] private LineChart lineChart;


    private void OnEnable() => WebAPI_GetTodayIAQData();

    public void WebAPI_GetTodayIAQData()
    {
        // 取得今天日期的凌晨12點 (00:00:00)
        DateTime startOfDay = DateTime.Today;
        // 取得今天日期的晚上11點59分59秒
        DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
        WebAPI_GetIAQData(startOfDay, endOfDay);
    }

    private void WebAPI_GetIAQData(DateTime startOfDay, DateTime endOfDay)
    {
        void onSuccess(long responseCode, string jsonString)
        {
            historyData = JsonConvert.DeserializeObject<List<KeyValueData>>(jsonString);

            Dictionary<DateTime, float> avgValues = historyData
                .SelectMany(item => item.value) // 將所有 DataPoint 展開為單一序列
                .GroupBy(dp => dp.Timestamp)// 依照 timestamp 分組
                .OrderBy(group => group.Key) //排序
                .ToDictionary(
                    group => group.Key,             // 使用 timestamp 作為鍵
                    group => group.Average(dp => dp.value) // 將同一 timestamp 的值取平均作為值
                );
            //設置LineChart圖表
            Data_IAQ.SetChart(lineChart, avgValues, columnName, false);
        }
        List<string> tagList = new List<string>();
        uiManager_IAQ.deviceModelVisualizer.ModelNameList.ForEach(name => tagList.Add(name + "/RT"));
        WebAPIManager.GetIAQIndexHistory(tagList, startOfDay, endOfDay, onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string jsonString)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (DateTimeHandler.isNowOnTheHour) WebAPI_GetTodayIAQData();
    }

    [Serializable]
    public class DataPoint
    {
        public string timestamp;
        public bool isNumeric;
        public bool isArray;
        public float value;

        public DateTime Timestamp => DateTime.Parse(timestamp);
    }

    [Serializable]
    public class KeyValueData
    {
        public string key;
        public List<DataPoint> value;
    }
}
