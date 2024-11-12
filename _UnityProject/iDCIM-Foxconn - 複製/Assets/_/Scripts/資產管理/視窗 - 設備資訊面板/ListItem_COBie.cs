using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RevitUtils;

public class ListItem_COBie : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTitle, txtLabel;
    [SerializeField] private GameObject txtEmpty;

    private List<TextMeshProUGUI> txtList { get; set; }

    public void ShowData(string columnName, string value)
    {
        columnName = RevitHandler.GetCOBieColumnName_ZH(columnName);
        txtTitle.SetText(columnName);

        txtEmpty.gameObject.SetActive(string.IsNullOrEmpty(value));
        txtLabel.SetText(value);

        txtList ??= new List<TextMeshProUGUI>() { txtTitle, txtLabel };
        DotweenHandler.ToBlink(txtList);
    }
}
