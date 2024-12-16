using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.iDCIM;

/// <summary>
/// IAQ單項指數顯示器
/// </summary>
public class IAQIndexDisplayer_RT : BlackboxDataDisplayer
{
    public override void ReceiveData(List<Data_Blackbox> datas)
    {
        base.ReceiveData(datas);
        ColorSet colorSet = iDCIM_ColorSetting.ColorSet_RT.FirstOrDefault(setting => value >= setting.threshold);
        colorSet ??= iDCIM_ColorSetting.ColorSet_RT.Last();
        txtValue.DOColor(colorSet.color, 0.3f);
    }
}

