using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;
using VictorDev.CameraUtils;
using static UnityEngine.Rendering.DebugUI;

public class Landmark : MonoBehaviour
{
    [Header(">>> 地標高度")]
    public float offsetHeight;
    [Header(">>> 目標對像")]
    public Transform targetObject;

    [Header(">>> 地標分類")]
    public LandmarkCategory category;

    public ListItem listItem;

    [Header(">>> 當點選Toggle時")]
    public UnityEvent<bool> onToggleChanged = new UnityEvent<bool>();

    [HideInInspector]
    public float distanceToCamera { get; set; } = 0;

    [Header("UI元件")]
    [SerializeField] private Toggle toggle;
    [SerializeField] public RectTransform uiElement;

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    private void Awake()
    {
        toggle.onValueChanged.AddListener((isOn)=> listItem?.SetIsOnWithoutNotify(isOn));
    }

    public void SetToggleIsOnWithNotify(bool isOn)
    {
        toggle.onValueChanged.RemoveListener(onToggleChanged.Invoke);
        toggle.isOn = isOn;
        toggle.onValueChanged.AddListener(onToggleChanged.Invoke);
    }
    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(onToggleChanged.Invoke);
    }

    private void OnDisable()
    {
        try
        {
            toggle.isOn = false;
            toggle.onValueChanged.RemoveListener(onToggleChanged.Invoke);
        }
        catch (Exception e) { }
    }
    /// <summary>
    /// 初始化 {目標模型、圖標位移高度、圖標分類}
    /// </summary>
    public void Initialize(Transform targetObject, float offsetHeight, LandmarkCategory category = default)
    {
        this.targetObject = targetObject;
        this.offsetHeight = offsetHeight;
        this.category = category;

        gameObject.SetActive(false);
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) OrbitCamera.MoveTargetTo(targetObject);
        });
    }

    private void OnValidate()
    {
        uiElement ??= transform.GetComponent<RectTransform>();
    }
}