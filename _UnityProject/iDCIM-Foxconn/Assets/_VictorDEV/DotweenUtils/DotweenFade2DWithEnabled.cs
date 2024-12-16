using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    /// <summary>
    /// Enabled動畫控制器
    /// </summary>
    public class DotweenFade2DWithEnabled : MonoBehaviour
    {
        #region [Componenets]
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private float dealy = 0.2f;
        [SerializeField] private Ease ease = Ease.OutQuad;
        [SerializeField] private Vector3 posValue = Vector3.zero;
        [SerializeField] private float scaleValue =1f;

        [Header(">>> 動畫目標對像(若為空則自動指向本身")]
        [SerializeField] private Transform targetTrans;
        private Vector3? originalPos { get; set; } = null;
        private Vector3? originalScale { get; set; } = null;
        private CanvasGroup _canvasGroup { get; set; }
        public CanvasGroup canvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        #endregion

        private void OnEnable()=> ToShow();
        public void ToShow()
        {
            targetTrans ??= transform;
            originalPos ??= targetTrans.localPosition;
            originalScale ??= targetTrans.localScale;
            if (targetTrans.TryGetComponent(out CanvasGroup cg) == false)
            {
                cg = targetTrans.AddComponent<CanvasGroup>();
            }
            Vector3 fromPos = (originalPos ?? Vector3.zero) + posValue;
            float rndDelay = Random.Range(0, dealy);
            cg.DOFade(1, duration).From(0).SetEase(ease).SetDelay(rndDelay);
            DOTween.Sequence()
                   .Join(targetTrans.DOLocalMove(originalPos ?? Vector3.zero, duration).From(fromPos).SetEase(ease).SetDelay(rndDelay))
                   .Join(targetTrans.DOScale(originalScale ?? Vector3.zero, duration).From(new Vector3(scaleValue, scaleValue, scaleValue)).SetEase(ease).SetDelay(rndDelay))
                   ;
        }
    }
}
