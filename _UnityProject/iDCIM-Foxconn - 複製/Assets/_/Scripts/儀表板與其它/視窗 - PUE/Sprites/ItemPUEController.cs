using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPUEController : MonoBehaviour
{
    [Header(">>> PUE - 資料項目 <<<")]
    [Header(">>>標題文字")]
    [SerializeField] private Sprite icon;
    [SerializeField] private string titleText = "項目標題";
    [SerializeField] private Color circleColor = Color.blue;

    [Header(">>>UI組件")]
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtValue, txtRatio;
    [SerializeField] private Image imgIcon, imgBar;

    public string Title => txtTitle.text;

    [ContextMenu("- OnValidate")]
    private void OnValidate()
    {
        txtTitle ??= transform.Find("txtTitle").GetComponent<TextMeshProUGUI>();
        imgIcon ??= txtTitle.transform.Find("imgIcon").GetComponent<Image>();

        imgBar ??= transform.Find("icon數值").GetComponent<Image>();
        txtValue ??= imgBar.transform.Find("txtValue").GetComponent<TextMeshProUGUI>();
        txtRatio ??= transform.GetChild(2).Find("txtRatio").GetComponent<TextMeshProUGUI>();

        RefreshData();
    }

    private void RefreshData()
    {
        txtTitle.SetText(titleText);
        imgIcon.sprite = icon;
        imgBar.color = circleColor;
    }
}
