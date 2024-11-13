using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.Common;
using static CCTVManager;

public class Panel_CCTV : MonoBehaviour
{
    [Header(">>> [WebAPI資料項] CCTV連線資訊")]
    [SerializeField] private Data_RTSP _data;
    public Data_RTSP data => _data;

    [Header(">>> 組件")]


    [Space(20)]


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
      //  btnScale.onClick.AddListener(() => onClickScale.Invoke(_data));
        dragHandler.onDragged.AddListener(OnDragHandler);
        doTweenFade.OnHideEvent.AddListener(CloseHandler);
    }

    private void OnDisable()
    {
        onClickScale.RemoveAllListeners();
        onDragged.RemoveAllListeners();
        onClose.RemoveAllListeners();
       // btnScale.onClick.RemoveListener(() => onClickScale.Invoke(_data));
        dragHandler.onDragged.RemoveListener(OnDragHandler);
        doTweenFade.OnHideEvent.RemoveListener(CloseHandler);
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

    /// <summary>
    /// [ForInspector]顯示視窗
    /// </summary>
    public void ShowData()
    {
        if (_data != null) ShowData(_data);
    }

    public void ShowData(Data_RTSP data)
    {
        this._data = data;
        //txtTitle.SetText(data.);
        txtDeviceName.SetText(data.name);
        
        doTweenFade.ToShow();

        if (listItem != null) listItem.isDisplay = true;
    }

    /// <summary>
    /// 關閉
    /// </summary>
    public void Close()
    {
        doTweenFade.ToHide();
      //  onClose.Invoke(_data);

        listItem.isDisplay = false;
    }

    public void CloseWithoutInvoke()
    {
        doTweenFade.OnHideEvent.RemoveListener(CloseHandler);
        doTweenFade.ToHide();
    }

    public void ToShining()
    {
        ColorHandler.LerpColor(border, Color.red, originalColor);
      //  pointerEventHandler.MoveToFront();
    }
}
