using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.XChart;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using static AlarmHistoryDataManager;
using Debug = VictorDev.Common.Debug;

public class AlarmOfYearsChart : MonoBehaviour, IAlarmHistoryDataReceiver
{
    public void ReceiveData(List<Data_AlarmHistoryData> sourceDatas)
    {
        List<float> alarmCount = new List<float>();
        List<int> amountOfMonths = sourceDatas.SelectMany(data => data.alarms).GroupBy(alarm => alarm.AlarmTime.Month)
            .Select(group => group.Count()).ToList();
        lineChartHandler.SetSeriaDatas(0, amountOfMonths);
 
        amountOfMonths.ForEach(value => Debug.Log(value.ToString()));
    }

    private void OnSelectYearChangedHandler(int selectIndex)
    {
        throw new NotImplementedException();
    }

    #region [Initialize]
    private void Awake()
    {
        _thisYear = DateTime.Now.Year;
        txtThisYear.SetText(_thisYear.ToString());
    }

    private void OnEnable()=> dropDownYears.onValueChanged.AddListener(OnSelectYearChangedHandler);
    private void OnDisable()
    {
        dropDownYears.onValueChanged.RemoveListener(OnSelectYearChangedHandler);
        dropDownYears.value = 0;
    }
    #endregion

    #region [Components]

    private int _thisYear;
    private Transform container => _container ??= transform.Find("Container");
    private Transform _container;
    private LineChartHandler lineChartHandler => _lineChartHandler ??= container.Find("LineChart圖表(設定好DataZoom) - 月份").GetComponent<LineChartHandler>();
    private LineChartHandler _lineChartHandler;
    private TextMeshProUGUI txtThisYear => _txtThisYear ??= container.Find("txt今年").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtThisYear;
    private TMP_Dropdown dropDownYears => _dropDownYears ??= container.Find("Dropdown歷史年度").GetComponent<TMP_Dropdown>();
    private TMP_Dropdown _dropDownYears;
    #endregion
}