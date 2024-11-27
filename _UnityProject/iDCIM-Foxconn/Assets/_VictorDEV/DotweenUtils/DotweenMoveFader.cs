using DG.Tweening;
using UnityEngine;

public class DotweenMoveFade : MonoBehaviour
{
    [Header(">>> 目標對像")]
    [SerializeField] private RectTransform rectTransform;

    [Header(">>> 動畫時間")]
    [SerializeField] float duration = 1f;

    [Header(">>> 延遲時間")]
    [SerializeField] float delaySec = 0f;

    [Header(">>> 是否OnEnabled時自動播放")]
    [SerializeField] bool isPlayOnEnabled = true;

    [Space(20)]
    [Header("Position Settings")]
    [SerializeField] Vector2 fromPosition;
    [SerializeField] Vector2 toPosition;

    [Header("Scale Settings")]
    [SerializeField] Vector3 fromScale = Vector3.one;
    [SerializeField] Vector3 toScale = Vector3.one;

    [Header("Fade Settings")]
    [SerializeField] float fromAlpha = 0f;
    [SerializeField] float toAlpha = 1f;

    [Header(">>> 動畫曲線")]
    [SerializeField] Ease tweenEase = Ease.OutQuad;

    [Header("Anchor Settings")]
    [SerializeField] bool respectAnchors = true;

    private CanvasGroup _canvasGroup { get; set; }
    private CanvasGroup canvasGroup => _canvasGroup ??= rectTransform.GetComponent<CanvasGroup>();


    private void OnEnable()
    {
        if (isPlayOnEnabled) PlayTween();
    }

    [ContextMenu("- PlayTween")]
    public void PlayTween()
    {
        OnDisable();

        if (rectTransform != null && respectAnchors) rectTransform.anchoredPosition = fromPosition;
        else transform.localPosition = fromPosition;

        transform.localScale = fromScale;
        canvasGroup.alpha = fromAlpha;

        Sequence sequence = DOTween.Sequence();

        // Position Tween
        if (rectTransform != null && respectAnchors)
        {
            sequence.Join(rectTransform.DOAnchorPos(toPosition, duration));
        }
        else
        {
            sequence.Join(transform.DOLocalMove(toPosition, duration));
        }

        // Scale Tween
        sequence.Join(transform.DOScale(toScale, duration));

        // Fade Tween
        sequence.Join(canvasGroup.DOFade(toAlpha, duration));

        sequence.Play().SetEase(tweenEase).SetDelay(delaySec).SetAutoKill(true);
    }

    private void OnDisable()
    {
        DOTween.Kill(rectTransform);
        DOTween.Kill(canvasGroup);
    }
}
