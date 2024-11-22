using System.Collections.Generic;
using UnityEngine;
using VictorDev.Advanced;
using VictorDev.Common;

/// <summary>
/// [���T�޲z] UI�P�ާ@�y�{�B�z
/// </summary>
[RequireComponent(typeof(AccessRecord_DataHandler))]
public class AccessControlManager : iDCIM_ModuleManager
{
    [Header(">>> [��ƶ�] - ���~�ת��T���")]
    [SerializeField] private List<Data_AccessRecord> dataOfThisYear;

    [Header(">>> [Prefab] - Landmark�ϼ�")]
    [SerializeField] private AccessDoor_LandMark landmarkPrefab;

    [Header(">>> [�ե�] - �i���`�H��")]
    [SerializeField] private Comp_AccessRecordTotalCount compTotalCount;
    [Header(">>> [�ե�] - ��ƾ�")]
    [SerializeField] private Comp_AccessRecordCalendar compCalendar;
    [Header(">>> [�ե�] - ���w�Y�骺�i���H�ƹ�")]
    [SerializeField] private Chart_DayCount chartDayCount;
    [Header(">>> [�ե�] - �I����T����")]
    [SerializeField] private AccessDoor_InfoPanel infoPanel;

    /// <summary>
    ///  [��ƳB�z] ���T�O����T
    /// </summary>
    private AccessRecord_DataHandler dataHandler { get; set; }

    private void Awake()
    {
        dataHandler = GetComponent<AccessRecord_DataHandler>();
        dataHandler.onGetAccessRecordOfThisYear.AddListener(compTotalCount.ShowData);
        dataHandler.onGetAccessRecordOfThisYear.AddListener(compCalendar.SetDatas);
        compCalendar.onSelectedDateEvent.AddListener(chartDayCount.ShowData);
        infoPanel.onClickCloseButton.AddListener((data) => RaycastHitManager.CancellObjectSelected(data.DevicePath));
    }

    private void Start()
    {
        LandmarkManager_RE.onToggleOnEvent.AddListener(OnLandmarkToggleOnHandler);
        RaycastHitManager.onSelectObjectEvent.AddListener((targetModel) =>
        {
            Data_AccessRecord data = dataHandler.GetDataByDevicePath(targetModel.name);
            if(data != null) infoPanel.ShowData(data);
        });
    }
    private void OnLandmarkToggleOnHandler(bool isOn, ILandmarkHandler result)
    {
        if (result is AccessDoor_LandMark landmark)
        {
            if (isOn) infoPanel.ShowData(landmark.data);
            else infoPanel.ToClose();
        }
    }

    protected override void OnShowHandler()
    {
        void onFailed(long responseCode, string msg) { }
        void onSuccess(List<Data_AccessRecord> result)
        {
            dataOfThisYear = result;
            ShowLandmarks();
        }
        //���o��ƶ�
        dataHandler.GetAccessRecordsOfThisYear(onSuccess, onFailed);
    }

    protected override void OnCloseHandler()
       => LandmarkManager_RE.RemoveLandmarks<AccessDoor_LandMark, Data_AccessRecord>();

    /// <summary>
    /// ��ܹϼ�
    /// </summary>
    private void ShowLandmarks()
      => LandmarkManager_RE.AddLandMarks(landmarkPrefab, dataHandler.datas, modelList);

    [ContextMenu("- [�ثe�~��] ���T�O��")] private void Test_GetYear() => dataHandler.GetAccessRecordsOfThisYear((result) => dataOfThisYear = result, null);
    [ContextMenu("- [�ثe���] ���T�O��")] private void Test_GetMonth() => dataHandler.GetAccessRecordsOfThisMonth((result) => dataOfThisYear = result, null);
    [ContextMenu("- [����] ���T�O��")] private void Test_GetToday() => dataHandler.GetAccessRecordsOfToday((result) => dataOfThisYear = result, null);
}
