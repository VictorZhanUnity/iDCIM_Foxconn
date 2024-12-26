using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Parser;

/// <summary>
/// Demo本地資料管理器
/// </summary>
public class DemoDataManager : Module
{
    public DeviceConfigureDataManager deviceConfigureManager;
    public DeviceAssetDataManager deviceAssetManager;

    [Header(">>> [JSON檔] - 機櫃設備資料，以第一個為主要讀取")]
    public List<TextAsset> jsonFiles;

    private string strDeviceDatas { get; set; }

    public override void OnInit(Action onInitComplete = null)
    {
        ParseJsonFile();
        deviceConfigureManager?.ParseJson(strDeviceDatas);
        deviceAssetManager?.ParseJson(strDeviceDatas);
        onInitComplete?.Invoke();
    }

    [ContextMenu("- 解析JSON檔")]
    private void ParseJsonFile()
    {
        strDeviceDatas = JsonUtils.PrintJSONFormatting(jsonFiles[0].text);

#if UNITY_EDITOR
        Debug.Log($"ParseJsonFile: {jsonFiles[0].name}\n{strDeviceDatas}");
#endif
    }
}
