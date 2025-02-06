using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VictorDev.Advanced
{
    /// 偵測鼠標於UI組件上的移入/移出狀態
    /// <para>+ 直接掛載在欲偵測事件的UI物件上即可，自動以RectTransform尺吋為偵測範圍</para>
    public class PointerEventHandler : MonoBehaviour
        , IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isEntering) return;
            _isEntering = true;
            onPointerEnterEvent.Invoke();
            onPointerEvent.Invoke(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isPressDown) return;

            // 檢查鼠標是否進入子物件
            _isEntering = false;
            onPointerExitEvent.Invoke();
            onPointerEvent.Invoke(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressDown = false;
            if (_isEntering == false) OnPointerExit(eventData);
        }

        #region Initialize

        private void Awake()
        {
            if (isHideInAwake) OnPointerExit(null);
        }

        #endregion

        #region Components

        [Header(">>> 是否在Awake時隱藏")] [SerializeField]
        private bool isHideInAwake = true;

        [Header("[Event] - 當鼠標移入/移出時(bool:是否移入)")]
        public UnityEvent<bool> onPointerEvent;

        [Header("[Event] - 當鼠標移入時")] public UnityEvent onPointerEnterEvent;
        [Header("[Event] - 當鼠標移出時")] public UnityEvent onPointerExitEvent;
        private bool _isPressDown = false;
        private bool _isEntering = false;

        #endregion
    }
}