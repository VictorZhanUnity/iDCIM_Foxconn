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

    [SerializeField] private PointerEventHandler pointerEventHandler;
    [SerializeField] private DoTweenFadeController doTweenFade;
    [SerializeField] private DragPanel dragHandler;
    [SerializeField] private RectTransformResizeLerp resizer;
    [SerializeField] private Image border;

    public UnityEvent<Sprite> onClickScale = new UnityEvent<Sprite>();
    public UnityEvent onDragged = new UnityEvent();
    public UnityEvent<SO_RTSP> onClose = new UnityEvent<SO_RTSP>();

    private Vector3 originalPos;
    private Color originalColor;

    private void Awake()
    {
        originalPos = doTweenFade.transform.position;
        originalColor = border.color;
    }

    public SO_RTSP data { get; private set; }

    private void OnEnable()
    {
        btnScale.onClick.AddListener(() => onClickScale.Invoke(screen.sprite));
        dragHandler.onDragged.AddListener(onDragged.Invoke);
        doTweenFade.OnFadeOutEvent.AddListener(CloseHandler);
    }

    private void OnDisable()
    {
        onClickScale.RemoveAllListeners();
        onDragged.RemoveAllListeners();
        onClose.RemoveAllListeners();
        btnScale.onClick.RemoveListener(() => onClickScale.Invoke(screen.sprite));
        dragHandler.onDragged.RemoveListener(onDragged.Invoke);
        doTweenFade.OnFadeOutEvent.RemoveListener(CloseHandler);
    }

    public void CloseHandler()
    {
        doTweenFade.transform.position = originalPos;
        print($"originalPos: {originalPos}");
        togglePin.isOn = false;
        toggleContent.isOn = false;
        resizer.Restore();
        ObjectPoolManager.PushToPool<Panel_CCTV>(this);
    }

    public void Show(SO_RTSP data)
    {
        this.data = data;
        txtTitle.SetText(data.title);
        screen.sprite = data.sprite;
        doTweenFade.FadeIn();
    }
    public void Close()
    {
        doTweenFade.FadeOut();
        onClose.Invoke(data);
    }

    public void ToShining()
    {
        ColorHandler.LerpColor(border, Color.red, originalColor);
        pointerEventHandler.MoveToFront();
    }
}
