using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [Header(">>> 點擊關閉時Invoke")]
    public UnityEvent onCloseEvent = new UnityEvent();

    [Header(">>> 行事曆組件")]
    [SerializeField] private DropDownCalendar dropdownCalendar;

    [Header(">>> 直線圖表")]
    [SerializeField] private LineChart lineChart;

    [Header(">>> UI組件")]
    [SerializeField] private Image imgICON;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private DoTweenFadeController doTweenFade;
    [SerializeField] private RectTransformResizeLerp resizer;
    [SerializeField] private Toggle toggleContent;

    private Vector3 originalPos { get; set; }

    private void Start()
    {
        originalPos = doTweenFade.transform.position;
        doTweenFade.OnFadeOutEvent.AddListener(CloseHandler);
 /*       DateTime today = DateTime.Today;
        calendarManager.SetDateTimeRange(today.AddDays(-7), today);*/
        dropdownCalendar.onSelectedDateRangeEvent.AddListener(WebAPI_GetIAQHistoryData);
    }

    /// <summary>
    /// 顯示資料
    /// </summary>
    public void ShowData(IAQIndexDisplayer item)
    {
        doTweenFade.transform.position = originalPos;
        indexDisplayer = item;
        imgICON.sprite = indexDisplayer.imgICON_Sprite;
        string title = string.IsNullOrEmpty(indexDisplayer.data.ModelID) ? "機房平均數據 - " : $"[{indexDisplayer.data.ModelID}] ";
        txtTitle.SetText(title + indexDisplayer.columnName);

       // WebAPI_GetIAQHistoryData(dropdownCalendar.StartDateTime, dropdownCalendar.EndDateTime);
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
            doTweenFade.FadeIn(true);
        }
        WebAPIManager.GetIAQIndexHistory(indexDisplayer.key, startTime, endTime, onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string msg)
    {
        throw new NotImplementedException();
    }

    private void CloseHandler()
    {
        toggleContent.isOn = false;
        resizer.Restore();
        ObjectPoolManager.PushToPool<IAQ_IndexHistoryPanel>(this);
    }


    public void Close()
    {
        doTweenFade.FadeOut();
        onCloseEvent.Invoke();
    }
}
