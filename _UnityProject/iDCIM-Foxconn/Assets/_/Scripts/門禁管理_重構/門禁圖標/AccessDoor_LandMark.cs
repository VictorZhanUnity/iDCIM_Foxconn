using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.Common;

public class AccessDoor_LandMark : LandmarkHandler<Data_AccessRecord>
{
    [SerializeField] private TextMeshProUGUI txtIdNumber, txtAmountofEntry;

    

    protected override void OnShowDataHandler(Data_AccessRecord data)
    {
        txtIdNumber.SetText("10");
        txtAmountofEntry.SetText(data.pageData.ChartData.Sum(chart => chart.Total).ToString() + "¤H");
    }

    protected override void OnToggleOnHandler()
    {
    }
    protected override void OnToggleOffHandler()
    {
    }
}
