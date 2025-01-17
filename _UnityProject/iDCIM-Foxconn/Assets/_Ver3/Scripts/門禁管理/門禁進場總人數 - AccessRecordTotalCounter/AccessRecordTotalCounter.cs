using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VictorDev.Common;
using static DataAccessRecord;

/// 門禁管理 - 各時段的進場人數
public class AccessRecordTotalCounter : AccessRecordDataReceiver
{
    [Header("[Event] - 點選各時段項目時Invoke用戶清單 {List<User>}")]
    public UnityEvent<List<User>> onItemClicked = new UnityEvent<List<User>>();

    public override void ReceiveData(DataAccessRecord data)
    {
        _thisYearList = data.pageData.users.Where(user => DateTimeHandler.isDateInThisYear(user.DateAccessTime)).ToList();
        _thisMonthList = _thisYearList.Where(user => DateTimeHandler.IsDateInThisMonth(user.DateAccessTime)).ToList();
        _todayList = _thisMonthList.Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).ToList();
        UpdateUI();
    }
    private void UpdateUI()
    {
        DotweenHandler.DoInt(TxtThisYear, 0, _thisYearList.Count);
        DotweenHandler.DoInt(TxtThisMonth, 0, _thisMonthList.Count);
        DotweenHandler.DoInt(TxtToday, 0, _todayList.Count);
    }
    
    #region [Initialize]
    private void OnEnable()
    {
        TxtThisYear.SetText("0");
        TxtThisMonth.SetText("0");
        TxtToday.SetText("0");
        BtnThisYear.onClick.AddListener(()=>onItemClicked?.Invoke(_thisYearList));
        BtnThisMonth.onClick.AddListener(()=>onItemClicked?.Invoke(_thisMonthList));
        BtnToday.onClick.AddListener(()=>onItemClicked?.Invoke(_todayList));
    }
    private void OnDisable()
    {
        BtnThisYear.onClick.RemoveAllListeners();
        BtnThisMonth.onClick.RemoveAllListeners();
        BtnToday.onClick.RemoveAllListeners();
    }
    #endregion
    
    #region [Components]
    [Header("[資料項] - 每時段的用戶清單")]
    private List<User> _thisYearList;
    private List<User> _thisMonthList;
    private List<User> _todayList;
    
    private Button BtnThisYear => _btnThisYear??= transform.Find("當年進場人數").GetComponent<Button>();
    private Button _btnThisYear;
    private Button BtnThisMonth => _btnThisMonth??= transform.Find("當月進場人數").GetComponent<Button>();
    private Button _btnThisMonth;
    private Button BtnToday => _btnToday??= transform.Find("今日進場人數").GetComponent<Button>();
    private Button _btnToday;
    private TextMeshProUGUI TxtThisYear => _txtThisYear??= BtnThisYear.transform.GetChild(0).Find("Container").Find("txt所選日期進場人數").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtThisYear;
    private TextMeshProUGUI TxtThisMonth => _txtThisMonth??= BtnThisMonth.transform.GetChild(0).Find("Container").Find("txt所選日期進場人數").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtThisMonth;
    private TextMeshProUGUI TxtToday => _txtToday??= BtnToday.transform.GetChild(0).Find("Container").Find("txt所選日期進場人數").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtToday;
    #endregion

    [ContextMenu("-隨機資料")]
    private void ContextTest()
    {
        DotweenHandler.DoInt(TxtThisYear, 0, Random.Range(100, 201));
        DotweenHandler.DoInt(TxtThisMonth, 0, Random.Range(50, 151));
        DotweenHandler.DoInt(TxtToday, 0, Random.Range(0, 21));
    }
}
