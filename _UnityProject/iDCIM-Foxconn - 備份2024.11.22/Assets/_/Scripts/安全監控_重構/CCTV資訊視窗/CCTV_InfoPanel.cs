using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.RTSP;

public class CCTV_InfoPanel : InfoPanel<Data_RTSP>
{
    [Space(20)]
    [Header(">>> 點擊全螢幕按鈕時Invoke")]
    public UnityEvent<CCTV_InfoPanel> onClickZoomButtn = new UnityEvent<CCTV_InfoPanel>();

    [Header(">>> 組件")]
    [SerializeField] private Button btnZoom;
    [SerializeField] private RtspScreen rtspScreen;

    /// 處理折線
    public RectTransform lineSegment1;   // 第一段線條 (模型 -> 中間拐點)
    public RectTransform lineSegment2;   // 第二段線條 (拐點 -> UI)
    public Vector2 offset = new Vector2(50, 50); // 折線的拐點偏移量
    public Transform targetModel;
    private RectTransform selfRect, parentRect;
    private Camera parentCanvasCamera, mainCamera;

    public RtspScreen RtspScreen => rtspScreen;

    [SerializeField] private TextMeshProUGUI txtDeviceName, txtDevicePath, txtIdNumber, txtDescription, txtRTSP_URL;
    protected override void OnAwakeHandler()
    {
        onClickCloseButton.AddListener((data) =>
        {
            Destroy(lineSegment1.gameObject);
            Destroy(lineSegment2.gameObject);
        });
        btnZoom.onClick.AddListener(() =>
        {
            onClickZoomButtn.Invoke(this);
        });
        onDraggedEvent.AddListener(UpdateLineSiblingIndex);
        selfRect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        parentCanvasCamera = parentRect.GetComponent<Canvas>().worldCamera;
        mainCamera = Camera.main;
    }
    private void UpdateLineSiblingIndex()
    {
        lineSegment1.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        lineSegment2.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
    }

    protected override void OnShowDataHandler(Data_RTSP data)
    {
        txtDeviceName.SetText(data.name);
        txtDevicePath.SetText(data.devicePath);
        txtIdNumber.SetText(data.idNumber);
        txtDescription.SetText(data.deviceInformation.description);
        txtRTSP_URL.SetText(data.deviceInformation.rtsp_connection_string);

        rtspScreen.Play(data.deviceInformation.rtsp_connection_string);
    }

    protected override void OnCloseHandler(Data_RTSP data)
    {
        //ObjectPoolManager.PushToPool(this);
        rtspScreen.Stop();
#if UNITY_EDITOR
        DestroyImmediate(gameObject);
#else
        Destroy(gameObject);
#endif
    }


    void Update()
    {
        // 1. 3D模型的螢幕座標
        Vector3 modelScreenPos = mainCamera.WorldToScreenPoint(targetModel.position);

        // 2. 螢幕座標轉Canvas局部座標
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            modelScreenPos,
            parentCanvasCamera,
            out Vector2 modelCanvasPos
        );

        // 3. UI物件的Canvas局部座標
        Vector2 dragUICanvasPos = selfRect.anchoredPosition;

        // 4. 計算拐點的位置
        Vector2 midPoint = (modelCanvasPos + dragUICanvasPos) / 2 + offset;

        // 5. 更新第一段線條 (模型 -> 拐點)
        UpdateLine(lineSegment1, modelCanvasPos, midPoint);

        // 6. 更新第二段線條 (拐點 -> UI)
        UpdateLine(lineSegment2, midPoint, dragUICanvasPos);
    }

    // 更新單段線條
    void UpdateLine(RectTransform line, Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float length = direction.magnitude; // 計算線條長度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 計算線條角度

        line.anchoredPosition = start; // 起點位置
        line.rotation = Quaternion.Euler(0, 0, angle); // 旋轉角度
        line.sizeDelta = new Vector2(length, line.sizeDelta.y); // 更新線條長度
    }

    private void OnDestroy()
    {
        btnZoom.onClick.RemoveAllListeners();
    }
}
