using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTV_InfoPanel : InfoPanel<Data_RTSP>
{
    [Space(20)]
    [SerializeField] private Image rtspScreen;
    [SerializeField] private TextMeshProUGUI txtDeviceName, txtDevicePath, txtIdNumber, txtDescription, txtRTSP_URL;

    protected override void OnShowDataHandler(Data_RTSP data)
    {
        txtDeviceName.SetText(data.name);
        txtDevicePath.SetText(data.devicePath);
        txtIdNumber.SetText(data.idNumber);
        txtDescription.SetText(data.deviceInformation.description);
        txtRTSP_URL.SetText(data.deviceInformation.rtsp_connection_string);

        ConnectRTSP();
    }

    private void ConnectRTSP()
    {
    }

    protected override void OnCloseHandler(Data_RTSP data)
    {
        //ObjectPoolManager.PushToPool(this);
#if UNITY_EDITOR
        DestroyImmediate(this);

        Debug.Log("Called");
#else
        Destroy(this);
#endif
    }
}
