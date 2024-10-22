using System;
using TMPro;
using UnityEngine;
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


    [Header(">>> ��ƾ�ե�")]
    [SerializeField] private DropDownCalendar dropdownCalendar;

    [Header(">>> ���u�Ϫ�")]
    [SerializeField] private LineChart lineChart;

    [Header(">>> UI�ե�")]
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
    /// ��ܸ��
    /// </summary>
    public void ShowData(IAQIndexDisplayer item)
    {

        indexDisplayer = item;
        imgICON.sprite = indexDisplayer.imgICON_Sprite;
        string title = indexDisplayer.data.ModelID.Contains(",") ? "���Х����ƾ� - " : $"[{indexDisplayer.data.ModelID}] ";
        txtTitle.SetText(title + indexDisplayer.columnName);

        dropdownCalendar.SetDate_PastWeeks();

        fader.isOn = true;
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
        }
        WebAPIManager.GetIAQIndexHistory(indexDisplayer.key, startTime, endTime, onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string msg)
    {
        throw new NotImplementedException();
    }
}
