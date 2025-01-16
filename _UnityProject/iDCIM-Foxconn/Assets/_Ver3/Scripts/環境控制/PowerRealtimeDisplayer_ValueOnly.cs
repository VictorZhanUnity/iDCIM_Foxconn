using System;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using VictorDev.Common;
using Debug = UnityEngine.Debug;

/// 市電、UPS、PDU電力顯示器
public class PowerRealtimeDisplayer_ValueOnly : PowerRealtimeDisplayer
{
    private TextMeshProUGUI firstTxt => txtCompList[0];

    /// 更新UI
    override protected void UpdateUI()
    {
        //將資料依Status與Value進行分組
        string keyword = firstTxt.name.Trim();
        float value = (DataList.FirstOrDefault(data => data.tagName.Contains(keyword)).value ?? 2);
        //設定值
        DotweenHandler.ToBlink(firstTxt, value.ToString("0.##"), 0.1f, 0.2f, true);
        ProgressBar.value = value;

        
        Vector3 endValue = new Vector3(0f, 0f, (value)/5 * -270f);
        //旋轉Pin 0~270
        ImgSliderBar.DORotate(endValue, 0.2f).SetEase(Ease.OutQuad);
    }

    #region [Initialize]

    private void Start()
    {
        ProgressBar.value = 0;
    }        
    #endregion


    #region [Components]

    private ProgressBarController ProgressBar => _progressBarController ??=
        transform.Find("Container").Find("SpeedMeter_PUE").GetComponent<ProgressBarController>();

    private ProgressBarController _progressBarController;

    private RectTransform ImgSliderBar => _imgSliderBar ??=
        ProgressBar.transform.Find("Slider").Find("ImagePin").GetComponent<RectTransform>();

    private RectTransform _imgSliderBar;

    #endregion
}