using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Net.WebAPI;

public class DeviceAssetManager : ModulePage
{
    public override void OnInit(Action onInitComplete = null)
    {
        onInitComplete?.Invoke();
    }

    protected override void InitEventListener()
    {
    }

    protected override void OnCloseHandler()
    {
    }

    protected override void OnShowHandler()
    {
    }

    protected override void RemoveEventListener()
    {
    }

    private void GetAllDeviceAssets()
    {
    }

}
