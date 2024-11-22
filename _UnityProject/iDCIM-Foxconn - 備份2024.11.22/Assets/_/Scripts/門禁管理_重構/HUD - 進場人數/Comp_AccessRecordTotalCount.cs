using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// [組件] 今年、當月、今日進場總人數 
/// </summary>
public class Comp_AccessRecordTotalCount : MonoBehaviour
{
    [Header(">>> [資料項]")]
    [SerializeField] private List<Data_AccessRecord> dataOfThisYear;

    [Header(">>> 組件")]
    [SerializeField] private TextMeshProUGUI txtYear, txtMonth, txtToday;

    private void Awake()
    {
        txtYear.SetText("0");
        txtMonth.SetText("0");
        txtToday.SetText("0");
    }

    public void ShowData(List<Data_AccessRecord> dataList)
    {
        dataOfThisYear = dataList;

        float delay = Random.Range(0, 0.5f);

        DotweenHandler.ToBlink(txtYear, dataOfThisYear.Sum(data => data.listOfThisYear.users.Count).ToString());
        DotweenHandler.ToBlink(txtMonth, dataOfThisYear.Sum(data => data.listOfThisMonth.users.Count).ToString(), 0.1f, delay);
        DotweenHandler.ToBlink(txtToday, dataOfThisYear.Sum(data => data.listOfToday.users.Count).ToString(), 0.1f, delay * 2);
    }
}
