using System;
using System.Collections;
using UnityEngine;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 控制RectTransform旋轉狀態
    /// <para>+ 直接掛載到卻縮於的UI組件上即可</para> 
    /// <para>+ 控制isOn來進行Lerp動畫</para> 
    /// </summary>
    public class RectTransformRotationLerp : MonoBehaviour
    {
        [Header(">>> 持續時間")]
        public float duration = 0.5f;

        [Header(">>> 目標旋轉角度")]
        public float targetRotation = 0f;


        [Space(10)]
        public RectTransform rectTransform; // 需要調整旋轉的 RectTransform
        public bool rotateToTarget = false; // 控制旋轉的布林變數

        private float initialRotation; // 起始旋轉角度
        private Coroutine lerpCoroutine; // 用於儲存 Coroutine

        /// <summary>
        /// 控制isOn來進行Lerp動畫
        /// </summary>
        public bool isOn
        {
            get => rotateToTarget;
            set
            {
                rotateToTarget = value;
                StartLerp(rotateToTarget);
            }
        }

        private void Awake()
        {
            // 初始化起始旋轉角度
            initialRotation = rectTransform.localEulerAngles.z;
        }

        // 呼叫這個方法來開始 Lerp 過程
        private void StartLerp(bool rotateToTarget)
        {
            if (gameObject.activeSelf == false) return;

            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine); // 停止正在運行的 Coroutine
            }

            // 設置 rotateToTarget 變數
            this.rotateToTarget = rotateToTarget;
            
            if(gameObject.activeInHierarchy) lerpCoroutine = StartCoroutine(LerpRotation());
            else rectTransform.localEulerAngles = new Vector3(0, 0, initialRotation);
        }

        private IEnumerator LerpRotation()
        {
            float elapsedTime = 0f;
            float startRotation = rotateToTarget ? initialRotation : targetRotation;
            float endRotation = rotateToTarget ? targetRotation : initialRotation;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                // 使用 SmoothLerp 函數進行平滑過渡
                float easedT = EaseOut(t);

                // 根據進度進行插值 (Lerp) 計算
                float newRotation = Mathf.Lerp(startRotation, endRotation, easedT);

                // 設置 RectTransform 的新旋轉
                rectTransform.localEulerAngles = new Vector3(0, 0, newRotation);

                yield return null; // 等待下一幀
            }

            // 確保最終旋轉設置為目標角度或初始角度
            rectTransform.localEulerAngles = new Vector3(0, 0, endRotation);

            lerpCoroutine = null; // 完成後清空 Coroutine 變數
        }

        // 使用 EaseOut 進行平滑過渡
        private float EaseOut(float t) => 1 - Mathf.Pow(1 - t, 3); // cubic ease out
        private void OnDisable()
        {
            if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        }
        private void OnValidate() => rectTransform ??= GetComponent<RectTransform>();
    }
}
