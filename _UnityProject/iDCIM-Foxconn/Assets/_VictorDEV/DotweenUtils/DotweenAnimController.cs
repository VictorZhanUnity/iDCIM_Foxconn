using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DotweenAnimController : MonoBehaviour
{
    [Header(">>> [主要] - 設定目標對像")]
    [SerializeField] RectTransform rectTransform;

    [Header(">>> 是否OnEnabled時自動播放")]
    [SerializeField] bool isPlayOnEnabled = true;

    [Header(">>> [Event] - 當Dotween播放完畢時Invoke")]
    public UnityEvent<GameObject> onDotweenFinished = new UnityEvent<GameObject>();

    [Header(">>> Dotween動畫集")]
    [SerializeField] List<DotweenAnimSet> animSet;

    private CanvasGroup _canvasGroup { get; set; }
    private CanvasGroup canvasGroup => _canvasGroup ??= rectTransform.GetComponent<CanvasGroup>();

    private Sequence sequence { get; set; }
    private bool isMouseOver { get; set; } = false;

    private int counter { get; set; } = 0;

    private void OnEnable()
    {
        if (isPlayOnEnabled) Play();
    }

    [ContextMenu("- 播放動畫集")]
    public void Play()
    {
        counter = 0;
        GoAnimTween(animSet[counter]);
    }

    private void GoAnimTween(DotweenAnimSet anim)
    {
        if (sequence != null && sequence.IsActive()) sequence.Kill(); // 終止動畫

        if (rectTransform != null && anim.respectAnchors) rectTransform.anchoredPosition = anim.moveSet.fromPosition;
        else transform.localPosition = anim.moveSet.fromPosition;

        transform.localScale = anim.scaleSet.fromScale;
        canvasGroup.alpha = anim.alphaSet.fromAlpha;

        sequence = DOTween.Sequence();

        // Position Tween
        if (anim.moveSet.isActivated)
        {
            if (rectTransform != null && anim.respectAnchors)
            {
                sequence.Join(rectTransform.DOAnchorPos(anim.moveSet.toPosition, anim.moveSet.moveDuration).SetEase(anim.tweenEase));
            }
            else
            {
                sequence.Join(transform.DOLocalMove(anim.moveSet.toPosition, anim.moveSet.moveDuration).SetEase(anim.tweenEase));
            }
        }

        // Scale Tween
        if (anim.scaleSet.isActivated) sequence.Join(transform.DOScale(anim.scaleSet.toScale, anim.scaleSet.scaleDuration).SetEase(anim.tweenEase));

        // Fade Tween
        if (anim.alphaSet.isActivated) sequence.Join(canvasGroup.DOFade(anim.alphaSet.toAlpha, anim.alphaSet.alphaDuration).SetEase(anim.alphaSet.toAlpha == 1 ? Ease.InQuad : Ease.OutQuad)
            .OnUpdate(() => canvasGroup.interactable = canvasGroup.blocksRaycasts = canvasGroup.alpha == 1));

        sequence.Play().SetDelay(anim.delaySec).SetAutoKill(true).OnComplete(() =>
        {
            if (++counter > animSet.Count - 1)
            {
                counter = 0;
                onDotweenFinished.Invoke(transform.gameObject);
            }
            else GoAnimTween(animSet[counter]);
        });
    }
    private void OnDisable() => onDotweenFinished.RemoveAllListeners();

    public void Pause() => sequence?.Pause();
    public void Resume() => sequence?.Play();

    #region[>>> 設定集]
    [Serializable]
    public class DotweenAnimSet
    {
        [Header(">>> 延遲時間")]
        public float delaySec = 0f;

        [Header(">>> 動畫曲線")]
        public Ease tweenEase = Ease.OutQuad;

        [Header("Anchor Settings")]
        public bool respectAnchors = true;

        public MoveSet moveSet;
        public ScaleSet scaleSet;
        public AlphaSet alphaSet;

        [Serializable]
        public class MoveSet
        {
            public bool isActivated = true;
            [Header(">>> 移動時間")]
            public float moveDuration = 0.7f;
            [Header("Position Settings")]
            public Vector2 fromPosition;
            public Vector2 toPosition;
        }

        [Serializable]
        public class ScaleSet
        {
            public bool isActivated = true;
            [Header(">>> 縮放時間")]
            public float scaleDuration = 0.7f;
            [Header("Scale Settings")]
            public Vector3 fromScale = Vector3.one;
            public Vector3 toScale = Vector3.one;
        }

        [Serializable]
        public class AlphaSet
        {
            public bool isActivated = true;
            [Header(">>> Alpha時間")]
            public float alphaDuration = 0.7f;
            [Header("Fade Settings")]
            public float fromAlpha = 0f;
            public float toAlpha = 1f;
        }
        #endregion
    }
}