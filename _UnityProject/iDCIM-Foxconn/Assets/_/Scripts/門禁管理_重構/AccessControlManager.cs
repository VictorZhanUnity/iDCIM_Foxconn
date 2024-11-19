using System.Collections.Generic;
using UnityEngine;
using VictorDev.Advanced;

/// <summary>
/// [門禁管理] UI與操作流程處理
/// </summary>
[RequireComponent(typeof(DataHandler_AccessRecord))]
public class AccessControlManager : iDCIM_ModuleManager
{
    [Header(">>> [資料項]")]
    [SerializeField] private List<Data_AccessRecord> datas;

    [Header(">>> Landmark圖標Prefab")]
    [SerializeField] private AccessDoor_LandMark landmarkPrefab;


    /// <summary>
    ///  [資料處理] 門禁記錄資訊
    /// </summary>
    private DataHandler_AccessRecord dataHandler { get; set; }

    private void Awake()
    {
        dataHandler = GetComponent<DataHandler_AccessRecord>();
    }

    protected override void OnShowHandler()
    {
        void onFailed(long responseCode, string msg) { }
        void onSuccess(List<Data_AccessRecord> result)
        {
            datas = result;
            ShowLandmarks();
        }

        dataHandler.GetAccessRecordsOfThisYear(onSuccess, onFailed);
    }

    protected override void OnCloseHandler()
       => LandmarkManager_RE.RemoveLandmarks<AccessDoor_LandMark, Data_AccessRecord>();

    /// <summary>
    /// 顯示圖標
    /// </summary>
    private void ShowLandmarks()
      => LandmarkManager_RE.AddLandMarks(landmarkPrefab, dataHandler.datas, modelList);

    [ContextMenu("- [目前年份] 門禁記錄")] private void Test_GetYear() => dataHandler.GetAccessRecordsOfThisYear((result) => datas = result, null);
    [ContextMenu("- [目前月份] 門禁記錄")] private void Test_GetMonth() => dataHandler.GetAccessRecordsOfThisMonth((result) => datas = result, null);
    [ContextMenu("- [今天] 門禁記錄")] private void Test_GetToday() => dataHandler.GetAccessRecordsOfToday((result) => datas = result, null);
}
