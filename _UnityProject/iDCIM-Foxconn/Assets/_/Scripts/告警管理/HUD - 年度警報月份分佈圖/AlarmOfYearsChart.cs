using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.XChart;
using TMPro;
using UnityEngine;
using static AlarmHistoryDataManager;

public class AlarmOfYearsChart : MonoBehaviour, IAlarmHistoryDataReceiver
{
    public AlarmHistoryDataManager manager;
    
    public void ReceiveData(List<Data_AlarmHistoryData> sourceDatas)
    {
        _dataOfMonths= sourceDatas;
         amountOfMonths = _dataOfMonths.SelectMany(data => data.alarms).GroupBy(alarm => alarm.AlarmTime.Month)
            .Select(group => group.Count()).ToList();
        LineChartHandlerInstance.SetSeriaDatas(1, amountOfMonths);
    }

    private void GetRecordOfSelectYear()
    {
        manager.GetAlarmRecordOfYear(SelectedYear, onSuccess, null);
        return;

        void onSuccess(List<Data_AlarmHistoryData> recordData)
        {
            _dataOfSelectYear = recordData;
            FilterSelectYearAlarmDataHandler();
        }
    }
    

    /// 依月份群組化資料
    private void FilterSelectYearAlarmDataHandler()
    {
        List<int> amountOfSelectedYearMonths = _dataOfSelectYear.SelectMany(data => data.alarms).GroupBy(alarm => alarm.AlarmTime.Month)
            .Select(group => group.Count()).ToList();
        
        int maxValue = amountOfSelectedYearMonths.Concat(amountOfMonths).Max() + 3;
        LineChartHandlerInstance.SetYAxisMaxMin(maxValue);
        
        LineChartHandlerInstance.SetSeriaDatas(0, amountOfSelectedYearMonths);
    }

    #region [Initialize]
    private void Awake()
    {
        _thisYear = DateTime.Now.Year;
        TxtThisYear.SetText(_thisYear.ToString());
    }

    private void OnEnable()
    {
        DropDownYears.onValueChanged.AddListener((index) => GetRecordOfSelectYear());
        DropDownYears.onValueChanged.Invoke(0);
    }

    private void OnDisable()
    {
        DropDownYears.onValueChanged.RemoveListener((index)=>GetRecordOfSelectYear());
        DropDownYears.value = 0;
    }
    #endregion

    #region [Components]
    /// 依年份分群組 {年份，告警資料清單}
    private List<Data_AlarmHistoryData> _dataOfMonths;
    
    private List<Data_AlarmHistoryData> _dataOfSelectYear = new List<Data_AlarmHistoryData>();
    /// 今年告警資料
    private List<int> amountOfMonths; 
    /// 今年
    private int _thisYear;
    /// Dropdown目前所選年份
    private int SelectedYear =>int.Parse(DropDownYears.options[DropDownYears.value].text.Trim());
    private Transform Container => _container ??= transform.Find("Container");
    private Transform _container;
    private LineChartHandler LineChartHandlerInstance => _lineChartHandler ??= Container.Find("LineChart圖表(設定好DataZoom) - 月份").GetComponent<LineChartHandler>();
    private LineChartHandler _lineChartHandler;
    private TextMeshProUGUI TxtThisYear => _txtThisYear ??= Container.Find("txt今年").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtThisYear;
    private TMP_Dropdown DropDownYears => _dropDownYears ??= Container.Find("Dropdown歷史年度").GetComponent<TMP_Dropdown>();
    private TMP_Dropdown _dropDownYears;
    #endregion
}