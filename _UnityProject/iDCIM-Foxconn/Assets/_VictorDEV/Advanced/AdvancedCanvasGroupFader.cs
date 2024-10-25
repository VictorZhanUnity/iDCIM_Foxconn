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

        public bool isOn { set => ToFade(value ? 1 : initAlpha); }

        private void Start() => SetAlpha(initAlpha);

        /// <summary>
        /// 開始 Lerp
        /// </summary>
        public void ToFade(float toAlpha)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            IEnumerator enumerator()
            {
                DotweenHandler.ToLerpValue(canvasGroup.alpha, toAlpha, SetAlpha, duration);
                yield return null;
            }
            if(gameObject.activeInHierarchy) coroutine = StartCoroutine(enumerator());
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
