using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.DoTweenUtils
{

    public class DoTweenFadeController : MonoBehaviour
    {
        [SerializeField] private bool isHideInAwake = false;

        [Tooltip("淡入淡出動畫持續時間")]
        public float fadeDuration = 0.7f;
        public float scaleDuration = 0.7f;  // 縮放動畫持續時間
        public Vector3 targetScale = new Vector3(1, 1, 1);  // 最終縮放比例
        public Vector3 initialScale = new Vector3(0.8f, 0.8f, 0.8f);  // 初始縮放比例

        public Ease easeShow = Ease.OutQuart;
        public Ease easeHide = Ease.InQuart;

        public UnityEvent OnShowEvent;
        public UnityEvent OnHideEvent;

        public CanvasGroup canvasGroup;  // 用於處理淡入淡出
        public RectTransform rectTransform;  // 用於處理縮放

        public bool isOn
        {
            set
            {
                if (value) Show();
                else ToHide();
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
                rectTransform.localScale = initialScale;
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
        [ContextMenu("Show")]
        // 淡入動畫
        private void Show()
        {
            ToShow(false);
        }
        public void ToShow(bool isForce = false)
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
            rectTransform.DOScale(targetScale, scaleDuration).SetEase(easeShow).OnComplete(() =>
            {
                OnShowEvent.Invoke();
                canvasGroup.interactable = true;
            });
        }

        [ContextMenu("Hide")]
        // 淡出動畫
        public void ToHide()
        {
            DOTween.Kill(canvasGroup);
            DOTween.Kill(rectTransform);

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0, fadeDuration * 0.6f);
            rectTransform.DOScale(initialScale, scaleDuration * 0.6f).SetEase(easeHide).OnComplete(() =>
            {
                OnHideEvent.Invoke();
            });
        }

        // 淡入並縮放的動畫（指定大小）
        public void FadeInWithScale(Vector3 targetScale, float fadeDuration, float scaleDuration)
        {
            canvasGroup.DOFade(1, fadeDuration).SetEase(Ease.InOutQuad);
            rectTransform.DOScale(targetScale, scaleDuration).SetEase(easeShow);
        }

        // 淡出並縮放的動畫（指定大小）
        public void FadeOutWithScale(Vector3 targetScale, float fadeDuration, float scaleDuration)
        {
            canvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
            rectTransform.DOScale(targetScale, scaleDuration).SetEase(easeHide);
        }
    }
}
