using DG.Tweening;
using System.Collections;
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
        [Header(">>> 初始Alpha值")]
        [SerializeField] private float initAlpha = 0f;

        [Header(">>> 動畫持續時間(建議0.15f)")]
        [SerializeField] private float duration = 0.15f;

        [Space(10)]
        [SerializeField] private CanvasGroup canvasGroup;

        private Coroutine coroutine { get; set; } = null;
        private Tween tween { get; set; } = null;

        public bool isOn { set => ToFade(value ? 1 : initAlpha); }

        private void Start() => SetAlpha(initAlpha);

        public void ToFade(float toAlpha) => ToFade(toAlpha, 0);
        /// <summary>
        /// 開始 Lerp
        /// </summary>
        public void ToFade(float toAlpha, float delay, float fromAlpha = -1)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            float startValue = (fromAlpha == -1) ? canvasGroup.alpha : fromAlpha;
            if(delay != 0) SetAlpha(startValue);

            IEnumerator enumerator()
            {
                tween= DotweenHandler.ToLerpValue(startValue, toAlpha, SetAlpha, duration, delay);
                yield return null;
            }
            if (tween != null) tween.Kill();
            if (gameObject.activeInHierarchy) coroutine = StartCoroutine(enumerator());
            else SetAlpha(toAlpha);
        }

        private void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;

            bool isInterable = (initAlpha == 0) ? alpha == 1 : alpha >= initAlpha;
            canvasGroup.interactable = isInterable;
            canvasGroup.blocksRaycasts = isInterable;
        }

        private void OnValidate() => canvasGroup ??= GetComponent<CanvasGroup>();
    }
}
