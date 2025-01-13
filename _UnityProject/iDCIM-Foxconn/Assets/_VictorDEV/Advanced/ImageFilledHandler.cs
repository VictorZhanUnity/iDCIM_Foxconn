using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.ColorUtils;

namespace VictorDev.Advanced
{
    public class ImageFilledHandler : Image
    {
        [Header(">>>顏色排序(依百分比從小到大, 若無顏色則不進行變化)")]
        [SerializeField]
        private List<Color> colorLevels = new List<Color>()
    {
        ColorHandler.HexToColor(0x7BFF69), ColorHandler.HexToColor(0xFFF532), ColorHandler.HexToColor(0xEF701A), ColorHandler.HexToColor(0x640000)
    };

        private float duration { get; set; } = 0.2f;
        private float segment { get; set; }

        /// <summary>
        /// 設定FillAmount百分比 0~1
        /// </summary>
        public void DoTween_FillAmount(float percent01)
        {
            this.DOFillAmount(percent01, duration).SetEase(Ease.OutQuad);

            if (colorLevels.Count > 0)
            {
                this.DOColor(GetColorByPercent(percent01), duration).SetEase(Ease.OutQuad);
            }
        }

        private Color GetColorByPercent(float percent)
        {
            if (segment == 0) segment = 1 / colorLevels.Count;

            // 找到對應的區間
            for (int i = 0; i < colorLevels.Count - 1; i++)
            {
                float start = i * segment;
                float end = (i + 1) * segment;

                if (percent >= start && percent <= end)
                {
                    // 計算區間內的插值比例
                    float t = (percent - start) / segment;
                    return Color.Lerp(colorLevels[i], colorLevels[i + 1], t);
                }
            }
            // 若超出範圍，返回默認顏色（應避免進入此情況）
            return colorLevels[colorLevels.Count - 1];
        }

        
        #if UNITY_EDITOR
        [CustomEditor(typeof(ImageFilledHandler))]
        public class ImageFilledHandlerEditor : Editor { } //需設置才能改變原組件的Inspector內容
        #endif
    }
}