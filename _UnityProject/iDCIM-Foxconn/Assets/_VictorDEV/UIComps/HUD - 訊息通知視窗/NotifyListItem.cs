using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace VictorDev.Common
{
    /// <summary>
    /// [HUD - 訊息通知視窗] - 訊息列表項目
    /// </summary>
    [RequireComponent(typeof(DotweenAnimController))]
    public abstract class NotifyListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header(">>> [資料項]")]
        [SerializeField] private INotifyData _data;
        public INotifyData data => _data;

        [Header(">>> [Event] - 點擊項目時Invoke")]
        public UnityEvent<INotifyData> onClickEvent = new UnityEvent<INotifyData>();
        [Header(">>> [Event] - Dotween結束時關閉Invoke")]
        public UnityEvent<NotifyListItem> onCloseEvent = new UnityEvent<NotifyListItem>();

        #region [>>> Components]
        private DotweenAnimController _animController { get; set; }
        private DotweenAnimController animController => _animController ??= GetComponent<DotweenAnimController>();

        private Button _buttonRow { get; set; }
        private Button buttonRow => _buttonRow ??= transform.GetChild(0).GetComponent<Button>();

        private Transform _container { get; set; }
        protected Transform container => _container ??= buttonRow.transform.Find("Container");

        private TextMeshProUGUI _txtTitle { get; set; }
        private TextMeshProUGUI txtTitle => _txtTitle ??= container.Find("txtTitle").GetComponent<TextMeshProUGUI>();

        private Button _buttonClose { get; set; }
        private Button buttonClose => _buttonClose ??= container.Find("ButtonClose").GetComponent<Button>();
        #endregion

        /// <summary>
        /// 設定資料
        /// </summary>
        public void ShowData(string title, INotifyData data)
        {
            txtTitle.SetText(title.Trim());
            _data = data;
            OnShowData(data);
        }
        /// <summary>
        /// 客製作設定
        /// </summary>
        protected abstract void OnShowData(INotifyData data);

        #region [>>> 事件處理]
        public void OnPointerEnter(PointerEventData eventData) => animController.Pause();
        public void OnPointerExit(PointerEventData eventData) => animController.Resume();

        private void OnEnable()
        {
            buttonRow.onClick.AddListener(() => onClickEvent.Invoke(data));
            buttonClose.onClick.AddListener(() => onCloseEvent.Invoke(this));
            animController.onDotweenFinished.AddListener((data) => onCloseEvent.Invoke(this));
        }
        private void OnDisable()
        {
            buttonRow.onClick.RemoveAllListeners();
            buttonClose.onClick.RemoveAllListeners();
            animController.onDotweenFinished.RemoveAllListeners();
        }
        #endregion
    }
    public interface INotifyData { }
}