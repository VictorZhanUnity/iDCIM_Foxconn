using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCostController : MonoBehaviour
{
    [Header(">>> 今日能耗 - 資料項目 <<<")]
    [Header(">>>標題文字")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Color circleColor = Color.blue;
    [SerializeField] private string titleText = "項目標題";

    [Header(">>>UI組件")]
    [SerializeField] private Image imgCircle;
    [SerializeField] private Image imgIcon, imgLastYear, imgLastMonth;
    [SerializeField] private TextMeshProUGUI txtTitle, txtValue, txtLastYear, txtLastMonth;

    public string Title => txtTitle.text;

    [ContextMenu("- OnValidate")]
    private void OnValidate()
    {
        imgCircle ??= transform.Find("imgCircle").GetComponent<Image>();
        imgIcon ??= imgCircle.transform.Find("imgIcon").GetComponent<Image>();
        txtTitle ??= transform.Find("txtTitle").GetComponent<TextMeshProUGUI>();
        txtValue ??= transform.Find("txtValue").GetComponent<TextMeshProUGUI>();
        txtLastYear ??= transform.GetChild(3).Find("txtLastYear").GetComponent<TextMeshProUGUI>();
        imgLastYear ??= transform.GetChild(3).Find("imgLastYear").GetComponent<Image>();
        txtLastMonth ??= transform.GetChild(4).Find("txtLastMonth").GetComponent<TextMeshProUGUI>();
        imgLastMonth ??= transform.GetChild(4).transform.Find("imgLastMonth").GetComponent<Image>();

        RefreshData();
    }

    private void RefreshData()
    {
        imgCircle.color = circleColor;
        imgIcon.sprite = icon;
        txtTitle.SetText(titleText);
        txtValue.color = circleColor;

        name = titleText;
    }
}
