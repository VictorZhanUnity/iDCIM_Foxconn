using System;
using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using _VictorDEV.Revit;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.Common;
using static AlarmHistoryDataManager;

/// [Component] - 警報記錄表格
public class AlarmHistoryTableList : MonoBehaviour, IAlarmHistoryDataReceiver
{
    [Header(">>> [Event] - 當項目被點擊時Invoke")]
    public UnityEvent<Data_Blackbox.Alarm> onItemClicked = new();

    [Header(">>> [Prefab] - 列表項目")] [SerializeField]
    private Button itemPrefab;

    public AlarmHistoryDataManager manager;
    
    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        _sourceDatas = datas;
        FiltData();
    }

    /// 取得指定年度的記錄
    private void GetRecordOfYear()
    {
        manager.GetAlarmRecordOfYear(SelectedYear, ReceiveData);
    }


    /// 依條件過濾資料
    private void FiltData()
    {
        //先過濾關鍵字
        string currentKeyword = InputKeyword.text.Trim();
        filterData = string.IsNullOrEmpty(currentKeyword) == false
            ? _sourceDatas.Where(data => data.tagName.Contains(currentKeyword)).ToList()
            : _sourceDatas;

        //再過濾日期時間，有符合的才留下AlarmHistoryData
        int currentYear = int.Parse(DropdownYear.options[DropdownYear.value].text.Replace("年", ""));
        int currentMonth =
            EnumHandler.GetValueWithEnumString<DateTimeHandler.EnumMonthName_ZH>(DropdownMonth
                .options[DropdownMonth.value].text);
        //先轉化JSON再解析，以避免變動到原始資料
        string jsonString = JsonConvert.SerializeObject(filterData);
        filterData = JsonConvert.DeserializeObject<List<Data_AlarmHistoryData>>(jsonString);
        filterData.ForEach(data =>
        {
            data.alarms = data.alarms.Where(alarm =>
                DateTimeHandler.IsDateInMonth(alarm.AlarmTime, currentMonth, currentYear)).ToList();
        });
        //只取有Alarm的資料
        filterData = filterData.Where(data => data.alarms.Count > 0).ToList();

        SortingData("警報時間");
    }
    

    /// 依欄位排序
    public void SortingData(string columnName, bool isDescending = true)
    {
        dataSet = filterData.SelectMany(data => data.alarms.Select(alarm =>
            new DataSet()
            {
                alarm = alarm,
                historyData = data,
                setting = Config_iDCIM.GetAlarmSystemSetting(data.tagName),
            }));

        if (isDescending)
        {
            switch (columnName)
            {
                case "系統分類": dataSet = dataSet.OrderByDescending(data => data.setting.label); break;
                case "警報時間": dataSet = dataSet.OrderByDescending(data => data.alarm.alarmTime); break;
                case "設備名稱": dataSet = dataSet.OrderByDescending(data => data.historyData.tagName); break;
                case "警報描述": dataSet = dataSet.OrderByDescending(data => data.alarm.alarmMessage); break;
            }
        }
        else
        {
            switch (columnName)
            {
                case "系統分類": dataSet = dataSet.OrderBy(data => data.setting.label); break;
                case "警報時間": dataSet = dataSet.OrderBy(data => data.alarm.alarmTime); break;
                case "設備名稱": dataSet = dataSet.OrderBy(data => data.historyData.tagName); break;
                case "警報描述": dataSet = dataSet.OrderBy(data => data.alarm.alarmMessage); break;
            }
        }
        UpdateUI();
    }
    
    /// 更新UI
    private void UpdateUI()
    {
        //清除項目
        foreach (Transform child in scrollRect.content)
        {
            Destroy(child.gameObject);
        }
        scrollRect.verticalNormalizedPosition = 1;

        //創建項目
        dataSet.ToList().ForEach(data =>
        {
            Button item = Instantiate(itemPrefab, scrollRect.content);
            item.onClick.AddListener(() => onItemClicked.Invoke(data.alarm));
            item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(data.setting.label);
            item.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = data.setting.icon;
            item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(data.alarm.AlarmTime.ToString());
            item.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(data.historyData.tagName);
            item.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(data.alarm.alarmMessage);
        });
        
        //設定告警則數
        DotweenHandler.DoInt(txtAmount, 0, dataSet.Count());
    }

    public struct DataSet
    {
        public Data_AlarmHistoryData historyData;
        public Data_Blackbox.Alarm alarm;
        public Config_iDCIM.AlarmSystemSetting setting;
    }
    
    #region [Initialize]

    private void OnEnable()
    {
        txtAmount.text = "0";
        scrollRect.verticalNormalizedPosition = 1;
        
        DropdownYear.onValueChanged.AddListener((index) => GetRecordOfYear());
        DropdownMonth.onValueChanged.AddListener((index) => FiltData());
        InputKeyword.onValueChanged.AddListener((keyword) => FiltData());
    }

    private void OnDisable()
    {
        //清除項目
        foreach (Transform child in scrollRect.content)
        {
            Destroy(child.gameObject);
        }
        scrollRect.verticalNormalizedPosition = 1;
        
        DropdownYear.onValueChanged.RemoveAllListeners();
        DropdownMonth.onValueChanged.RemoveAllListeners();
        InputKeyword.onValueChanged.RemoveAllListeners();
    }
    #endregion

    #region [Components]

    private int SelectedYear =>int.Parse(DropdownYear.options[DropdownYear.value].text.Trim().Replace("年", ""));
    
    private List<Data_AlarmHistoryData> _sourceDatas;

    [Header("[資料項] - 過濾告警記錄")]
    private List<Data_AlarmHistoryData> filterData;

    private IEnumerable<DataSet> dataSet;

    private Transform Container => _container ??= transform.Find("Container");
    private Transform _container;

    private ScrollRect scrollRect =>
        _scrollRect ??= Container.Find("Table表格").Find("ScrollView滑動列表").GetComponent<ScrollRect>();

    private ScrollRect _scrollRect;

    private TMP_Dropdown DropdownYear =>
        _dropdownYear ??= Container.Find("自動調整年份Dropdown").GetComponent<TMP_Dropdown>();

    private TMP_Dropdown _dropdownYear;

    private TMP_Dropdown DropdownMonth =>
        _dropdownMonth ??= Container.Find("自動調整月份Dropdown").GetComponent<TMP_Dropdown>();

    private TMP_Dropdown _dropdownMonth;

    private TMP_InputField InputKeyword =>
        _inputKeyword ??= Container.Find("文字輸入框 - 關鍵字搜尋").GetComponent<TMP_InputField>();

    private TMP_InputField _inputKeyword;

    private TextMeshProUGUI txtAmount => _txtAmount ??= Container.Find("txtAmount").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtAmount;

    #endregion
}