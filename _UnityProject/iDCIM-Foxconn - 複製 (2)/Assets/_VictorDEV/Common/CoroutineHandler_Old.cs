using System;
using System.Collections;
using UnityEngine;

namespace VictorDev.Common
{
    /// <summary>
    /// Coroutine相關處理(舊版)
    /// </summary>
    public class CoroutineHandler_Old : SingletonMonoBehaviour<CoroutineHandler_Old>
    {
        /// <summary>
        /// Lerp數值從A值到B值
        /// </summary>
        public static IEnumerator LerpValue(float fromValue, float toValue, Action<float> onUpdate, float duration = 1.5f)
        {
            IEnumerator LerpValueEnumerator(float from, float to, float overTime, Action<float> onUpdateCall)
            {
                float currentValue = 0;
                float startTime = Time.time;
                while (Time.time < startTime + overTime)
                {
                    // 计算进度
                    float t = (Time.time - startTime) / overTime;

                    // 使用 Lerp 进行插值
                    currentValue = Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));

                    onUpdateCall.Invoke(currentValue);

                    // 等待下一帧
                    yield return null;
                }
                currentValue = to;
                onUpdateCall.Invoke(currentValue);
            }

            return RunCoroutine(LerpValueEnumerator(fromValue, toValue, duration, onUpdate));
        }

        /// <summary>
        /// 執行Coroutine
        /// </summary>
        public static IEnumerator RunCoroutine(IEnumerator iEnumerator)
        {
            Instance.StartCoroutine(iEnumerator);
            return iEnumerator;
        }


        /// <summary>
        /// 取消Coroutine
        /// </summary>
        public static void CancellCoroutine(IEnumerator iEnumerator) => Instance.StopCoroutine(iEnumerator);
    }
}
