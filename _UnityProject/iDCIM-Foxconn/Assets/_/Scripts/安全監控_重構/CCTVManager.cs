using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Advanced;
using VictorDev.Common;

public class CCTVManager : iDCIM_ModuleManager
{
    [Header(">>> [WebAPI資料項] CCTV連線資訊")]
    [SerializeField] private List<Data_RTSP> webapiDatas;

    [Header(">>> Landmark圖標Prefab")]
    [SerializeField] private CCTV_LandMark landmarkPrefab;

    [Header(">>> CCTV資訊視窗")]
    [SerializeField] private CCTV_InfoPanel infoPanelPrefab;
    [SerializeField] private RectTransform containerForCCTVPanel;

    [Header(">>> CCTV全屏播放視窗")]
    [SerializeField] private CCTV_FullScreenPlayer fullScreenPlayer;

    /// <summary>
    /// 目前的CCTV資訊視窗
    /// </summary>
    private CCTV_InfoPanel currentPanel { get; set; } = null;

    private List<CCTV_InfoPanel> openedPanels { get; set; } = new List<CCTV_InfoPanel>();


    private void Start()
    {
        LandmarkManager_RE.onToggleOnEvent.AddListener(OnLandmarkToggleOnHandler);
        RaycastHitManager.onDeselectObjectEvent.AddListener(OnCancellSelectHandler);
        RaycastHitManager.onSelectObjectEvent.AddListener((targetModel) =>
        {
            Data_RTSP data = webapiDatas.FirstOrDefault(data => targetModel.name.Contains(data.DevicePath));
            ShowData(data);
        });
        GetAllCCTVInfo();
    }

    private void OnCancellSelectHandler(Transform targetModel)
    {
        //檢查是否已開啟
        if (currentPanel != null)
        {
            if (targetModel.name.Contains(currentPanel.data.DevicePath))
            {
                currentPanel.ToClose();
                currentPanel.onClickZoomButtn.RemoveAllListeners();
                return;
            }
        }
    }

    private void OnLandmarkToggleOnHandler(bool isOn, ILandmarkHandler result)
    {
        if (result is CCTV_LandMark landmark)
        {
            if (isOn) ShowData(landmark.data);
            else OnCancellSelectHandler(result.targetModel);
        }
    }

    /// <summary>
    /// 創建CCTV獨立視窗
    /// </summary>
    private void ShowData(Data_RTSP data)
    {

        //檢查是否已開啟
        if (currentPanel != null)
        {
            if (currentPanel.data == data)
            {
                currentPanel.ToBlink();
                return;
            }
            currentPanel.onClickZoomButtn.RemoveAllListeners();
            currentPanel.ToClose();
        }

        var existPanel = openedPanels.FirstOrDefault(panel => panel.data == data);
        if (existPanel != null)
        {
            existPanel.ToBlink();
            return;
        }

        //  var infoPanel = ObjectPoolManager.GetInstanceFromQueuePool(infoPanelPrefab, containerForCCTVPanel);
        var infoPanel = Instantiate(infoPanelPrefab, containerForCCTVPanel);
        infoPanel.containerForDrag = containerForCCTVPanel;
        infoPanel.ShowData(data);
        infoPanel.onClickZoomButtn.AddListener(OnZoomHandler);
        infoPanel.onClickCloseButton.AddListener((data) =>
        {
            openedPanels.Remove(infoPanel);
            //取消模型的選取狀態
            Transform targetModel = modelList.FirstOrDefault(model => model.name.Contains(data.devicePath));
            if (targetModel != null) RaycastHitManager.CancellObjectSelected(targetModel);
        });

        infoPanel.onDraggedEvent.AddListener(() =>
        {
            openedPanels.Add(infoPanel);
            currentPanel = null;
        });

        currentPanel = infoPanel;
    }

    private void OnZoomHandler(Data_RTSP data)
    {
        fullScreenPlayer.Show(data);
    }

    protected override void OnShowHandler()
    {
        GetAllCCTVInfo();
    }

    protected override void OnCloseHandler()
    {
        LandmarkManager_RE.RemoveLandmarks<CCTV_LandMark, Data_RTSP>();
    }

    [ContextMenu("- [WebAPI] 取得所有CCTV設備資訊")]
    private void GetAllCCTVInfo()
    {
        void onFailed(long responseCode, string msg)
        {
            throw new NotImplementedException();
        }

        void onSuccess(long responseCode, string jsonString_PageData)
        {
            webapiDatas = JsonConvert.DeserializeObject<List<Data_RTSP>>(jsonString_PageData);
            if (isOn) ShowLandmarks();
        }

        WebAPI_SearchDeviceAsset.SearchDeviceAsset("+CCTV", onSuccess, onFailed);
    }

    private void ShowLandmarks()
        => LandmarkManager_RE.AddLandMarks(landmarkPrefab, webapiDatas, modelList);
}
