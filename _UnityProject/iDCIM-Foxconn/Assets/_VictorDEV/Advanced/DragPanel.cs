using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 滑鼠左鍵按住標題列對像，進行拖曳視窗
    /// <para>+ 直接掛載到欲Dragging的物件面板上</para>
    /// <para>+ 設定titleBar對像為拖曳控制的標題列</para>
    /// </summary>
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [Header(">>> 是否啟動Drag功能")]
        [SerializeField] private bool _isActivated = true;

        public bool isActivated { set => _isActivated = value; }

        [Header(">>> 點選控制拖曳的標題列")]
        [SerializeField] private RectTransform titleBarRectTransform;

        [Header(">>> 與容器邊界的距離")]
        [SerializeField] private float gap = 10f;

        [Header(">>> 本身RectTransform")]
        [SerializeField] private RectTransform rectTransform;
        [Header(">>> 父物件RectTransform")]
        [SerializeField] private RectTransform parentRectTransform;

        public RectTransform ParentRectTransform { set => parentRectTransform = value; }

        private Vector2 pointerOffset;
        private bool isDragging { get; set; }

        public UnityEvent onDragged = new UnityEvent();

        public void OnPointerDown(PointerEventData eventData)
        {
            // 檢查是否在標題列上點擊
            if (RectTransformUtility.RectangleContainsScreenPoint(titleBarRectTransform, eventData.position, eventData.pressEventCamera))
            {
                // 當按下標題列時，計算滑鼠點擊位置與 Panel 中心的偏移量
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset);
                isDragging = true;
            }
            else
            {
                isDragging = false;
            }
            MoveToFront();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging || _isActivated)
            {
                // 將滑鼠位置轉換為父物件的本地座標
                Vector2 localPointerPosition;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
                {
                    // 計算 Panel 的新位置
                    Vector2 newPosition = localPointerPosition - pointerOffset;

                    // 限制 Panel 不會移動到父物件外，並且考慮 gap
                    Vector2 clampedPosition = ClampToParent(newPosition);

                    // 設置 Panel 的新位置
                    rectTransform.localPosition = clampedPosition;

                    MoveToFront();
                }
            }
            onDragged?.Invoke();
        }

        // 限制 Panel 的位置使其不會移動到父物件外，並加上 gap
        private Vector2 ClampToParent(Vector2 newPosition)
        {
            // 獲取父物件的邊界
            Vector3[] parentCorners = new Vector3[4];
            parentRectTransform.GetLocalCorners(parentCorners);

            // 獲取 Panel 的邊界
            Vector3[] panelCorners = new Vector3[4];
            rectTransform.GetLocalCorners(panelCorners);

            // 計算 Panel 的寬度與高度
            float panelWidth = panelCorners[2].x - panelCorners[0].x;
            float panelHeight = panelCorners[2].y - panelCorners[0].y;

            // 計算 Panel 允許的最小與最大位置，並考慮 gap
            float minX = parentCorners[0].x + panelWidth / 2 + gap;
            float maxX = parentCorners[2].x - panelWidth / 2 - gap;
            float minY = parentCorners[0].y + panelHeight / 2 + gap;
            float maxY = parentCorners[2].y - panelHeight / 2 - gap;

            // 限制 Panel 的新位置在父物件範圍內，並考慮 gap
            float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);

            return new Vector2(clampedX, clampedY);
        }

        public void MoveToFront()
        {
            transform.SetAsLastSibling();
            onDragged.Invoke();
        }
        private void OnDestroy()
        {
            onDragged.RemoveAllListeners();
        }
    }
}