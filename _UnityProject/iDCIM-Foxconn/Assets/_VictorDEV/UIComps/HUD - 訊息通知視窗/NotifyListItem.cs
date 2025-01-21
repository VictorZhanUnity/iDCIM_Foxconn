using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VictorDev.DoTweenUtils;

namespace VictorDev.Common
{
    /// [HUD - 訊息通知視窗] - 訊息列表項目
    [RequireComponent(typeof(DotweenAnimController))]
    public class NotifyListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("[Event] - Dotween結束時 / 關閉時 Invoke")]
        public UnityEvent<NotifyListItem> onCloseEvent = new UnityEvent<NotifyListItem>();

        /// 關閉
        public void Close(bool isInvokeEvent = true)
        {
            if(isInvokeEvent) onCloseEvent.Invoke(this);
            Destroy(gameObject);
        }
        
        #region Intiialize
        private void OnEnable()
        {
            ButtonClose.onClick.AddListener(()=>Close(true));
           // AnimController.onDotweenFinished.AddListener((obj)=>Close(true));
        }
        private void OnDisable()
        {
            ButtonRow.onClick.RemoveAllListeners();
            ButtonClose.onClick.RemoveListener(()=>Close(true));
           // AnimController.onDotweenFinished.RemoveListener((obj)=>Close(true));
        }
        public void OnPointerEnter(PointerEventData eventData) => AnimController.Pause();
        public void OnPointerExit(PointerEventData eventData) => AnimController.Resume();
        #endregion
        
        #region Components
        /// 動畫控制
        private DotweenAnimController AnimController => _animController ??= GetComponent<DotweenAnimController>();
        private DotweenAnimController _animController;
        
        protected Button ButtonRow => _buttonRow ??= transform.GetChild(0).GetComponent<Button>();
        private Button _buttonRow;
        
        protected Transform Container => _container ??= ButtonRow.transform.transform.Find("Container");
        private Transform _container;
        /// 關閉按鈕
        private Button ButtonClose => _buttonClose ??= Container.Find("ButtonClose").GetComponent<Button>();
        private Button _buttonClose;
       
        #endregion
    }
}