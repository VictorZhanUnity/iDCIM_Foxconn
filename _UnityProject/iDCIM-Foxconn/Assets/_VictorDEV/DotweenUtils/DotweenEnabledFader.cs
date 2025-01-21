using DG.Tweening;
using UnityEngine;

namespace VictorDev.DoTweenUtils
{
    public class DotweenEnabledFader : MonoBehaviour
    {
        [Header("若無則擷取物件本身")] public CanvasGroup canvasGroup;

        public void Show()
        {
            DOTween.Kill(canvasGroup);
            canvasGroup.DOFade(1, duration).From(0).SetEase(_ease).SetAutoKill(true).OnUpdate(OnUpdateHandler);
        }

        private void OnUpdateHandler()
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = (canvasGroup.alpha == 1);
        }

        public void Closs()
        {
             //= canvasGroup.DOFade(1, duration).From(0).SetEase(_ease).SetAutoKill(true).OnUpdate(OnUpdateHandler);
        }


        #region Initailize

        private void OnEnable() => Show();
        private void OnDisable() => Closs();

        private void Awake()
        {
            if (canvasGroup == null)
            {
                if (TryGetComponent(out canvasGroup) == false)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
        }

        #endregion

        #region Components

        [Header(">>> 動畫設定")] public bool isMoveable = true;
        public Vector3 fromPos;
        public bool isScaleable = true;
        public Vector3 fromScale;
        private readonly Ease _ease = Ease.OutQuad;
        public float duration = 0.3f;

        #endregion
    }
}