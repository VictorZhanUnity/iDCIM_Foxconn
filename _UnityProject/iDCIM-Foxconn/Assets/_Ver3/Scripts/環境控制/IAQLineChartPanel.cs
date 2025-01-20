using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.XChart;
using UnityEngine;
using static WebApiGetHistoryData;

public class IAQLineChartPanel : MonoBehaviour, IReceiveTodayData
{
    [Header(">>> IAQ 資料欄位名稱")] [SerializeField]
    private string columnName = "RT";

    public void ReceiveTodayData(List<ReceiveJsonData> receivedData)
    {
        _receiveData = receivedData;
        UpdateUI();
    }

    private void UpdateUI()
    {
        List<ReceiveJsonData> dataList = _receiveData.Where(data => data.key.Contains(columnName, StringComparison.OrdinalIgnoreCase)).ToList();
        averageList =
            dataList.SelectMany(data => data.value).GroupBy(valData => DateTime.Parse(valData.timestamp))
                .OrderBy(data => data.Key)
                .Select(group => group.Average(valData => valData.value)).ToList();
        LineChart.SetSeriaDatas(0, averageList);
    }

    #region Components

    [Header(">>> [資料項]")] public List<ReceiveJsonData> _receiveData;

    public List<float> averageList;

    private LineChartHandler LineChart =>
        _lineChartHandler ??= transform.Find("LineChart圖表").GetComponent<LineChartHandler>();

    private LineChartHandler _lineChartHandler;

    #endregion
}