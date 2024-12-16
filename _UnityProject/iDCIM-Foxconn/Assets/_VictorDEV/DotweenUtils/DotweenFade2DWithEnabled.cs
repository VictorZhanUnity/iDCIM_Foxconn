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
        [SerializeField] private float duration = 0.1f;
        [SerializeField] private float dealy = 0.1f;
        [SerializeField] private Ease ease = Ease.OutQuad;
        [SerializeField] private Vector3 pos = new Vector3(0, 0);

        [Header(">>> 動畫目標對像(若為空則自動指向本身")]
        [SerializeField] private Transform targetTrans;
        private Vector3? originalPos { get; set; } = null;
        private CanvasGroup _canvasGroup { get; set; }
        public CanvasGroup canvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        #endregion

        private void OnEnable()
        {
            targetTrans ??= transform;
            originalPos ??= targetTrans.localPosition;

            if (targetTrans.TryGetComponent(out CanvasGroup cg) == false)
            {
                cg = targetTrans.AddComponent<CanvasGroup>();
            }

            Vector3 fromPos = originalPos ?? Vector3.zero + pos;

            float rndDelay = Random.Range(0, dealy);
            cg.DOFade(1, duration).From(0).SetEase(ease).SetDelay(rndDelay);
            targetTrans.DOLocalMove(originalPos ?? Vector3.zero, duration).From(fromPos).SetEase(ease).SetDelay(rndDelay);
        }
    }
}
