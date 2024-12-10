using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using Random = UnityEngine.Random;

namespace VictorDev.Common
{
    public static class DotweenHandler
    {

        public static void ToBlink(List<TextMeshProUGUI> targets, string showText = null, float duration = 0.1f, float delay = 0, bool isRandomDelay = false)
            => targets.ForEach(txt => ToBlink(txt, showText, duration, delay, isRandomDelay));

        /// <summary>
        /// 閃爍後顯示指定文字
        /// <para>+ showText：顯示指定文字</para>
        /// </summary>
        public static void ToBlink(TextMeshProUGUI target, string showText = null, float duration = 0.1f, float delay = 0, bool isRandomDelay = false)
        {
            DOTween.Kill(target);
            // 首先将Text的透明度设置为0（完全透明）
            target.DOFade(0f, duration).OnComplete(() =>
            {
                // 更改文本内容
                if (showText != null) target.SetText(showText);
                // 然后将Text的透明度从0渐变为1（完全不透明）
                target.DOFade(1f, duration).SetEase(Ease.OutQuad);
            }).SetDelay(Random.Range(isRandomDelay ? 0 : delay, delay));
        }

        public static Tween ToLerpValue(float startValue, float endValue, Action<float> onUpdate, float duration = 0.15f, float dealy = 0)
        {
            return DOTween.To(() => startValue, x =>
            {
                startValue = x;
                onUpdate.Invoke(startValue);
            }, endValue, duration).SetDelay(dealy).SetEase(Ease.OutQuart);
        }
    }
}
