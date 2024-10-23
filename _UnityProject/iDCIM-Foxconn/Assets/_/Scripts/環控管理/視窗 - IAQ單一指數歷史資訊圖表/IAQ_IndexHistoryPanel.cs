using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.Calendar;
using VictorDev.Common;
using XCharts.Runtime;

/// <summary>
/// IAQ單一指數歷史資訊面板
/// </summary>
public class IAQ_IndexHistoryPanel : MonoBehaviour
{
    [Header(">>> [資料項] IAQ單一指數")]
    [SerializeField] private IAQIndexDisplayer indexDisplayer;
    public IAQIndexDisplayer dataDisplayer => indexDisplayer;
    private Data_IAQ.IAQ_DateFormat iaqDataFormat => Data_IAQ.UnitName[indexDisplayer.columnName];

    [Header(">>> [資料項] 歷史資料結果")]
    [SerializeField] private List<KeyValueData> historyData;

    [Header(">>> 行事曆組件")]
    [SerializeField] private DropDownCalendar dropdownCalendar;

    [Header(">>> 直線圖表")]
    [SerializeField] private LineChart lineChart;

    [Header(">>> UI組件")]
    [SerializeField] private ListItem_IAQHistory listItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Image imgICON;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private AdvancedCanvasGroupFader fader;
    [SerializeField] private RectTransformResizeLerp resizer;
    [SerializeField] private Toggle toggleContent;

    private void Start()
    {
        dropdownCalendar.SetDate_PastWeeks();
        dropdownCalendar.onSelectedDateRangeEvent.AddListener(WebAPI_GetIAQHistoryData);
    }

    /// <summary>
    /// 顯示資料
    /// </summary>
    public void ShowData(IAQIndexDisplayer item)
    {
        indexDisplayer = item;
        imgICON.sprite = indexDisplayer.imgICON_Sprite;

        string title = indexDisplayer.data.ModelID.Contains(",") ? "機房平均數據 - " : $"[{indexDisplayer.data.ModelID}] ";
        txtTitle.SetText(title + iaqDataFormat.columnName_ZH);

        DotweenHandler.ToBlink(txtTitle);
        fader.isOn = true;

        WebAPI_GetIAQHistoryData(dropdownCalendar.StartDateTime, dropdownCalendar.EndDateTime);
    }


    /// <summary>
    /// [WebAPI] 取得IAQ指數歷史資訊
    /// </summary>
    private void WebAPI_GetIAQHistoryData(DateTime startTime, DateTime endTime)
    {
        print($"startTime: {startTime} / endTime: {endTime}");

        void onSuccess(long responseCode, string jsonString)
        {
            print("解析JSON資料");
            // 解析 JSON 字串
            historyData = JsonConvert.DeserializeObject<List<KeyValueData>>(jsonString);

            // 計算平均值
            // 使用 LINQ 建立 Dictionary<string, float>，鍵為 timestamp，值為平均值
            Dictionary<DateTime, float> avgValues = historyData
                .SelectMany(item => item.value) // 將所有 DataPoint 展開為單一序列
                .GroupBy(dp => dp.Timestamp)// 依照 timestamp 分組
                .OrderBy(group => group.Key) //排序
                .ToDictionary(
                    group => group.Key,             // 使用 timestamp 作為鍵
                    group => group.Average(dp => dp.value) // 將同一 timestamp 的值取平均作為值
                );

            //清除圖表與設置
            lineChart.series[0].data.Clear();
            XAxis xAxis = lineChart.EnsureChartComponent<XAxis>();
            xAxis.data.Clear();
            YAxis yAxis = lineChart.EnsureChartComponent<YAxis>();
            yAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            yAxis.min = iaqDataFormat.minValue;
            yAxis.max = iaqDataFormat.maxValue;
            yAxis.axisLabel.formatter = "{value}";
            lineChart.series[0].label.formatter = "{c} " + iaqDataFormat.unitName;
            Tooltip toolTip = lineChart.EnsureChartComponent<Tooltip>();
            toolTip.numericFormatter = "0.### " + iaqDataFormat.unitName;

            xAxis.refreshComponent();
            toolTip.refreshComponent();
            lineChart.series[0].label.show = avgValues.Count > 0;

            //清除表格
            ObjectPoolManager.PushToPool<ListItem_IAQHistory>(scrollRect.content);

            avgValues.ToList().ForEach(keyPair =>
            {
                //設定圖表
                if (lineChart.series[0].data.Count < 5)
                {
                    lineChart.AddData("", keyPair.Value);
                    string xKey = keyPair.Key.ToString(DateTimeFormatter.FullDateTimeMinuteFormat);
                    lineChart.AddXAxisData(xKey);
                }

                // 生成列表
                ListItem_IAQHistory item = ObjectPoolManager.GetInstanceFromQueuePool(listItemPrefab, scrollRect.content);
                item.iaqColumnName = indexDisplayer.columnName;
                item.ShowData(keyPair.Key, keyPair.Value);
            });
            scrollRect.verticalNormalizedPosition = 1;
        }
        WebAPIManager.GetIAQIndexHistory(indexDisplayer.key, startTime, endTime, onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string msg)
    {
        throw new NotImplementedException();
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
