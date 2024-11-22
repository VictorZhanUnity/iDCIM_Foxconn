using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.DoTweenUtils;

public abstract class InfoPanel<T> : MonoBehaviour
{
    [Header(">>> [資料項] ")]
    [SerializeField] private T _data;
    public T data => _data;

    [Header(">>> 點擊關閉按鈕時Invoke")]
    public UnityEvent<T> onClickCloseButton = new UnityEvent<T>();
    [Header(">>> 拖曳時Invoke")]
    public UnityEvent onDraggedEvent = new UnityEvent();

    [Header(">>> 組件")]
    [SerializeField] private Button btnClose;
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private DragPanel dragPanel;

    public RectTransform containerForDrag { set => dragPanel.ParentRectTransform = value; }

    private void Awake()
    {
        btnClose.onClick.AddListener(doTweenFadeController.ToHide);
        dragPanel.onDragged.AddListener(onDraggedEvent.Invoke);
        doTweenFadeController.OnHideEvent.AddListener(Close);

        OnAwakeHandler();
    }
    protected abstract void OnAwakeHandler();

    private void Close()
    {
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
    private void OnDestroy()
    {
        btnClose.onClick.RemoveAllListeners();
        dragPanel.onDragged.RemoveAllListeners();
        onClickCloseButton.RemoveAllListeners();
        onDraggedEvent.RemoveAllListeners();
    }

    public void ToBlink() => doTweenFadeController.ToShow(true);

    protected abstract void OnShowDataHandler(T data);
    protected abstract void OnCloseHandler(T data);
}
