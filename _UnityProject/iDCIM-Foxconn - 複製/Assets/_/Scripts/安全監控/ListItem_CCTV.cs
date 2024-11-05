using UnityEngine;
using UnityEngine.UI;

public class ListItem_CCTV : ListItem
{
    public Landmark minimapLandmark;

    [SerializeField] Image imgView;

    /// <summary>
    /// �ثe�O�_�w��ܻP�e���W
    /// </summary>
    public bool isDisplay { set => imgView.enabled = value; }

    public override void SetIsOnWithoutNotify(bool isOn)
    {
        base.SetIsOnWithoutNotify(isOn);
        minimapLandmark.SetToggleIsOnWithNotify(isOn);
    }
}
