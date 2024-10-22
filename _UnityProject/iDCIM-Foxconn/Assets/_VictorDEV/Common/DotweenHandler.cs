using DG.Tweening;
using TMPro;
using UnityEngine;
namespace VictorDev.Common
{
    public static class DotweenHandler
    {
        /// <summary>
        /// 閃爍後顯示指定文字
        /// </summary>
        public static void ToBlink(TextMeshProUGUI target, string showText = null, float duration = 0.1f, float randomDelay=0)
        {
            // 首先将Text的透明度设置为0（完全透明）
            target.DOFade(0f, duration).OnComplete(() =>
            {
                // 更改文本内容
                if (showText != null) target.SetText(showText);
                // 然后将Text的透明度从0渐变为1（完全不透明）
                target.DOFade(1f, duration);
            }).SetDelay(Random.Range(0, randomDelay)) ;
        }
    }
}
