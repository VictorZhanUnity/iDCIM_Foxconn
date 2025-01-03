using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.ColorUtils;
using VictorDev.Common;

public class ProgressBarController : MonoBehaviour
{
    [Header(">>> 目前值")]
    [SerializeField] private float currentValue = 0;
    [Header(">>>最大值")]
    [SerializeField] private int maxValue = 5000;

    public int MaxValue
    {
        set
        {
            maxValue = value;
            slider.maxValue = maxValue;
        }
    }

    [Header(">>> 單位文字")]
    [SerializeField] private string unitText = "w";

    [Header(">>> 是否顯示為整數")]
    [SerializeField] private bool isWholeNumber = true;
    [Header(">>> 是否顯示最大值")]
    [SerializeField] private bool isShowMaxValue = true;

    [Header(">>> UI組件")]
    [SerializeField] private Slider slider;
    [SerializeField] private Image imgSliderBar, icon;
    [SerializeField] private List<Image> imgList;
    [SerializeField] private TextMeshProUGUI txtPercent, txtValue;

    /// <summary>
    /// 目前百分比
    /// </summary>
    public float percentage => slider.value / slider.maxValue;

    /// <summary>
    /// 目前數值
    /// </summary>
    public float value
    {
        get => slider.value;
        set
        {
            slider.value = Mathf.Max(value, 0);
            currentValue = slider.value;

            Color color = ColorHandler.GetColorLevelFromPercentage(percentage);

            imgSliderBar.color = color;
            if (icon != null) icon.color = color;
            imgList.ForEach(img => img.color = color);

            txtValue.SetText($"{slider.value.ToString((isWholeNumber) ? "" : "F2")}");
            if (isShowMaxValue) txtValue.SetText($"{txtValue.text} / {slider.maxValue}");
            txtValue.SetText($"{txtValue.text} {unitText}");

            float fontSize = txtPercent.fontSize * 0.4f;
            fontSize = Mathf.Max((float)fontSize, 14);
            txtPercent.SetText($"{Mathf.FloorToInt(percentage * 100)}{StringHandler.SetFontSizeString("%", (int)fontSize)}");
        }
    }

    private void OnValidate()
    {
        slider ??= transform.Find("Slider").GetComponent<Slider>();
        imgSliderBar ??= slider.transform.Find("imgSliderBar").GetComponent<Image>();
        txtPercent ??= slider.transform.Find("txtPercent").GetComponent<TextMeshProUGUI>();
        txtValue ??= slider.transform.Find("txtValue").GetComponent<TextMeshProUGUI>();

        slider.maxValue = maxValue;
        slider.wholeNumbers = isWholeNumber;
        value = currentValue;
    }
}
