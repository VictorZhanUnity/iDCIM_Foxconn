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
    [SerializeField] private Image rtspScreen;
    [SerializeField] private TextMeshProUGUI txtDeviceName, txtDevicePath, txtIdNumber, txtDescription, txtRTSP_URL;
    [SerializeField] private DoTweenFadeController doTweenFadeController;

    private void Start()
    {
        btnClose.onClick.AddListener(doTweenFadeController.ToHide);
        doTweenFadeController.OnHideEvent.AddListener(() => canvasObj.SetActive(false));
    }

    public void Show(Data_RTSP data)
    {
        this.data = data;

        txtDeviceName.SetText(data.name);
        txtDevicePath.SetText(data.devicePath);
        txtIdNumber.SetText(data.idNumber);
        txtDescription.SetText(data.deviceInformation.description);
        txtRTSP_URL.SetText(data.deviceInformation.rtsp_connection_string);

        canvasObj.SetActive(true);
        doTweenFadeController.ToShow();

        ConnectRTSP();
    }

    private void ConnectRTSP()
    {

    }

    public void Show(SO_RTSP data)
    {

    }

}
