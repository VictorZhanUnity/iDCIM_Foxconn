using DG.Tweening;  // 確保已導入 DoTween 命名空間
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DoTweenFadeController : MonoBehaviour
{
    [SerializeField] private bool isHideInAwake = false;

    [Tooltip("淡入淡出動畫持續時間")]
    public float fadeDuration = 0.5f;
    public float scaleDuration = 0.7f;  // 縮放動畫持續時間
    public Vector3 targetScale = new Vector3(1, 1, 1);  // 最終縮放比例
    public Vector3 initialScale = new Vector3(0.9f, 0.9f, 0.9f);  // 初始縮放比例

    public Ease easeFadeIn = Ease.OutExpo;
    public Ease easeFadeOut = Ease.OutQuart;

    public UnityEvent OnFadeInEvent;
    public UnityEvent OnFadeOutEvent;

    public CanvasGroup canvasGroup;  // 用於處理淡入淡出
    public RectTransform rectTransform;  // 用於處理縮放

    public bool isOn
    {
        set
        {
            if (value) FadeIn();
            else FadeOut();
        }
    }

    private void Awake()
    {
        OnValidate();
        if (isHideInAwake)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

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
        FadeIn(false);
    }
    public void FadeIn(bool isForce = false)
    {
        DOTween.Kill(canvasGroup);
        DOTween.Kill(rectTransform);

        gameObject.SetActive(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;

        if (isForce)
        {
            canvasGroup.alpha = 0;
            rectTransform.localScale = initialScale;
        }

        canvasGroup.DOFade(1, fadeDuration);
        rectTransform.DOScale(targetScale, scaleDuration).SetEase(easeFadeIn).OnComplete(() =>
        {
            OnFadeInEvent.Invoke();
            canvasGroup.interactable = true;
        });
    }

    [ContextMenu("FadeOut")]
    // 淡出動畫
    public void FadeOut()
    {
        DOTween.Kill(canvasGroup);
        DOTween.Kill(rectTransform);

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0, fadeDuration);
        rectTransform.DOScale(initialScale, scaleDuration).SetEase(easeFadeOut).OnComplete(() =>
        {
            OnFadeOutEvent.Invoke();
        });
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
