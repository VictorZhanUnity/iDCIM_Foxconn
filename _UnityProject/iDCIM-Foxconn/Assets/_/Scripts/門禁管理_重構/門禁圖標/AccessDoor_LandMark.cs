using TMPro;
using UnityEngine;

public class AccessDoor_LandMark : LandmarkHandler<Data_AccessRecord>
{
    [SerializeField] private TextMeshProUGUI txtIdNumber, txtAmountofEntry;

    protected override void OnShowDataHandler(Data_AccessRecord data)
    {
        txtIdNumber.SetText(data.DevicePathBySplit);
        txtAmountofEntry.SetText(data.listOfToday.users.Count.ToString() + "¤H");
    }

    protected override void OnToggleOnHandler()
    {
    }
    protected override void OnToggleOffHandler()
    {
    }
}
