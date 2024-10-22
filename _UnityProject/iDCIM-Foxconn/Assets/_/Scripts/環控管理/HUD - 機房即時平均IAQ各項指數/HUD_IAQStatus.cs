using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// IAQ指數資訊顯示器
/// <para>+ 除了顯示資訊，還要改變圖示顏色</para>
/// </summary>
public class HUD_IAQStatus : GridItem_IAQIndex
{
    [SerializeField] private Image imgGradient;
    [SerializeField] private TextMeshProUGUI txtStatus;

    public override Data_IAQ data
    {
        set
        {
            base.data = value;
            imgICON.DOColor(value.levelColor, 1f);
            imgGradient.DOColor(value.levelColor, 2f);
            txtStatus.SetText(value.levelStatus);
        }
    }
}
