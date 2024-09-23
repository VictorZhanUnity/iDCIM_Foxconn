public class ListItem_CCTV : ListItem
{
    public Landmark minimapLandmark;

    public override void SetIsOnWithoutNotify(bool isOn)
    {
        base.SetIsOnWithoutNotify(isOn);
        minimapLandmark.SetToggleIsOnWithNotify(isOn);
    }
}
