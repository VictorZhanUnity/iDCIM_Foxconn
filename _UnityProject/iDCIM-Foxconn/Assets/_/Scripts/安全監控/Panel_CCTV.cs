using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

public class Panel_CCTV : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTitle, txtDeviceName;
    [SerializeField] private Image screen;
    [SerializeField] private Button btnScale;
    [SerializeField] private Toggle togglePin, toggleContent;

    public bool isPinOn => togglePin.isOn;

    [SerializeField] private PointerEventHandler pointerEventHandler;
    [SerializeField] private DoTweenFadeController doTweenFade;
    [SerializeField] private DragPanel dragHandler;
    [SerializeField] private RectTransformResizeLerp resizer;
    [SerializeField] private Image border;
    [SerializeField] private Canvas canvasBlur;

    public UnityEvent<SO_RTSP> onClickScale = new UnityEvent<SO_RTSP>();
    public UnityEvent onDragged = new UnityEvent();
    public UnityEvent<SO_RTSP> onClose = new UnityEvent<SO_RTSP>();

    public SO_RTSP data;

    private Vector3 originalPos;
    private Color originalColor;

    public ListItem_CCTV listItem;

    private void Awake()
    {
        originalPos = doTweenFade.transform.position;
        originalColor = border.color;
        canvasBlur.worldCamera = Camera.main;
    }


    private void OnEnable()
    {
        btnScale.onClick.AddListener(() => onClickScale.Invoke(data));
        dragHandler.onDragged.AddListener(OnDragHandler);
        doTweenFade.OnFadeOutEvent.AddListener(CloseHandler);
    }

    private void OnDisable()
    {
        onClickScale.RemoveAllListeners();
        onDragged.RemoveAllListeners();
        onClose.RemoveAllListeners();
        btnScale.onClick.RemoveListener(() => onClickScale.Invoke(data));
        dragHandler.onDragged.RemoveListener(OnDragHandler);
        doTweenFade.OnFadeOutEvent.RemoveListener(CloseHandler);
    }

    private void OnDragHandler()
    {
        onDragged.Invoke();
        togglePin.isOn = true;
    }

    private void CloseHandler()
    {
        doTweenFade.transform.position = originalPos;
        togglePin.isOn = false;
        toggleContent.isOn = false;
        resizer.Restore();
        ObjectPoolManager.PushToPool<Panel_CCTV>(this);
    }

    public void ShowData(SO_RTSP data)
    {
        this.data = data;
        txtTitle.SetText(data.title);
        txtDeviceName.SetText(data.title);
        screen.sprite = data.sprite;
        doTweenFade.FadeIn();

        listItem.isDisplay = true;
    }

    /// <summary>
    /// Ãö³¬
    /// </summary>
    public void Close()
    {
        doTweenFade.FadeOut();
        onClose.Invoke(data);

        listItem.isDisplay = false;
    }

    public void CloseWithoutInvoke()
    {
        doTweenFade.OnFadeOutEvent.RemoveListener(CloseHandler);
        doTweenFade.FadeOut();
    }

    public void ToShining()
    {
        ColorHandler.LerpColor(border, Color.red, originalColor);
        pointerEventHandler.MoveToFront();
    }
}
