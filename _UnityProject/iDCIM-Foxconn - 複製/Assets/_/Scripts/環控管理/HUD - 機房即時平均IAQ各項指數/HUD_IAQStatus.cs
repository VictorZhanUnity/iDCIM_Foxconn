using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;

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
            
            ColorHandler.ChangeColorLevel_Temperature(data.RT, imgICON, 1f);
            imgList.ForEach(img => ColorHandler.ChangeColorLevel_Temperature(data.RT, txtValue, 0.5f));
           // txtStatus.SetText(value.levelStatus);
            //imgICON.DOColor(value.levelColor, 1f);
            //txtStatus.DOColor(value.levelColor, 1.5f);
            //txtValue.DOColor(value.levelColor, 0.5f);
        }
    }
}
