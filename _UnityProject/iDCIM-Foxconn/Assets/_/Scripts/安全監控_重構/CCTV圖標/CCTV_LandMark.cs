using DG.Tweening;
using TMPro;
using UnityEngine;
using static CCTVManager;

public class CCTV_LandMark : LandmarkHandler<Data_RTSP>
{
    [SerializeField] private TextMeshProUGUI txtIdNumber, txtDescription;

    protected override void OnShowDataHandler(Data_RTSP data)
    {
        txtIdNumber.SetText(data.idNumber);
        txtDescription.SetText(data.deviceInformation.description);
    }

    protected override void OnToggleOnHandler()
    {
    }
    protected override void OnToggleOffHandler()
    {
    }
}
