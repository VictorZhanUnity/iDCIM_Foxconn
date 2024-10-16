using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VictorDev.Common;

namespace VictorDev.Advanced
{
    /// <summary>
    /// 偵測鼠標於UI組件上的移入/移出狀態
    /// <para>+ 直接掛載在欲偵測事件的UI物件上即可，自動以RectTransform尺吋為偵測範圍</para>
    /// </summary>
    public class PointerEventHandler : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header(">>> 當鼠標移入時")]
        public UnityEvent OnPointerEnterEvent;
        [Header(">>> 當鼠標移出時")]
        public UnityEvent OnPointerExitEvent;
        [Header(">>> 當鼠標移入/移出時(bool:是否移入)")]
        public UnityEvent<bool> OnPointerEvent;
        [Header(">>> 本身物件的Canvas(選填：設定SortOrder先後順序用)")]
        [SerializeField] private Canvas canvas;

        private bool isPressDown { get; set; } = false;
        private bool isEntering { get; set; } = false;

        private void Awake() => OnPointerExit(null);

        /// <summary>
        /// 若用OnPointerEnter，鼠標速度過快會觸發不了，所以用OnPointerMove
        /// </summary>
        public void OnPointerMove(PointerEventData eventData)
        {
            if (isEntering) return;
            isEntering = true;
            OnPointerEnterEvent.Invoke();
            OnPointerEvent.Invoke(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isPressDown) return;

            // 檢查鼠標是否進入子物件
            isEntering = false;
            OnPointerExitEvent.Invoke();
            OnPointerEvent.Invoke(false);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressDown = true;
            if (canvas != null) MoveToFront();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isPressDown = false;
            if (isEntering == false) OnPointerExit(eventData);
        }

        public void MoveToFront() => CanvasSorter.MoveCanvasToFront(canvas);
    }
}
