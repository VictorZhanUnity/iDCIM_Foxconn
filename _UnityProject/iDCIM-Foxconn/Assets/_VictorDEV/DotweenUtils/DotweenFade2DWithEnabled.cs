using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Sequence = DG.Tweening.Sequence;

namespace VictorDev.DoTweenUtils
{
    /// <summary>
    /// Enabled動畫控制器
    /// </summary>
    public class DotweenFade2DWithEnabled : MonoBehaviour
    {
        #region [Components]
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private float dealy = 0.2f;
        [SerializeField] private Ease ease = Ease.OutQuad;
        [Header(">>> 是否移動")]
        [SerializeField] private bool isDoMove = true;
        [SerializeField] private Vector3 fromPosValue = Vector3.zero;
        [Header(">>> 是否縮放")]
        [SerializeField] private bool isDoScale = true;
        [SerializeField] private float fromScaleValue = 1f;

        [Header(">>> 動畫目標對像(若為空則自動指向本身")]
        [SerializeField] private Transform targetTrans;
        private Vector3? originalPos { get; set; } = null;
        private Vector3? originalScale { get; set; } = null;
        private CanvasGroup _canvasGroup { get; set; }
        public CanvasGroup canvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        #endregion

        [Header(">>> [Event] OnEabled時Invoke")]
        public UnityEvent onEnabledEvent = new UnityEvent();
        [Header(">>> [Event] OnDisabled時Invoke")]
        public UnityEvent onDisabledEvent = new UnityEvent();

        private void OnEnable()
        {
            onEnabledEvent?.Invoke();
            ToShow();
        }

        public void ToShow()
        {
            DOTween.Kill(targetTrans);

            if (targetTrans == null) targetTrans = transform;
            originalPos ??= targetTrans.localPosition;
            originalScale ??= targetTrans.localScale;
            if (targetTrans.TryGetComponent(out CanvasGroup cg) == false)
            {
                cg = targetTrans.AddComponent<CanvasGroup>();
            }
            Vector3 fromPos = (originalPos ?? Vector3.zero) + fromPosValue;
            float rndDelay = Random.Range(0, dealy);
            cg.DOFade(1, duration).From(0).SetEase(ease).SetDelay(rndDelay);

            if (isDoMove) targetTrans.DOLocalMove(originalPos ?? Vector3.zero, duration).From(fromPos).SetEase(ease).SetDelay(rndDelay);
            if (isDoScale) targetTrans.DOScale(originalScale ?? Vector3.zero, duration).From(new Vector3(fromScaleValue, fromScaleValue, fromScaleValue)).SetEase(ease).SetDelay(rndDelay);

            gameObject.SetActive(true);
        }
        private void OnDisable() => onDisabledEvent?.Invoke();
    }
}
