using DG.Tweening;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    /// <summary>
    /// 鼠標覆蓋時縮放
    /// </summary>
    public class ScaleOnMouseOver : MonoBehaviour
    {
        [Header(">>> 變大倍率")]
        [SerializeField] private Vector3 scaleFactor = new Vector3(1.2f, 1.2f, 1.2f);
        [Header(">>> 時間")]
        [SerializeField] private float duration = 0.7f;
        [Header(">>> 延遲")]
        [SerializeField] private float delay = 0f;
        [Header(">>> 動畫")]
        [SerializeField] private Ease easeMouseEnter = Ease.OutQuad;
        [SerializeField] private Ease easeMouseExit = Ease.InQuad;
        private Vector3 origianlScale { get; set; }

        private void OnMouseEnter() => SetScale(true);
        private void OnMouseExit() => SetScale(false);

        private void SetScale(bool isIncrease)
        {
            Vector3 toScale = isIncrease ? scaleFactor : origianlScale;
            transform.DOScale(toScale, duration).SetEase(isIncrease ? easeMouseEnter : easeMouseExit).SetDelay(delay);
        }

        private void Awake() => origianlScale = transform.localScale;
    }
}
