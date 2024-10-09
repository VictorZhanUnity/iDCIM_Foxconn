using System.Collections;
using UnityEngine;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 控制RectTransform縮放狀態
    /// <para>+ 直接掛載到卻縮於的UI組件上即可</para>
    /// <para>+ 控制isOn來進行Lerp動畫</para> 
    /// </summary>
    public class RectTransformResizeLerp : MonoBehaviour
    {
        [Header(">>> 持續時間")]
        public float duration = 0.5f;

        [Header(">>> 目標高度 (若不需變更則設定初始值即可")]
        public float targetHeight = 500f;

        [Header(">>> 目標寬度 (若不需變更則設定初始值即可")]
        public float targetWidth = 500f;

        [Space(10)]
        public RectTransform rectTransform; // 需要調整大小的 RectTransform
        private bool increaseSize = false; // 控制大小變化的布林變數

        private float initialHeight; // 起始高度
        private float initialWidth; // 起始寬度
        private Coroutine lerpCoroutine; // 用於儲存 Coroutine

        public void Restore()
        {
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine); // 停止正在運行的 Coroutine
            }
            rectTransform.sizeDelta = new Vector2(initialWidth, initialHeight);
        }

        /// <summary>
        /// 控制isOn來進行Lerp動畫
        /// </summary>
        public bool isOn
        {
            get => increaseSize;
            set
            {
                increaseSize = value;
                StartLerp(increaseSize);
            }
        }

        private void Start()
        {
            // 初始化起始高度和寬度
            initialHeight = rectTransform.sizeDelta.y;
            initialWidth = rectTransform.sizeDelta.x;
        }

        // 呼叫這個方法來開始 Lerp 過程
        private void StartLerp(bool increase)
        {
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine); // 停止正在運行的 Coroutine
            }

            // 設置 increaseSize 變數
            increaseSize = increase;

            // 啟動新的 Coroutine
            lerpCoroutine = StartCoroutine(LerpSize());
        }

        private IEnumerator LerpSize()
        {
            float elapsedTime = 0f;
            float startHeight = increaseSize ? initialHeight : targetHeight;
            float endHeight = increaseSize ? targetHeight : initialHeight;
            float startWidth = increaseSize ? initialWidth : targetWidth;
            float endWidth = increaseSize ? targetWidth : initialWidth;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                // 使用自定義的緩動函數進行平滑過渡
                float easedT = EaseOut(t);

                // 根據進度進行插值 (Lerp) 計算
                float newHeight = Mathf.Lerp(startHeight, endHeight, easedT);
                float newWidth = Mathf.Lerp(startWidth, endWidth, easedT);

                // 設置 RectTransform 的新大小
                rectTransform.sizeDelta = new Vector2(newWidth, newHeight);

                yield return null; // 等待下一幀
            }

            // 確保最終大小設置為目標寬度和高度
            rectTransform.sizeDelta = new Vector2(endWidth, endHeight);

            lerpCoroutine = null; // 完成後清空 Coroutine 變數
        }

        private float EaseOut(float t) => 1 - Mathf.Pow(1 - t, 3); // cubic ease out
        private void OnDisable()
        {
            if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
        }
        private void OnValidate() => rectTransform ??= GetComponent<RectTransform>();
    }
}
