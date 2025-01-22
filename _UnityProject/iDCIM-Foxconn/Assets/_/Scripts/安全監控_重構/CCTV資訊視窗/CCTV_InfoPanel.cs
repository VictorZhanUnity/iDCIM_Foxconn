using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.RTSP;

public class CCTV_InfoPanel : InfoPanel<SoData_RTSP_Channel>
{
    [Space(20)]
    [Header(">>> �I�����ù����s��Invoke")]
    public UnityEvent<CCTV_InfoPanel> onClickZoomButtn = new UnityEvent<CCTV_InfoPanel>();

    [Header(">>> �ե�")]
    [SerializeField] private Button btnZoom;
    [SerializeField] private RtspScreen rtspScreen;

    /// �B�z��u
    public RectTransform lineSegment1;   // �Ĥ@�q�u�� (�ҫ� -> �������I)
    public RectTransform lineSegment2;   // �ĤG�q�u�� (���I -> UI)
    public Vector2 offset = new Vector2(50, 50); // ��u�����I�����q
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
        //lineSegment1.transform.SetAsFirstSibling();
       // lineSegment2.transform.SetAsFirstSibling();

       lineSegment1.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        lineSegment2.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
    }

    protected override void OnShowDataHandler(SoData_RTSP_Channel data)
    {
    txtDeviceName.SetText(data.name);
        /*  txtDevicePath.SetText(data.devicePath);
      txtIdNumber.SetText(data.idNumber);
      txtDescription.SetText(data.deviceInformation.description);
      txtRTSP_URL.SetText(data.deviceInformation.rtsp_connection_string);
*/
        rtspScreen.Play(data.RTSP_URL);
        txtRTSP_URL.SetText(data.RTSP_URL);
    }

    public UnityEvent<CCTV_InfoPanel> onCloseEvent = new UnityEvent<CCTV_InfoPanel>();
    protected override void OnCloseHandler(SoData_RTSP_Channel data)
    {
        //ObjectPoolManager.PushToPool(this);
        rtspScreen.Stop();
        onCloseEvent?.Invoke(this);
#if UNITY_EDITOR
        DestroyImmediate(gameObject);
#else
        Destroy(gameObject);
#endif
    }


    void Update()
    {
        // 1. 3D座標轉換螢幕座標
        Vector3 modelScreenPos = mainCamera.WorldToScreenPoint(targetModel.position);
        // 2. �ù��y����Canvas�����y��
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            modelScreenPos,
            parentCanvasCamera,
            out Vector2 modelCanvasPos
        );

        // 3. UI����Canvas�����y��
        Vector2 dragUICanvasPos = selfRect.anchoredPosition;

        // 4. �p����I����m
        Vector2 midPoint = (modelCanvasPos + dragUICanvasPos) / 2 + offset;

        // 5. ��s�Ĥ@�q�u�� (�ҫ� -> ���I)
       // UpdateLine(lineSegment1, modelCanvasPos, midPoint);

        // 6. ��s�ĤG�q�u�� (���I -> UI)
       // UpdateLine(lineSegment2, midPoint, dragUICanvasPos);
    }

    // ��s��q�u��
    void UpdateLine(RectTransform line, Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float length = direction.magnitude; // �p��u������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // �p��u������

        line.anchoredPosition = start; // �_�I��m
        line.rotation = Quaternion.Euler(0, 0, angle); // ���ਤ��
        line.sizeDelta = new Vector2(length, line.sizeDelta.y); // ��s�u������
    }

    private void OnDestroy()
    {
        btnZoom.onClick.RemoveAllListeners();
    }
}
