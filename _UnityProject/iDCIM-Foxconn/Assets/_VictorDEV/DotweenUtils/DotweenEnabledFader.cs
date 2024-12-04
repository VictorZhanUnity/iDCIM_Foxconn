using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DotweenEnabledFader : MonoBehaviour
{
    public float duration = 0.3f;
    public Ease ease = Ease.OutQuad;

    private CanvasGroup _canvasGroup { get; set; }
    private CanvasGroup canvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

    private void OnEnable()
    {
        DOTween.Kill(canvasGroup);
        canvasGroup.DOFade(1, duration).From(0).SetEase(ease).SetAutoKill(true).OnUpdate(() =>
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = (canvasGroup.alpha == 1);
        });
    }
}
