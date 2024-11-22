using DG.Tweening;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AutoFadeSpriteRender : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = .7f; // 单次Fade的持续时间
        [SerializeField] private float minAlpha = 0f; // 最低透明度
        [SerializeField] private float maxAlpha = 1f; // 最高透明度

        private SpriteRenderer spriteRenderer { get; set; }
        private void Start()
        {
            // 获取 SpriteRenderer 组件
            spriteRenderer ??= GetComponent<SpriteRenderer>();

            // 开始循环 Fade 动画
            StartFadeLoop();
        }

        private void StartFadeLoop()
        {
            spriteRenderer ??= GetComponent<SpriteRenderer>();
            // 设置初始颜色
            Color startColor = spriteRenderer.color;
            startColor.a = minAlpha; // 从最低透明度开始
            spriteRenderer.color = startColor;

            // 循环 Fade 动画：从 minAlpha 到 maxAlpha，再回到 minAlpha
            spriteRenderer.DOFade(maxAlpha, fadeDuration)
                .SetEase(Ease.InOutSine) // 渐进渐出效果
                .SetLoops(-1, LoopType.Yoyo); // 无限循环，Yoyo模式
        }

        private void OnEnable()
        {
            StartFadeLoop();
        }
        private void OnDisable()
        {
            DOTween.Kill(spriteRenderer);
        }
    }
}
