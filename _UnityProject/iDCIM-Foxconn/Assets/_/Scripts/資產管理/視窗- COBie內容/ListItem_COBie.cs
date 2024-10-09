using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListItem_COBie : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTitle, txtLabel;

    internal void Show(KeyValuePair<string, string> data)
    {
        txtTitle.SetText(data.Key);
        txtLabel.SetText(data.Value);
    }
}
