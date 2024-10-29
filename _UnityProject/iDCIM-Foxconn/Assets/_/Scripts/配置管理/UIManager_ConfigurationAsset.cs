using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// [配置管理]
/// </summary>
public class UIManager_ConfigurationAsset : MonoBehaviour
{

    [Header(">>> [資料項] Switch")]
    [SerializeField] private List<Data_DeviceAsset> switchDataList;
    [Header(">>> [資料項] Router")]
    [SerializeField] private List<Data_DeviceAsset> routerDataList;
    [Header(">>> [資料項] Server")]
    [SerializeField] private List<Data_DeviceAsset> serverDataList;

    [Header(">>> UI組件")]
    [SerializeField] private GameObject uiObj;
    [SerializeField] private DeviceModelVisualizer deviceModelVisualizer;

    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;
            uiObj.SetActive(value);
        }
    }

    
}
