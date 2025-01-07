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
using static AlarmHistoryDataManager;

/// [Component] - 警報記錄表格
public class AlarmHistoryTableList : MonoBehaviour, IAlarmHistoryDataReceiver
{
    [Header(">>> [Event] - 當項目被點擊時Invoke")]
    public UnityEvent<Data_AlarmHistoryData> onItemClicked = new();

    [Header(">>> [Prefab] - 列表項目")] [SerializeField]
    private Button itemPrefab;

    public void ReceiveData(List<Data_AlarmHistoryData> datas)
    {
        _sourceDatas = datas;
        FiltData();
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
        int currentYear  = int.Parse(DropdownYear.options[DropdownYear.value].text.Replace("年",""));
        int currentMonth = EnumHandler.GetValueWithEnumString<DateTimeHandler.EnumMonthName_ZH>(DropdownMonth.options[DropdownMonth.value].text);
        //先轉化JSON再解析，以避免變動到原始資料
        string jsonString = JsonConvert.SerializeObject(filterData);
        filterData = JsonConvert.DeserializeObject<List<Data_AlarmHistoryData>>(jsonString);
        filterData.ForEach(data =>
        {
            data.alarms = data.alarms.Where(alarm =>
                DateTimeHandler.IsDateInMonth(alarm.AlarmTime, currentMonth, currentYear)).ToList();
        });
        //只取有Alarm的資料
        filterData = filterData.Where(data=>data.alarms.Count>0).ToList();
       
        UpdateUI();
    }
    /// 更新UI
    private void UpdateUI()
    { 
        //清除
        foreach (Transform child in scrollRect.content)
        {
            Destroy(child.gameObject);
        }
        
        filterData.SelectMany(data=>data.alarms).OrderByDescending(alarm=> alarm.AlarmTime).ToList().ForEach(alarm=>
        {
            Button item = Instantiate(itemPrefab, scrollRect.content);
            item.onClick.AddListener(()=>onItemClicked?.Invoke(filterData[0]));
            item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(alarm.AlarmTime.ToString());
            item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(Config_iDCIM.GetAlarmSystem(filterData[0].tagName));
            item.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(filterData[0].tagName);
            item.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(alarm.alarmMessage);
        });
    }

    #region [Initialize]
    private void OnEnable()
    {
        DropdownYear.onValueChanged.AddListener((index)=>FiltData());
        DropdownMonth.onValueChanged.AddListener((index)=>FiltData());
        InputKeyword.onValueChanged.AddListener((keyword)=>FiltData());
    }
    private void OnDisable()
    {
        DropdownYear.onValueChanged.RemoveAllListeners();
        DropdownMonth.onValueChanged.RemoveAllListeners();
        InputKeyword.onValueChanged.RemoveAllListeners();
    }
    #endregion

    #region [Components]

    private List<Data_AlarmHistoryData> _sourceDatas;

    [Header("[資料項] - 過濾告警記錄")] [SerializeField]
    private List<Data_AlarmHistoryData> filterData;
    
    private Transform Container => _container ??= transform.Find("Container");
    private Transform _container;
    
    private ScrollRect scrollRect => _scrollRect ??= Container.Find("Table表格").Find("ScrollView滑動列表").GetComponent<ScrollRect>();
    private ScrollRect _scrollRect;

    private TMP_Dropdown DropdownYear => _dropdownYear ??= Container.Find("自動調整年份Dropdown").GetComponent<TMP_Dropdown>();
    private TMP_Dropdown _dropdownYear;

    private TMP_Dropdown DropdownMonth => _dropdownMonth ??= Container.Find("自動調整月份Dropdown").GetComponent<TMP_Dropdown>();
    private TMP_Dropdown _dropdownMonth;

    private TMP_InputField InputKeyword => _inputKeyword ??= Container.Find("文字輸入框 - 關鍵字搜尋").GetComponent<TMP_InputField>();
    private TMP_InputField _inputKeyword;

    #endregion
}