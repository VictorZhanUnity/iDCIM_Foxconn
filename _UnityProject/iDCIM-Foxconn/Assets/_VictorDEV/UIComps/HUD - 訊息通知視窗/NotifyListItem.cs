using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VictorDev.DoTweenUtils;


namespace VictorDev.Common
{
    /// <summary>
    /// [HUD - 訊息通知視窗] - 訊息列表項目
    /// </summary>
    [RequireComponent(typeof(DotweenAnimController))]
    public class NotifyListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header(">>> [Event] - Dotween結束時關閉Invoke")]
        public UnityEvent<NotifyListItem> onCloseEvent = new UnityEvent<NotifyListItem>();

        #region [>>> Components]
        private DotweenAnimController _animController { get; set; }
        private DotweenAnimController animController => _animController ??= GetComponent<DotweenAnimController>();
        protected Transform container => _container ??= transform.GetChild(0).transform.Find("Container");
        private Transform _container;
        private Button _buttonClose { get; set; }
        private Button buttonClose => _buttonClose ??= container.Find("ButtonClose").GetComponent<Button>();
        private Button _buttonRow { get; set; }
        protected Button buttonRow => _buttonRow ??= transform.GetChild(0).GetComponent<Button>();
        #endregion

        #region [>>> 事件處理 - 關閉、Dotween控制器]
        public void OnPointerEnter(PointerEventData eventData) => animController.Pause();
        public void OnPointerExit(PointerEventData eventData) => animController.Resume();

        private void OnEnable()
        {
            buttonClose.onClick.AddListener(() => onCloseEvent.Invoke(this));
            animController.onDotweenFinished.AddListener((data) => onCloseEvent.Invoke(this));
            buttonRow.onClick.RemoveAllListeners();
        }
        private void OnDisable()
        {
            buttonClose.onClick.RemoveAllListeners();
            animController.onDotweenFinished.RemoveAllListeners();
        }
        #endregion
    }
}