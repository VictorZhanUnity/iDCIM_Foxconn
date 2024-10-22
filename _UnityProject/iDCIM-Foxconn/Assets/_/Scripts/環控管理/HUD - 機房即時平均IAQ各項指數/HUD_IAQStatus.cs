using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// IAQ���Ƹ�T��ܾ�
/// <para>+ ���F��ܸ�T�A�٭n���ܹϥ��C��</para>
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
