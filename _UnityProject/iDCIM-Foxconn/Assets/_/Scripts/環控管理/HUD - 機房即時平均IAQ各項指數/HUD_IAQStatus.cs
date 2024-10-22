using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// IAQ指數資訊顯示器
/// <para>+ 除了顯示資訊，還要改變圖示顏色</para>
/// </summary>
public class HUD_IAQStatus : IAQIndexDisplayer
{
    [SerializeField] private List<Image> imgList;
    [SerializeField] private TextMeshProUGUI txtStatus;

    public override Data_IAQ data
    {
        set
        {
            base.data = value;
            imgICON.DOColor(value.levelColor, 1f);
            imgList.ForEach(img=>img.DOColor(value.levelColor, 2f));
            txtStatus.SetText(value.levelStatus);
            txtStatus.DOColor(value.levelColor, 1.5f);
        }
    }
}
