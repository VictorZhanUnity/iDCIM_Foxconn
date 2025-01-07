using System.Collections.Generic;
using System.Linq;
using _VictorDEV.DateTimeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;
using static Data_AccessRecord_Ver2;

/// <summary>
/// HUD - ���T�i���`�H��
/// </summary>
public class HUD_AccessRecordTotalCounter : AccessRecordDataReceiver
{
    [Header(">>> [��ƶ�] ���~���T�O��")]
    [SerializeField] private List<User> thisYearList;
    [SerializeField] private List<User> thisMonthList;
    [SerializeField] private List<User> todayList;

    [Header(">>> [Event]  �I�����@���خ�Invoke")]
    public UnityEvent<List<User>> onClickButton = new UnityEvent<List<User>>();

    public override void ReceiveData(List<Data_AccessRecord_Ver2> datas)
    {
        //�̤���i�����
        thisYearList = datas.SelectMany(data => data.pageData.users)
            .Where(user => DateTimeHandler.isDateInThisYear(user.DateAccessTime)).ToList();
        thisMonthList = thisYearList.Where(user => DateTimeHandler.IsDateInThisMonth(user.DateAccessTime)).ToList();
        todayList = thisMonthList.Where(user => DateTimeHandler.isDateInToday(user.DateAccessTime)).ToList();
        UpdateUI();
    }
    private void UpdateUI()
    {
        DotweenHandler.ToBlink(txtThisYear, thisYearList.Count.ToString(), 0.1f, 0.1f);
        DotweenHandler.ToBlink(txtThisMonth, thisMonthList.Count.ToString(), 0.1f, 0.2f);
        DotweenHandler.ToBlink(txtToday, todayList.Count.ToString(), 0.1f, 0.3f);
    }
    private void OnEnable()
    {
        btnThisYear.onClick.AddListener(() => onClickButton?.Invoke(thisYearList));
        btnThisMonth.onClick.AddListener(() => onClickButton?.Invoke(thisMonthList));
        btnToday.onClick.AddListener(() => onClickButton?.Invoke(todayList));
        UpdateUI();
    }

    private void OnDisable()
    {
        btnThisYear.onClick.RemoveAllListeners();
        btnThisMonth.onClick.RemoveAllListeners();
        btnToday.onClick.RemoveAllListeners();
    }

    #region [Components]
    [Header(">>> [�ե�]")]
    [SerializeField] private TextMeshProUGUI txtThisYear;
    [SerializeField] private TextMeshProUGUI txtThisMonth;
    [SerializeField] private TextMeshProUGUI txtToday;
    [SerializeField] private Button btnThisYear, btnThisMonth, btnToday;
    #endregion
}
