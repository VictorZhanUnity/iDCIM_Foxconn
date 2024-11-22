using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DotweenAutoRotate : MonoBehaviour
{
    [Header(">>> 旋轉圈數，若-1則持續旋轉")]
    [SerializeField] private int loops = -1;
    [Header(">>> 每圈旋轉所需的時間")]
    [SerializeField] private float rotationDuration = 2f;
    [Header(">>> 是否順時針方向")]
    [SerializeField] private bool clockwise = true;

    private RectTransform rect { get; set; }

    private void OnEnable() => StartRotating();
    private void OnDisable() => DOTween.Kill(rect);

    private void StartRotating()
    {
        rect ??= GetComponent<RectTransform>();

        // 計算旋轉角度（順時針為正，逆時針為負）
        float rotationAngle = clockwise ? -360f : 360f;

        rect.DORotate(
            new Vector3(0, 0, rotationAngle), // 最終旋轉角度
            rotationDuration, // 每次旋轉所需的時間
            RotateMode.FastBeyond360 // 旋轉模式，允許超過360度
        )
        .SetEase(Ease.Linear) // 線性動畫，保持恆定速度
        .SetLoops(loops, LoopType.Restart); // 持續循環
    }

    private void OnValidate()
    {
        OnDisable();
        OnEnable();
    }
}
