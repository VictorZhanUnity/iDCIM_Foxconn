using UnityEngine;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 自動調整Rect尺吋大小
    /// <para>+ 直接掛載到GameObject下即可</para>
    /// </summary>
    [ExecuteInEditMode]
    public class AutoResizeRect : MonoBehaviour
    {
        [SerializeField] private bool isActivated = true;

        [Header(">>> 指定解析度")]
        [SerializeField] private Vector2 resolution = new Vector2(1920, 1080);
        [Header(">>> 是否強迫為指定解析度")]
        [SerializeField] private bool isForcedResolution = false;

        [Space(10)]
        [SerializeField] private bool isNeedToUpdate = false;
        private RectTransform rectTransform;
        private Vector2 newSize { get; set; }

        private void Awake() => rectTransform ??= GetComponent<RectTransform>();

#if UNITY_EDITOR
        private void Update()
        {
            isNeedToUpdate = rectTransform.sizeDelta != newSize;
            if (isNeedToUpdate && isActivated) Resize();
        }
#endif

        private void Resize()
        {
            Canvas.ForceUpdateCanvases(); //強迫Canvas更新一幀

            //指定尺吋
            if (isForcedResolution) rectTransform.sizeDelta = resolution;
            else
            {
                // 設置新的尺寸，根據新的寬度等比例調整高度
                float newHeight = resolution.y * (rectTransform.rect.width / resolution.x);
                newSize = new Vector2(rectTransform.rect.width, newHeight);
                // 設置新的尺寸
                rectTransform.sizeDelta = newSize;
            }
        }
    }
}
