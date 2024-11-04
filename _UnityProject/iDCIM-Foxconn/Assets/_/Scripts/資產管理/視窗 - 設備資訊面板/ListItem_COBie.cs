using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RevitUtils;

public class ListItem_COBie : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTitle, txtLabel;

    private List<TextMeshProUGUI> txtList { get; set; }

    public void ShowData(string columnName, string value)
    {
        columnName = RevitHandler.GetCOBieColumnName_ZH(columnName);
        txtTitle.SetText(columnName);
        txtLabel.SetText(value);

        txtList ??= new List<TextMeshProUGUI>() { txtTitle, txtLabel };
        DotweenHandler.ToBlink(txtList);
    }
}
