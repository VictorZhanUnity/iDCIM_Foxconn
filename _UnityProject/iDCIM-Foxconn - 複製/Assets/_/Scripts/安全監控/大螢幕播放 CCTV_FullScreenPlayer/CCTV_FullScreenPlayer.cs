using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTV_FullScreenPlayer : MonoBehaviour
{
    [SerializeField] private Image cctvScreen;
    [SerializeField] private TextMeshProUGUI txtDeviceName;
    [SerializeField] private GameObject canvasObj;

    public SO_RTSP data;

    private void Start()
    {
        canvasObj.SetActive(false);
    }

    public void Show(SO_RTSP data)
    {
        this.data = data;
        txtDeviceName.SetText(data.title);
        cctvScreen.sprite = data.sprite;
        canvasObj.SetActive(true);
    }

    public void Hide()
    {
        canvasObj.SetActive(false);
    }
}
