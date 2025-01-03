using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.ColorUtils;
using VictorDev.Common;

namespace VictorDev.iDCIM
{
    public class iDCIM_ColorSetting : SingletonMonoBehaviour<iDCIM_ColorSetting>
    {
        /// <summary>
        /// 溫度顏色門檻(由門檻高至低排列)
        /// <para>+ if(value 大於 threshold_1)  SetColor_1... </para>
        /// <para>+ else if(value 大於 threshold_2)  SetColor_2... </para>
        /// </summary>
        public static List<ColorSet> ColorSet_RT => Instance._colorSet_RT;
        [Header(">>> 溫度顏色門檻(由門檻高至低排列)")]
        public List<ColorSet> _colorSet_RT = new List<ColorSet>()
        {
            new ColorSet(32, ColorHandler.HexToColor(0xffa681)),
            new ColorSet(27, ColorHandler.HexToColor(0xeca02d)),
            new ColorSet(18, ColorHandler.HexToColor(0xaac5e0)),
        };
    }

    [Serializable]
    public class ColorSet
    {
        public float threshold;
        public Color color = Color.white;
        public ColorSet(float threshold, Color color)
        {
            this.threshold = threshold;
            this.color = color;
        }
    }
}