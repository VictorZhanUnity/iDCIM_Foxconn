using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.XChart;
using TMPro;
using UnityEngine;
using static AlarmHistoryDataManager;

public class AlarmOfYearsChart : MonoBehaviour, IAlarmHistoryDataReceiver
{
    public void ReceiveData(List<Data_AlarmHistoryData> sourceDatas)
    {
        //先依照年份進行分組
        _dataOfYears = sourceDatas.SelectMany(data => data.alarms).GroupBy(alarm => alarm.AlarmTime.Year)
            .ToDictionary(group=>group.Key, group=>group.ToList());
        
        //今年資料
        _amountOfThisYearMonths = _dataOfYears[_thisYear].GroupBy(alarm => alarm.AlarmTime.Month)
            .Select(group => group.Count()).ToList();
        LineChartHandlerInstance.SetSeriaDatas(1, _amountOfThisYearMonths);

        FilterSelectYearAlarmDataHandler();
    }

    /// 篩選出Dropdown所選年份的資料
    private void FilterSelectYearAlarmDataHandler(int selectIndex=0)
    {
        List<int> amountOfSelectedYearMonths = _dataOfYears[SelectedYear].GroupBy(alarm => alarm.AlarmTime.Month)
            .Select(group => group.Count()).ToList();
        
        int maxValue = amountOfSelectedYearMonths.Concat(_amountOfThisYearMonths).Max() + 3;
        LineChartHandlerInstance.SetYAxisMaxMin(maxValue);
        
        LineChartHandlerInstance.SetSeriaDatas(0, amountOfSelectedYearMonths);
    }

    #region [Initialize]
    private void Awake()
    {
        _thisYear = DateTime.Now.Year;
        TxtThisYear.SetText(_thisYear.ToString());
    }

    private void OnEnable()=> DropDownYears.onValueChanged.AddListener(FilterSelectYearAlarmDataHandler);
    private void OnDisable()
    {
        DropDownYears.onValueChanged.RemoveListener(FilterSelectYearAlarmDataHandler);
        DropDownYears.value = 0;
    }
    #endregion

    #region [Components]
    /// 依年份分群組 {年份，告警資料清單}
    private Dictionary<int, List<Data_Blackbox.Alarm>> _dataOfYears;
    /// 今年告警資料
    private List<int> _amountOfThisYearMonths; 
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