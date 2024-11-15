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

    public RectTransform lineImage;      // 用來作為連接線條的 UI Image
    public Transform targetModel;

    public RtspScreen RtspScreen => rtspScreen;

    [SerializeField] private TextMeshProUGUI txtDeviceName, txtDevicePath, txtIdNumber, txtDescription, txtRTSP_URL;
    protected override void OnAwakeHandler()
    {
        onClickCloseButton.AddListener((data)=> Destroy(lineImage.gameObject));
        btnZoom.onClick.AddListener(() =>
        {
            onClickZoomButtn.Invoke(this);
        });

        onDraggedEvent.AddListener(UpdateLineSiblingIndex);
    }
    private void UpdateLineSiblingIndex()
    {
        lineImage.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
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
        // 1. 獲取3D模型的螢幕座標
        Vector3 modelScreenPos = Camera.main.WorldToScreenPoint(targetModel.position);

        // 2. 將螢幕座標轉換為Canvas局部座標
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            modelScreenPos,
            transform.parent.GetComponent<Canvas>().worldCamera,
            out Vector2 modelCanvasPos
        );

        // 3. 獲取UI的局部座標
        Vector2 dragUICanvasPos = GetComponent<RectTransform>().anchoredPosition;

        // 4. 計算線條的長度和旋轉角度
        Vector2 direction = dragUICanvasPos - modelCanvasPos;
        float length = direction.magnitude; // 線條長度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 線條角度

        // 5. 更新線條的Transform屬性
        lineImage.anchoredPosition = modelCanvasPos; // 線條的起點
        lineImage.rotation = Quaternion.Euler(0, 0, angle); // 線條的方向
        lineImage.sizeDelta = new Vector2(length, lineImage.sizeDelta.y); // 更新線條的長度
    }

    private void OnDestroy()
    {
        btnZoom.onClick.RemoveAllListeners();
    }
}
