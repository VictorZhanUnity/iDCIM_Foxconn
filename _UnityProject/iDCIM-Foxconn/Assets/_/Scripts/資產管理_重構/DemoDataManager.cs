using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Managers;
using VictorDev.Parser;

/// <summary>
/// Demo本地資料管理器
/// </summary>
public class DemoDataManager : Module
{
    [Header(">>> [接收器IJsonParser] - 接收JSON檔解析後的json字串")]
    [SerializeField] List<MonoBehaviour> jsonParseReceivers;

    [Header(">>> [JSON檔] - 本地機櫃設備資料，以第一個為主要讀取")]
    [SerializeField] List<TextAsset> jsonFiles;

    [Header(">>> 是否在非運行狀態下Invoke資料")]
    [SerializeField] bool isInvokeDataInEditor = false;

    private string jsonDeviceDatas { get; set; }

    public override void OnInit(Action onInitComplete = null)
    {
        ParseJsonFile();
        InvokeData();
        onInitComplete?.Invoke();
    }

    [ContextMenu("- 解析JSON檔")]
    private void ParseJsonFile()
    {
        jsonDeviceDatas = JsonUtils.PrintJSONFormatting(jsonFiles[0].text);
        if (isInvokeDataInEditor) InvokeData();
#if UNITY_EDITOR
        Debug.Log($"ParseJsonFile: {jsonFiles[0].name}\n{jsonDeviceDatas}");
#endif
    }

    /// <summary>
    /// 發送資料
    /// </summary>
    private void InvokeData() => jsonParseReceivers.OfType<IJsonParseReceiver>().ToList().ForEach(receiver =>
    {
        Debug.Log($">>> 發送資料 to [{(receiver as MonoBehaviour).name}]");
        receiver.ParseJson(jsonDeviceDatas);
    });

    private void OnValidate()
    {
        List<MonoBehaviour> exclude = jsonParseReceivers.Where(receiver => receiver is IJsonParseReceiver == false).ToList();
        jsonParseReceivers = jsonParseReceivers.Except(exclude).ToList();
        exclude.ForEach(target => Debug.Log($">>> [{target.name}] 並無實作{typeof(IJsonParseReceiver).Name}, 已從List中移除 !!"));
    }
}
