using UnityEngine;
using VictorDev.Common;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 透用CanvasGroup達到Fade效果
    /// <para> 放在要設置alpha的物件上即可</para>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class AdvancedCanvasGroupFader : MonoBehaviour
    {
        [Header(">>> 動畫持續時間(建議0.15f)")]
        [SerializeField] private float duration = 0.15f;

        [Space(10)]
        [SerializeField] private CanvasGroup canvasGroup;

        private Coroutine coroutine { get; set; } = null;

        public bool isOn { set => ToFade(value ? 1 : 0); }

        /// <summary>
        /// 開始 Lerp
        /// </summary>
        public void ToFade(float toAlpha)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            float startAlpha = canvasGroup.alpha;
            coroutine = StartCoroutine(LerpHandler.ToLerpAlpha(startAlpha, toAlpha, (lerpValue) =>
            {
                canvasGroup.alpha = lerpValue;
                canvasGroup.interactable = lerpValue == 1;
            }, duration));
        }

        private void OnValidate() => canvasGroup ??= GetComponent<CanvasGroup>();
    }
}
