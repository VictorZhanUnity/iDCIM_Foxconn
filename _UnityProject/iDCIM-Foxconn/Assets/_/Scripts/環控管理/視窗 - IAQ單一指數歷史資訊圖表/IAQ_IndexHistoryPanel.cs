using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

    [Header(">>> [資料項] 歷史資料結果")]
    [SerializeField] private List<KeyValueData> historyData;

    [Header(">>> 行事曆組件")]
    [SerializeField] private DropDownCalendar dropdownCalendar;

    [Header(">>> 直線圖表")]
    [SerializeField] private LineChart lineChart;

    [Header(">>> UI組件")]
    [SerializeField] private Image imgICON;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private ScrollRect scrollRect;
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
        txtTitle.SetText(title + Data_IAQ.ColumnName[indexDisplayer.columnName]);

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
    }

    [Serializable]
    public class KeyValueData
    {
        public string key;
        public List<DataPoint> value;
    }
}
