using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;

public class Panel_CCTV : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtTitle;
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

    public UnityEvent<Sprite> onClickScale = new UnityEvent<Sprite>();
    public UnityEvent onDragged = new UnityEvent();
    public UnityEvent<SO_RTSP> onClose = new UnityEvent<SO_RTSP>();

    public SO_RTSP data;

    private Vector3 originalPos;
    private Color originalColor;

    private void Awake()
    {
        originalPos = doTweenFade.transform.position;
        originalColor = border.color;
        canvasBlur.worldCamera = Camera.main;
    }


    private void OnEnable()
    {
        btnScale.onClick.AddListener(() => onClickScale.Invoke(screen.sprite));
        dragHandler.onDragged.AddListener(OnDragHandler);
        doTweenFade.OnFadeOutEvent.AddListener(CloseHandler);
    }

    private void OnDisable()
    {
        onClickScale.RemoveAllListeners();
        onDragged.RemoveAllListeners();
        onClose.RemoveAllListeners();
        btnScale.onClick.RemoveListener(() => onClickScale.Invoke(screen.sprite));
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


    public void ShowData()
    {
        if (data != null) ShowData(data);
    }
    public void ShowData(SO_RTSP data)
    {
        this.data = data;
        txtTitle.SetText(data.title);
        screen.sprite = data.sprite;
        doTweenFade.FadeIn();
    }

    /// <summary>
    /// Ãö³¬
    /// </summary>
    public void Close()
    {
        doTweenFade.FadeOut();
        onClose.Invoke(data);
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

    private void OnValidate()
    {
        if (data != null) ShowData(data);
    }
}
