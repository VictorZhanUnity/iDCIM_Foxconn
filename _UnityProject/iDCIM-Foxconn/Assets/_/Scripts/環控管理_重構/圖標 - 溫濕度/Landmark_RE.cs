using UnityEngine;
using UnityEngine.UI;

public class Landmark_RE : MonoBehaviour
{
    [Header(">>> 地標高度")]
    public float offsetHeight;
    [Header(">>> 目標對像")]
    public Transform targetModel;

    [HideInInspector]
    public float distanceToCamera { get; set; } = 0;

    #region [Components]
    private RectTransform _rectTransform { get; set; }
    public RectTransform rectTransform => _rectTransform ??= GetComponent<RectTransform>();
    private Toggle _toggle { get; set; }
    public Toggle toggle => _toggle ??= transform.Find("Toggle").GetComponent<Toggle>();
    public ToggleGroup toggleGroup { set => toggle.group = value; }
    #endregion
}

