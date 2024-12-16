using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 圖標組件
/// <para>+ 直接掛載到目標UI上，再丟給LandmarkManager進行事件註冊即可</para>
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class Landmark_RE : MonoBehaviour
{
    [Header(">>> 目標模型")]
    public Transform targetModel;

    [Header(">>> 地標高度")]
    public float offsetHeight = 0;

    /// <summary>
    /// Toggle狀態變更時Invoke {isOn, 目標模型}
    /// </summary>
    public UnityEvent<bool, Transform> onToggleValueChanged { get; set; } = new UnityEvent<bool, Transform>();

    public bool isOn { set => toggle.isOn = value; }

    [HideInInspector]
    public float distanceToCamera { get; set; } = 0;

    private float originalPosY { get; set; }
    private void Awake() => originalPosY = transform.localPosition.y;

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener((isOn) => onToggleValueChanged?.Invoke(isOn, targetModel));
        ShowEffect();
    }

    private void OnDisable() => toggle.onValueChanged.RemoveAllListeners();

    /// <summary>
    /// 設定Toggle.isOn狀態
    /// <para>+ isInvokeEvent: 是否觸發Toggle事件</para>
    /// </summary>
    public void SetToggleStatus(bool isOn, bool isInvokeEvent = false)
    {
        if (isInvokeEvent == false) OnDisable();
        toggle.isOn = isOn;
        //OnEnable();
    }

    #region [顯示特效設定]
    /// <summary>
    /// 顯示特效
    /// </summary>
    private void ShowEffect()
    {
        float delay = Random.Range(0f, 0.2f);
        uiObject.DOLocalMoveY(0, 0.3f).From(posY)
            .SetEase(ease).SetDelay(delay);
        canvasGroup.DOFade(1, 0.3f).From(0)
          .SetEase(ease).SetDelay(delay).OnUpdate(() =>
          {
              canvasGroup.interactable = canvasGroup.blocksRaycasts = canvasGroup.alpha == 1;
          });
    }
    private Ease ease = Ease.OutBack;
    private float posY = 100;
    private float duration = 1f;
    #endregion

    #region [Components]
    private RectTransform _rectTransform { get; set; }
    public RectTransform rectTransform => _rectTransform ??= GetComponent<RectTransform>();
    private CanvasGroup _canvasGroup { get; set; }
    public CanvasGroup canvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

    private Transform _uiObject { get; set; }
    private Transform uiObject => _uiObject ??= transform.GetChild(0);

    private Toggle _toggle { get; set; }
    public Toggle toggle => _toggle ??= uiObject.Find("Toggle").GetComponent<Toggle>();
    public ToggleGroup toggleGroup { set => toggle.group = value; }
    #endregion
}

