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
        /// <summary>
        /// Lerp CanvasGroup Alpha
        /// </summary>
        public static IEnumerator ToLerpAlpha(float startValue, float endValue, Action<float> action, float duration)
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

        /// <summary>
        /// Lerp RectTransform Rotation {Quaternion}
        /// </summary>
        public static IEnumerator ToLerpRectRotationZ(float startValue, float endValue, Action<Quaternion> action, float rotationDuration)
        {
            bool isLerping = true;
            Quaternion startRotation = Quaternion.Euler(0, 0, startValue);
            Quaternion targetRotation = Quaternion.Euler(0, 0, endValue);
            float rotationTime = 0; // 當前旋轉的時間

            while (isLerping)
            {
                // 增加已經過的旋轉時間
                rotationTime += Time.deltaTime;

                // 計算旋轉的比例
                float t = Mathf.Clamp01(rotationTime / rotationDuration);

                // 使用Lerp進行平滑旋轉
                Quaternion result = Quaternion.Lerp(startRotation, targetRotation, t);
                action.Invoke(result);

                // 檢查是否完成旋轉
                if (t >= 1f)
                {
                    isLerping = false;  // 停止旋轉
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
