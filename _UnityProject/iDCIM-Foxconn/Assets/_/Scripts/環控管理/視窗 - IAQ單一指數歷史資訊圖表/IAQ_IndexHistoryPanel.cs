using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.Calendar;
using XCharts.Runtime;

/// <summary>
/// IAQ單一指數歷史資訊面板
/// </summary>
public class IAQ_IndexHistoryPanel : MonoBehaviour
{
    [Header(">>> [資料項] IAQ單一指數")]
    [SerializeField] private IAQIndexDisplayer indexDisplayer;
    public IAQIndexDisplayer dataDisplayer => indexDisplayer;


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
        txtTitle.SetText(title + indexDisplayer.columnName);

        dropdownCalendar.SetDate_PastWeeks();

        fader.isOn = true;
    }

    /// <summary>
    /// [WebAPI] 取得IAQ指數歷史資訊
    /// </summary>
    private void WebAPI_GetIAQHistoryData(DateTime startTime, DateTime endTime)
    {
        print($"startTime: {startTime} / endTime: {endTime}");

        void onSuccess(long responseCode, string jsonString)
        {
            print("WebAPI_GetIAQHistoryData: " + jsonString);
        }
        WebAPIManager.GetIAQIndexHistory(indexDisplayer.key, startTime, endTime, onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string msg)
    {
        throw new NotImplementedException();
    }
}
