using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

/// <summary>
/// [父類別] - 圖標物件
/// <para>+ 負責圖標Toggle點選事件，Invoke資料ILandmarkData</para>
/// </summary>
public abstract class LandmarkHandler<T> : MonoBehaviour, ILandmarkHandler where T : ILandmarkData
{
    [Header(">>> [資料項] ")]
    [SerializeField] private T _data;
    public T data => _data;

    private Image imgGradient { get; set; }

    [Header(">>> 選取中的顏色")]
    [SerializeField] protected Color selectedColor = ColorHandler.HexToColor(0x5191F1);

    [Header(">>> [僅顯示] 目標對像")]
    [SerializeField] private Transform targetObject;

    [Header(">>> ToggeOn為True時Invoke (圖標物件)")]
    public UnityEvent<bool, ILandmarkHandler> onToggleEvent = new UnityEvent<bool, ILandmarkHandler>();

    [Header(">>> 地標高度")]
    [SerializeField] private float _offsetHeight;

    public float offsetHeight => _offsetHeight;
    public Transform targetModel
    {
        get => targetObject;
        set => targetObject = value;
    }
    public RectTransform rectTransform => GetComponent<RectTransform>();
    public float distanceToCamera { get; set; } = 0;


    [Header(">>> 組件")]
    [SerializeField] private Toggle toggle;

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    private void Start() => InitToggleListener();

    public void ShowData(ILandmarkData data, Transform targetModel)
    {
        this._data = (T)data;
        this.targetModel = targetModel;
        OnShowDataHandler((T)data);

        if (RaycastHitManager.CurrentSelectedObject == targetModel) SetToggleOnWithoutNotify();
    }
    protected abstract void OnShowDataHandler(T data);

    public void SetToggleOnWithoutNotify(bool isOn = true)
    {
        toggle.onValueChanged.RemoveAllListeners();
        ChangeColor(isOn);
        toggle.isOn = isOn;
        InitToggleListener();
    }

    private void InitToggleListener()
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            onToggleEvent.Invoke(isOn, this);
            ChangeColor(isOn);
            if (isOn) OnToggleOnHandler();
            else OnToggleOffHandler();
        });
    }

    private void ChangeColor(bool isOn)
    {
        imgGradient ??= transform.GetChild(0).GetComponent<Image>();
        imgGradient.color = isOn ? selectedColor : Color.black;
    }

    protected abstract void OnToggleOnHandler();
    protected abstract void OnToggleOffHandler();
}

public interface ILandmarkHandler
{
    Transform targetModel { get; set; }
    float offsetHeight { get; }
    RectTransform rectTransform { get; }
    float distanceToCamera { get; set; }

    void SetToggleOnWithoutNotify(bool isOn = true);
}
public interface ILandmarkData
{
    string DevicePath { get; }
}