using DG.Tweening;
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

    private float originalPosY { get; set; }
    private void Awake() => originalPosY = transform.localPosition.y;

    private void OnEnable()
    {
        transform.DOLocalMoveY(originalPosY, 0.1f).From(100)
            .SetEase(Ease.OutBack).SetDelay(Random.Range(0f, 0.3f));
    }
}
