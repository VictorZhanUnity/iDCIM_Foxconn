using DG.Tweening;
using TMPro;
using UnityEngine;
using static CCTVManager;

public class CCTV_LandMark : LandmarkHandler<Data_RTSP>
{
    [SerializeField] private TextMeshProUGUI txtIdNumber, txtDescription;

    [Header(">>> 選取中的文字顏色")]
    [SerializeField] private Color selectedColor = Color.yellow;

    protected override void OnShowDataHandler(Data_RTSP data)
    {
        txtIdNumber.SetText(data.idNumber);
        txtDescription.SetText(data.deviceInformation.description);
    }

    protected override void OnToggleOnHandler()
    {
        txtDescription.DOColor(selectedColor, 0.2f);
    }
    protected override void OnToggleOffHandler()
    {
        txtDescription.DOColor(Color.white, 0.2f);
    }
}
