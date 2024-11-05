using UnityEngine;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 同步自身RectTransform與對像RectTransform的數值設定
    /// <para> 直接放在自身RectTransform上即可</para>
    /// </summary>
    [ExecuteInEditMode]
    public class SyncRectTransforms : MonoBehaviour
    {
        [Header(">>> 要同步的目標 RectTransform")]
        [SerializeField] private RectTransform targetRectTransform;

        [Header(">>> 自身的RectTransform")]
        [SerializeField] private RectTransform sourceRectTransform;

        private void Awake() => OnValidate();
        private void OnValidate() => sourceRectTransform ??= GetComponent<RectTransform>();

        /// <summary>
        /// 用來同步 RectTransform 的函數
        /// </summary>
        private void SyncTransforms()
        {
            // 複製 sourceRectTransform 的值到 targetRectTransform
            targetRectTransform.anchoredPosition = sourceRectTransform.anchoredPosition;
            targetRectTransform.sizeDelta = sourceRectTransform.sizeDelta;
            targetRectTransform.anchorMin = sourceRectTransform.anchorMin;
            targetRectTransform.anchorMax = sourceRectTransform.anchorMax;
            targetRectTransform.pivot = sourceRectTransform.pivot;
            // 同步旋转
            targetRectTransform.localEulerAngles = sourceRectTransform.localEulerAngles;
            // 同步缩放
            targetRectTransform.localScale = sourceRectTransform.localScale;
        }

        /// <summary>
        /// 是否已同步
        /// </summary>
        private bool IsSynced =>
            targetRectTransform.anchoredPosition == sourceRectTransform.anchoredPosition &&
            targetRectTransform.sizeDelta == sourceRectTransform.sizeDelta &&
            targetRectTransform.anchorMin == sourceRectTransform.anchorMin &&
            targetRectTransform.anchorMax == sourceRectTransform.anchorMax &&
            targetRectTransform.pivot == sourceRectTransform.pivot &&
            targetRectTransform.localEulerAngles == sourceRectTransform.localEulerAngles &&
            targetRectTransform.localScale == sourceRectTransform.localScale;

        private void Update()
        {
            if (IsSynced == false && sourceRectTransform != null && targetRectTransform != null)
            {
                SyncTransforms();
            }
        }
    }
}
