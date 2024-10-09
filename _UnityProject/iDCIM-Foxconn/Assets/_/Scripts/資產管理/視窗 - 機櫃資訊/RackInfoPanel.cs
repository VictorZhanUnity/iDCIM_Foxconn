using System;
using TMPro;
using UnityEngine;
using VictorDev.RevitUtils;

/// <summary>
/// 視窗 - 機櫃資訊
/// </summary>
public class RackInfoPanel : MonoBehaviour
{
    [SerializeField] private DoTweenFadeController doTweenFadeController;
    [SerializeField] private RackRUList rackRUList;
    [SerializeField] private TextMeshProUGUI txtLabel;

    public void Close() => doTweenFadeController.FadeOut();

    private void Start()
    {
        rackRUList.OnClickItemEvent.AddListener(ShowCOBieInfo);
    }

    private void ShowCOBieInfo(DeviceRUItem item)
    {
        GameManager.ToSelectTarget(item.DeviceModel);
    }

    public void Show(Demo_Rack data)
    {
        txtLabel.SetText(RevitHandler.GetRackModelName(data.rack.name));
        rackRUList.ShowRULayout(data.Devices);
        doTweenFadeController.FadeIn();
    }
}