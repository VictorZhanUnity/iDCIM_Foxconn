using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Advanced;
using VictorDev.Common;

/// <summary>
/// [門禁管理] UI與操作流程處理
/// </summary>
[RequireComponent(typeof(AccessRecord_DataHandler))]
public class AccessControlManager : iDCIM_ModuleManager
{
    [Header(">>> [資料項] - 今年度門禁資料")]
    [SerializeField] private List<Data_AccessRecord> dataOfThisYear;

    [Header(">>> [Prefab] - Landmark圖標")]
    [SerializeField] private AccessDoor_LandMark landmarkPrefab;

    [Header(">>> [組件] - 進場總人數")]
    [SerializeField] private Comp_AccessRecordTotalCount compTotalCount;
    [Header(">>> [組件] - 行事曆")]
    [SerializeField] private Comp_AccessRecordCalendar compCalendar;
    [Header(">>> [組件] - 指定某日的進場人數圖")]
    [SerializeField] private Chart_DayCount chartDayCount;
    [Header(">>> [組件] - 點選門禁視窗")]
    [SerializeField] private AccessDoor_InfoPanel infoPanel;

    /// <summary>
    ///  [資料處理] 門禁記錄資訊
    /// </summary>
    private AccessRecord_DataHandler _dataHandler { get; set; }
    private AccessRecord_DataHandler dataHandler => _dataHandler ??= GetComponent<AccessRecord_DataHandler>();

    /// <summary>
    /// 顯示圖標
    /// </summary>
    private void ShowLandmarks()
      => LandmarkManager_RE.AddLandMarks(landmarkPrefab, dataHandler.datas, modelList);

    /// <summary>
    /// 圖標被點選時
    /// </summary>
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
        InitListener();
        void onFailed(long responseCode, string msg) { }
        void onSuccess(List<Data_AccessRecord> result)
        {
            dataOfThisYear = result;
            ShowLandmarks();
        }
        //取得資料集
        dataHandler.GetAccessRecordsOfThisYear(onSuccess, onFailed);
    }

    protected override void OnCloseHandler()
    {
        RemoveListener();
    //   LandmarkManager_RE.RemoveLandmarks<AccessDoor_LandMark, Data_AccessRecord>();
    }

    private void InitListener()
    {
        dataHandler.onGetAccessRecordOfThisYear.AddListener(compTotalCount.ShowData);
        dataHandler.onGetAccessRecordOfThisYear.AddListener(compCalendar.SetDatas);
        infoPanel.onClickCloseButton.AddListener((data) => RaycastHitManager.CancellObjectSelected(data.DevicePath));

        LandmarkManager_RE.onToggleOnEvent.AddListener(OnLandmarkToggleOnHandler);
        RaycastHitManager.onSelectObjectEvent.AddListener(onSelectObjectEvent);
    }
    private void onSelectObjectEvent(Transform targetModel)
    {
        Data_AccessRecord data = dataHandler.GetDataByDevicePath(targetModel.name);
        if (data != null) infoPanel.ShowData(data);
    }


    private void RemoveListener()
    {
        dataHandler.onGetAccessRecordOfThisYear.RemoveAllListeners();
        dataHandler.onGetAccessRecordOfThisYear.RemoveAllListeners();
        compCalendar.onSelectedDateEvent.RemoveAllListeners();
        infoPanel.onClickCloseButton.RemoveAllListeners();
        LandmarkManager_RE.onToggleOnEvent.RemoveAllListeners();
        RaycastHitManager.onSelectObjectEvent.RemoveListener(onSelectObjectEvent);
    }


    [ContextMenu("- [目前年份] 門禁記錄")] private void Test_GetYear() => dataHandler.GetAccessRecordsOfThisYear((result) => dataOfThisYear = result, null);
    [ContextMenu("- [目前月份] 門禁記錄")] private void Test_GetMonth() => dataHandler.GetAccessRecordsOfThisMonth((result) => dataOfThisYear = result, null);
    [ContextMenu("- [今天] 門禁記錄")] private void Test_GetToday() => dataHandler.GetAccessRecordsOfToday((result) => dataOfThisYear = result, null);

    public override void OnInit(Action onInitComplete = null)
    {
        throw new NotImplementedException();
    }
}
