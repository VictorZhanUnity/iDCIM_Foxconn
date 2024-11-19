using System.Collections.Generic;
using UnityEngine;
using VictorDev.Advanced;

/// <summary>
/// [���T�޲z] UI�P�ާ@�y�{�B�z
/// </summary>
[RequireComponent(typeof(DataHandler_AccessRecord))]
public class AccessControlManager : iDCIM_ModuleManager
{
    [Header(">>> [��ƶ�]")]
    [SerializeField] private List<Data_AccessRecord> datas;

    [Header(">>> Landmark�ϼ�Prefab")]
    [SerializeField] private AccessDoor_LandMark landmarkPrefab;


    /// <summary>
    ///  [��ƳB�z] ���T�O����T
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
    /// ��ܹϼ�
    /// </summary>
    private void ShowLandmarks()
      => LandmarkManager_RE.AddLandMarks(landmarkPrefab, dataHandler.datas, modelList);

    [ContextMenu("- [�ثe�~��] ���T�O��")] private void Test_GetYear() => dataHandler.GetAccessRecordsOfThisYear((result) => datas = result, null);
    [ContextMenu("- [�ثe���] ���T�O��")] private void Test_GetMonth() => dataHandler.GetAccessRecordsOfThisMonth((result) => datas = result, null);
    [ContextMenu("- [����] ���T�O��")] private void Test_GetToday() => dataHandler.GetAccessRecordsOfToday((result) => datas = result, null);
}
