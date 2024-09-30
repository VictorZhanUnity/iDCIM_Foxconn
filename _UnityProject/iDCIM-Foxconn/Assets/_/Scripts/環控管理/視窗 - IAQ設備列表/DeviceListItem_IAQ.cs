using UnityEngine;
using UnityEngine.UI;

public class DeviceListItem_IAQ : ListItem
{
    [SerializeField] private Image imgView;

    /// <summary>
    /// 目前是否已顯示與畫面上
    /// </summary>
    public bool isDisplay { set => imgView.enabled = value; }

    public override void SetIsOnWithoutNotify(bool isOn)
    {
        base.SetIsOnWithoutNotify(isOn);
    }
}
