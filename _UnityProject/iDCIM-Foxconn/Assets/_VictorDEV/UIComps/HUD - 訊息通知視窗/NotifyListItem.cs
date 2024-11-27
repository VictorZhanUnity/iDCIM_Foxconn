using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace VictorDev.Common
{
    /// <summary>
    /// [HUD - �T���q������] - �T���C����
    /// </summary>
    [RequireComponent(typeof(DotweenAnimController))]
    public abstract class NotifyListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header(">>> [��ƶ�]")]
        [SerializeField] private INotifyData _data;
        public INotifyData data => _data;

        [Header(">>> [Event] - �I�����خ�Invoke")]
        public UnityEvent<INotifyData> onClickEvent = new UnityEvent<INotifyData>();
        [Header(">>> [Event] - Dotween����������Invoke")]
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
        /// �]�w���
        /// </summary>
        public void ShowData(string title, INotifyData data)
        {
            txtTitle.SetText(title.Trim());
            _data = data;
            OnShowData(data);
        }
        /// <summary>
        /// �Ȼs�@�]�w
        /// </summary>
        protected abstract void OnShowData(INotifyData data);

        #region [>>> �ƥ�B�z]
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