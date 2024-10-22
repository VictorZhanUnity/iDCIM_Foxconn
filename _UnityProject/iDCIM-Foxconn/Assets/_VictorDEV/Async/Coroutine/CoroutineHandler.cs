using System;
using System.Collections;
using UnityEngine;
using VictorDev.Common;

namespace VictorDev.Async.CoroutineUtils
{
    /// <summary>
    /// 特地獨立出來進行Coroutine相關處理
    /// <para>+ 為了避免GameObject被Disabled時無法執行Coroutine的問題</para>
    /// </summary>
    public class CoroutineHandler : SingletonMonoBehaviour<CoroutineHandler>
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

            return RunCoroutine_Old(LerpValueEnumerator(fromValue, toValue, duration, onUpdate));
        }

        /// <summary>
        /// 執行Coroutine
        /// </summary>
        public static Coroutine ToStartCoroutine(IEnumerator iEnumerator) => Instance.StartCoroutine(iEnumerator);

        /// <summary>
        /// 取消Coroutine
        /// </summary>
        public static void ToStopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null) Instance.StopCoroutine(coroutine);
        }

        /// <summary>
        /// 執行Coroutine (舊版)
        /// </summary>
        public static IEnumerator RunCoroutine_Old(IEnumerator iEnumerator)
        {
            Instance.StartCoroutine(iEnumerator);
            return iEnumerator;
        }

        /// <summary>
        /// 取消Coroutine(舊版)
        /// </summary>
        public static void CancellCoroutine_Old(IEnumerator iEnumerator) => Instance.StopCoroutine(iEnumerator);
    }
}
