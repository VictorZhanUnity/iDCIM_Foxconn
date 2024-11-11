using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.Net.WebAPI;

/// <summary>
/// [°t¸mºÞ²z] DeviceConfigure
/// </summary>
public class DeviceConfigureManager : iDCIM_ModuleManager
{
    [Header(">>> WebAPI")]
    [SerializeField] DeviceConfigure_WebAPI webAPI;



    protected override void OnShowHandler()
    {
        //GetAllStockDevice();
    }

    protected override void OnCloseHandler()
    {
    }

}
