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
/// IAQ��@���ƾ��v��T���O
/// </summary>
public class IAQ_IndexHistoryPanel : MonoBehaviour
{
    [Header(">>> [��ƶ�] IAQ��@����")]
    [SerializeField] private IAQIndexDisplayer indexDisplayer;
    public IAQIndexDisplayer dataDisplayer => indexDisplayer;
    private Data_IAQ.IAQ_DateFormat iaqDataFormat => Data_IAQ.UnitName[indexDisplayer.columnName];

    [Header(">>> [��ƶ�] ���v��Ƶ��G")]
    [SerializeField] private List<KeyValueData> historyData;

    [Header(">>> ��ƾ�ե�")]
    [SerializeField] private DropDownCalendar dropdownCalendar;

    [Header(">>> ���u�Ϫ�")]
    [SerializeField] private LineChart lineChart;

    [Header(">>> UI�ե�")]
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
    /// ��ܸ��
    /// </summary>
    public void ShowData(IAQIndexDisplayer item)
    {
        indexDisplayer = item;
        imgICON.sprite = indexDisplayer.imgICON_Sprite;

        string title = indexDisplayer.data.ModelID.Contains(",") ? "���Х����ƾ� - " : $"[{indexDisplayer.data.ModelID}] ";
        txtTitle.SetText(title + iaqDataFormat.columnName_ZH);

        DotweenHandler.ToBlink(txtTitle);
        fader.isOn = true;

        WebAPI_GetIAQHistoryData(dropdownCalendar.StartDateTime, dropdownCalendar.EndDateTime);
    }


    /// <summary>
    /// [WebAPI] ���oIAQ���ƾ��v��T
    /// </summary>
    private void WebAPI_GetIAQHistoryData(DateTime startTime, DateTime endTime)
    {
        void onSuccess(long responseCode, string jsonString)
        {
            print("�ѪRJSON���");
            // �ѪR JSON �r��
            historyData = JsonConvert.DeserializeObject<List<KeyValueData>>(jsonString);

            // �p�⥭����
            // �ϥ� LINQ �إ� Dictionary<string, float>�A�䬰 timestamp�A�Ȭ�������
            Dictionary<DateTime, float> avgValues = historyData
                .SelectMany(item => item.value) // �N�Ҧ� DataPoint �i�}����@�ǦC
                .GroupBy(dp => dp.Timestamp)// �̷� timestamp ����
                .OrderBy(group => group.Key) //�Ƨ�
                .ToDictionary(
                    group => group.Key,             // �ϥ� timestamp �@����
                    group => group.Average(dp => dp.value) // �N�P�@ timestamp ���Ȩ������@����
                );

            //�]�mLineChart�Ϫ�
            Data_IAQ.SetChart(lineChart, avgValues, indexDisplayer.columnName);

            //�M�����
            ObjectPoolManager.PushToPool<ListItem_IAQHistory>(scrollRect.content);

            avgValues.ToList().ForEach(keyPair =>
            {
                // �ͦ��C��
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
