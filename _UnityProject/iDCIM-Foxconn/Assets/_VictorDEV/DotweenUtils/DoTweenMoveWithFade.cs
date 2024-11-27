using DG.Tweening;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    public class DoTweenMoveWithFade : MonoBehaviour
    {
        [Header(">>> 動畫對像")]
        [SerializeField] RectTransform targetRect;

        [Space(10)]
        [Header(">>> 偏移值")]
        [SerializeField] Vector2 offset = new Vector2(100, 0);
        [Header(">>> 移動時間")]
        [SerializeField] float moveDuration = 1f;
        [Header(">>> 移動曲線")]
        [SerializeField] Ease moveEase = Ease.OutQuad;
        [Header(">>> 淡入時間")]
        [SerializeField] float fadeDuration = 1f;
        [Header(">>> 延遲時間")]
        [SerializeField] float delaySec = 0f;

        private CanvasGroup _canvasGroup { get; set; }
        private CanvasGroup canvasGroup => _canvasGroup ??= targetRect.GetComponent<CanvasGroup>();

        /// <summary>
        ///  記錄初始位置
        /// </summary>
        private Vector2 originalPosition { get; set; }

        private void Awake()
        {
            originalPosition = targetRect.anchoredPosition;
        }

        private void OnEnable() => PlayTween();

        [ContextMenu("- PlayTween")]
        private void PlayTween()
        {
            OnDisable();
            // 初始化透明度和位置
            canvasGroup.alpha = 0;
            targetRect.anchoredPosition = originalPosition + offset;
            // 使用 DOTween 進行移動和透明度動畫
            targetRect.DOAnchorPos(originalPosition, moveDuration).SetEase(moveEase).SetDelay(delaySec).SetAutoKill(true);
            canvasGroup.DOFade(1, fadeDuration).OnUpdate(() =>
            {
                canvasGroup.interactable = canvasGroup.blocksRaycasts = canvasGroup.alpha == 1;
            }).SetDelay(delaySec).SetAutoKill(true);
        }

        private void OnDisable()
        {
            targetRect.DOKill();
            canvasGroup.DOKill();
        }
    }
}
