using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAQStatusDisplayer : GridItem_IAQIndex
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
