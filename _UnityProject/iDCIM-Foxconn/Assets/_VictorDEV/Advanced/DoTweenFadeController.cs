using DG.Tweening;  // 確保已導入 DoTween 命名空間
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DoTweenFadeController : MonoBehaviour
{
    [Tooltip("淡入淡出動畫持續時間")]
    public float fadeDuration = 0.3f;
    public float scaleDuration = 0.3f;  // 縮放動畫持續時間
    public Vector3 targetScale = new Vector3(1, 1, 1);  // 最終縮放比例
    public Vector3 initialScale = new Vector3(0, 0, 0);  // 初始縮放比例

    public Ease easeFadeIn = Ease.InBack;
    public Ease easeFadeOut = Ease.OutBack;

    public UnityEvent OnFadeInEvent;
    public UnityEvent OnFadeOutEvent;

    public CanvasGroup canvasGroup;  // 用於處理淡入淡出
    public RectTransform rectTransform;  // 用於處理縮放

    private void Awake() => OnValidate();

    private void OnValidate()
    {
        if (canvasGroup == null)
        {
            if (transform.TryGetComponent<CanvasGroup>(out canvasGroup) == false)
            {
                canvasGroup = transform.AddComponent<CanvasGroup>();
            }
        }
        rectTransform ??= GetComponent<RectTransform>();
    }
    [ContextMenu("FadeIn")]
    // 淡入動畫
    public void FadeIn()
    {
        canvasGroup.interactable = false;
        canvasGroup.DOFade(1, fadeDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            OnFadeInEvent.Invoke();
            canvasGroup.interactable = true;
        });
        rectTransform.DOScale(targetScale, scaleDuration).SetEase(easeFadeOut);
    }

    [ContextMenu("FadeOut")]
    // 淡出動畫
    public void FadeOut()
    {
        canvasGroup.interactable = false;
        canvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad).OnComplete(OnFadeOutEvent.Invoke);
        rectTransform.DOScale(initialScale, scaleDuration).SetEase(easeFadeIn);
    }

    // 淡入並縮放的動畫（指定大小）
    public void FadeInWithScale(Vector3 targetScale, float fadeDuration, float scaleDuration)
    {
        canvasGroup.DOFade(1, fadeDuration).SetEase(Ease.InOutQuad);
        rectTransform.DOScale(targetScale, scaleDuration).SetEase(easeFadeOut);
    }

    // 淡出並縮放的動畫（指定大小）
    public void FadeOutWithScale(Vector3 targetScale, float fadeDuration, float scaleDuration)
    {
        canvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        rectTransform.DOScale(targetScale, scaleDuration).SetEase(easeFadeIn);
    }
}
