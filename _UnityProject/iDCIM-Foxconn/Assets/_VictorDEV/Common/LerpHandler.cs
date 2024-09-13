using System;
using System.Collections;
using UnityEngine;

namespace VictorDev.Common
{
    /// <summary>
    /// Lerp效果公式
    /// <parp>僅回傳Lerp效果公式Coroutine，讓呼叫者自已執行與管理Coroutine</parp>
    /// </summary>
    public static class LerpHandler
    {
        public static IEnumerator Lerping(float startValue, float endValue, Action<float> action, float duration)
        {
            bool isLerping = true;
            float lerpStartTime = Time.time;
            while (isLerping)
            {
                float timeElapsed = Time.time - lerpStartTime;
                float t = timeElapsed / duration;

                float lerpValue = Mathf.Lerp(startValue, endValue, t);
                action.Invoke(lerpValue);

                // 動畫結束時停止 Lerp
                if (t >= 1)
                {
                    isLerping = false;
                }
                yield return new WaitForEndOfFrame();
            }
            action.Invoke(endValue); //確保最終值一定為endValue
        }
    }
}
