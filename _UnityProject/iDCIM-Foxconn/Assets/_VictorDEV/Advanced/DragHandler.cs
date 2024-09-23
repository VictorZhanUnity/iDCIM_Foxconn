/*using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 滑鼠左鍵按住自身，進行拖曳
    /// <para>+ 直接掛載到欲Dragging的物件上</para>
    /// </summary>
    public class DragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private RectTransform panelRectTransform;
        private Vector2 pointerOffset;
        private RectTransform parentRectTransform;

        // 定義 gap 參數，默認為 10f，可以在 Unity 編輯器中修改
        public float gap = 10f;

        private void Awake()
        {
            // 獲取 Panel 的 RectTransform 組件
            panelRectTransform = GetComponent<RectTransform>();
            // 獲取 Panel 父物件的 RectTransform
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // 當按下時，計算滑鼠點擊位置與 Panel 中心的偏移量
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset);
        }

        public void OnDrag(PointerEventData eventData)
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
                panelRectTransform.localPosition = clampedPosition;
            }
        }

        // 限制 Panel 的位置使其不會移動到父物件外，並加上 gap
        private Vector2 ClampToParent(Vector2 newPosition)
        {
            // 獲取父物件的邊界
            Vector3[] parentCorners = new Vector3[4];
            parentRectTransform.GetLocalCorners(parentCorners);

            // 獲取 Panel 的邊界
            Vector3[] panelCorners = new Vector3[4];
            panelRectTransform.GetLocalCorners(panelCorners);

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
    }
}
*/