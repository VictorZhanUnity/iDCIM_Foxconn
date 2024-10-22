using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.Calendar;
using XCharts.Runtime;

/// <summary>
/// IAQ��@���ƾ��v��T���O
/// </summary>
public class IAQ_IndexHistoryPanel : MonoBehaviour
{
    [Header(">>> [��ƶ�] IAQ��@����")]
    [SerializeField] private IAQIndexDisplayer indexDisplayer;
    public IAQIndexDisplayer dataDisplayer => indexDisplayer;

    [Header(">>> �I��������Invoke")]
    public UnityEvent onCloseEvent = new UnityEvent();

    [Header(">>> ��ƾ�ե�")]
    [SerializeField] private DropDownCalendar dropdownCalendar;

    [Header(">>> ���u�Ϫ�")]
    [SerializeField] private LineChart lineChart;

    [Header(">>> UI�ե�")]
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
    /// ��ܸ��
    /// </summary>
    public void ShowData(IAQIndexDisplayer item)
    {
        doTweenFade.transform.position = originalPos;
        indexDisplayer = item;
        imgICON.sprite = indexDisplayer.imgICON_Sprite;
        string title = string.IsNullOrEmpty(indexDisplayer.data.ModelID) ? "���Х����ƾ� - " : $"[{indexDisplayer.data.ModelID}] ";
        txtTitle.SetText(title + indexDisplayer.columnName);

       // WebAPI_GetIAQHistoryData(dropdownCalendar.StartDateTime, dropdownCalendar.EndDateTime);
    }


    /// <summary>
    /// [WebAPI] ���oIAQ���ƾ��v��T
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
