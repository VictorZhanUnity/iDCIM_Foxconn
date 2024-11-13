using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

public abstract class InfoPanel<T> : MonoBehaviour
{
    [Header(">>> [資料項] ")]
    [SerializeField] private T _data;
    public T data => _data;

    [Header(">>> 點擊全螢幕按鈕時Invoke")]
    public UnityEvent<T> onClickZoomButtn = new UnityEvent<T>();
    [Header(">>> 點擊關閉按鈕時Invoke")]
    public UnityEvent<T> onClickCloseButton = new UnityEvent<T>();
    [Header(">>> 拖曳時Invoke")]
    public UnityEvent onDraggedEvent = new UnityEvent();

    [Header(">>> 組件")]
    [SerializeField] private Toggle toggleTitlebar;
    [SerializeField] private Button btnZoom, btnClose;
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private DragPanel dragPanel;

    public RectTransform containerForDrag { set => dragPanel.ParentRectTransform = value; }

    private void Start()
    {
        btnZoom.onClick.AddListener(() => onClickZoomButtn.Invoke(data));
        btnClose.onClick.AddListener(doTweenFadeController.ToHide);
        doTweenFadeController.OnHideEvent.AddListener(Close);
        dragPanel.onDragged.AddListener(onDraggedEvent.Invoke);
    }

    private void Close()
    {
        toggleTitlebar.isOn = false;
        onClickCloseButton.Invoke(data);
        OnCloseHandler(data);
    }

    public void ToClose() => btnClose.onClick.Invoke();
    public void ShowData(T data)
    {
        this._data = data;
        OnShowDataHandler(data);
        doTweenFadeController.ToShow();
    }

    protected abstract void OnShowDataHandler(T data);
    protected abstract void OnCloseHandler(T data);
}
