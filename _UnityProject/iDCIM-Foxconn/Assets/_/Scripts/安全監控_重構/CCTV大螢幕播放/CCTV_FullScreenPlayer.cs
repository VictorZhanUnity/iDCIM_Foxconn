using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTV_FullScreenPlayer : MonoBehaviour
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_RTSP data;

    [Header(">>> 組件")]
    [SerializeField] private GameObject canvasObj;
    [SerializeField] private Button btnClose;
    [SerializeField] private RawImage rawImgScreen;
    [SerializeField] private TextMeshProUGUI txtDeviceName, txtDevicePath, txtIdNumber, txtDescription, txtRTSP_URL;
    [SerializeField] private DoTweenFadeController doTweenFadeController;

    private CCTV_InfoPanel currentCCTVPanel { get; set; }

    private void Start()
    {
        btnClose.onClick.AddListener(doTweenFadeController.ToHide);
        doTweenFadeController.OnHideEvent.AddListener(() =>
        {
            currentCCTVPanel.RtspScreen.RemoveRenderingTarget(rawImgScreen.gameObject);
            currentCCTVPanel = null;
            canvasObj.SetActive(false);
        });
    }

    public void Show(CCTV_InfoPanel panel)
    {
        currentCCTVPanel = panel;
        data = panel.data;

        txtDeviceName.SetText(data.name);
        txtDevicePath.SetText(data.devicePath);
        txtIdNumber.SetText(data.idNumber);
        txtDescription.SetText(data.deviceInformation.description);
        txtRTSP_URL.SetText(data.deviceInformation.rtsp_connection_string);

        panel.RtspScreen.AddRenderingTarget(rawImgScreen.gameObject);

        canvasObj.SetActive(true);
        doTweenFadeController.ToShow();
    }
}
