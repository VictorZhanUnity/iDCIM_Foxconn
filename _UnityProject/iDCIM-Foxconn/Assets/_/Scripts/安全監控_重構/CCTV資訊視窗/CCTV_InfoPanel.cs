using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.RTSP;

public class CCTV_InfoPanel : InfoPanel<Data_RTSP>
{
    [Space(20)]
    [Header(">>> �I�����ù����s��Invoke")]
    public UnityEvent<CCTV_InfoPanel> onClickZoomButtn = new UnityEvent<CCTV_InfoPanel>();

    [Header(">>> �ե�")]
    [SerializeField] private Button btnZoom;
    [SerializeField] private RtspScreen rtspScreen;

    public RectTransform lineImage;      // �Ψӧ@���s���u���� UI Image
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
        // 1. ���3D�ҫ����ù��y��
        Vector3 modelScreenPos = Camera.main.WorldToScreenPoint(targetModel.position);

        // 2. �N�ù��y���ഫ��Canvas�����y��
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            modelScreenPos,
            transform.parent.GetComponent<Canvas>().worldCamera,
            out Vector2 modelCanvasPos
        );

        // 3. ���UI�������y��
        Vector2 dragUICanvasPos = GetComponent<RectTransform>().anchoredPosition;

        // 4. �p��u�������שM���ਤ��
        Vector2 direction = dragUICanvasPos - modelCanvasPos;
        float length = direction.magnitude; // �u������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // �u������

        // 5. ��s�u����Transform�ݩ�
        lineImage.anchoredPosition = modelCanvasPos; // �u�����_�I
        lineImage.rotation = Quaternion.Euler(0, 0, angle); // �u������V
        lineImage.sizeDelta = new Vector2(length, lineImage.sizeDelta.y); // ��s�u��������
    }

    private void OnDestroy()
    {
        btnZoom.onClick.RemoveAllListeners();
    }
}
