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
        throw new NotImplementedException();
    }

    protected override void OnCloseHandler()
    {
        throw new NotImplementedException();
    }

    protected override void OnShowHandler()
    {
        throw new NotImplementedException();
    }

    protected override void RemoveEventListener()
    {
        throw new NotImplementedException();
    }

    private void GetAllDeviceAssets()
    {
    }

}
