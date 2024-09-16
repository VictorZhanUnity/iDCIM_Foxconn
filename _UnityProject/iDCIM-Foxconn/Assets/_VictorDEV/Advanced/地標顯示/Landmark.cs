using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Advanced;
using static UnityEngine.Rendering.DebugUI;

public class Landmark : MonoBehaviour
{
    [Header(">>> 目標對像的Transform")]
    public Transform targetObject;
    [Header(">>> 地標高度")]
    public float offsetHeight;
    [Header(">>> 地標分類")]
    public LandmarkCategory category;

    [Header(">>> 當點選Toggle時")]
    public UnityEvent<bool> onToggleChanged = new UnityEvent<bool>();

    [HideInInspector]
    public float distanceToCamera { get; set; } = 0;

    [Header("UI元件")]
    [SerializeField] private Toggle toggle;
    [SerializeField] public RectTransform uiElement;

    public ToggleGroup toggleGroup { set => toggle.group = value; }

    private void OnEnable() => toggle.onValueChanged.AddListener(onToggleChanged.Invoke);
    private void OnDisable() => toggle.onValueChanged.RemoveAllListeners();

    /// <summary>
    /// 地標 {建筑物的Transform} {对应的UI元素} {地標高度}
    /// </summary>
    public void Initialize(Transform targetObject, float offsetHeight, LandmarkCategory category = default)
    {
        this.targetObject = targetObject;
        this.offsetHeight = offsetHeight;
        this.category = category;
    }

    private void OnValidate()
    {
        uiElement ??= transform.GetComponent<RectTransform>();
    }
}