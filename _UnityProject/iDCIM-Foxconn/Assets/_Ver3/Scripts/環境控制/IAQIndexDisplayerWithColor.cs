using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using VictorDev.iDCIM;

/// IAQ單項指數顯示器
public class IAQIndexDisplayerWithColor : BlackboxDataDisplayer
{
    public override void ReceiveData(List<Data_Blackbox> datas)
    {
        base.ReceiveData(datas);
        ColorSet colorSet = iDCIM_ColorSetting.ColorSet_RT.FirstOrDefault(setting => value >= setting.threshold);
        colorSet ??= iDCIM_ColorSetting.ColorSet_RT.Last();
        TxtValue.DOColor(colorSet.color, 0.3f);
    }
}