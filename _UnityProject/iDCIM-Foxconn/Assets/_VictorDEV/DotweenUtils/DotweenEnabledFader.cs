using DG.Tweening;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DotweenEnabledFader : MonoBehaviour
    {
        public float duration = 0.3f;
        public Ease ease = Ease.OutQuad;
        public float delaySec = 0;
        public bool isRandomDelay = false;

        private void OnEnable()
        {
            DOTween.Kill(canvasGroup);

            canvasGroup.DOFade(1, duration).From(0).SetEase(ease).SetAutoKill(true)
            .SetDelay(isRandomDelay ? Random.Range(0, delaySec) : delaySec)
            .OnUpdate(OnUpdateHandler);
        }

        private void OnUpdateHandler()
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = (canvasGroup.alpha == 1);
        }
        private CanvasGroup _canvasGroup { get; set; }
        private CanvasGroup canvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
    }
}
