using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static VictorDev.RevitUtils.RevitHandler;

public class DeviceAssetToolTip : MonoBehaviour, IToolTipPanel
{
    [Header(">>> [資料項]")]
    [SerializeField] private Data_DeviceAsset data;

    [Header(">>> 是否顯示中")]
    [SerializeField] private bool _isOn;

    [Header(">>> UI組件")]
    [SerializeField] private List<GameObject> deviceImgTags;
    [SerializeField] private TextMeshProUGUI txtDeviceName, txtRackLocation;
    [SerializeField] private DoTweenFadeController fadeController;

    public bool isOn => _isOn;
    public Vector2 sizeDelta => GetComponent<RectTransform>().sizeDelta;

    public void ShowData(IToolTipPanel_Data data)
    {
        _isOn = true;
        if (data is Data_DeviceAsset deviceAsset)
        {
            txtDeviceName.SetText(deviceAsset.deviceName);
            txtRackLocation.SetText($"第 {deviceAsset.rackLocation} U");

            deviceImgTags.ForEach(tag => tag.SetActive(tag.name.Contains(deviceAsset.system)));
        }
        fadeController.FadeIn(true);
    }

    public void Close()
    {
        _isOn = false;
        fadeController.FadeOut();
    }

    public void UpdatePosition(Vector2 position) => transform.position = position;
}