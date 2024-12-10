using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Advanced;
using VictorDev.Common;

public class CCTVManager : iDCIM_ModuleManager
{
    [Header(">>> [資料處理] WebAPI CCTV連線資訊")]
    [SerializeField] private CCTV_DataHandler webApiHandler;

    [Header(">>> Landmark圖標Prefab")]
    [SerializeField] private CCTV_LandMark landmarkPrefab;

    [Space(10)]
    [Header(">>> [Prefab] - CCTV資訊視窗")]
    [SerializeField] private CCTV_InfoPanel infoPanelPrefab;
    [SerializeField] private RectTransform containerForCCTVPanel;

    public RectTransform linePrefab;

    [Header(">>> CCTV全屏播放視窗")]
    [SerializeField] private CCTV_FullScreenPlayer fullScreenPlayer;

    /// <summary>
    /// 目前的CCTV資訊視窗
    /// </summary>
    private CCTV_InfoPanel currentPanel { get; set; } = null;

    private List<CCTV_InfoPanel> openedPanels = new List<CCTV_InfoPanel>();

    private void Start()
    {
        LandmarkManager_RE.onToggleOnEvent.AddListener(OnLandmarkToggleOnHandler);
        RaycastHitManager.onDeselectObjectEvent.AddListener(OnCancellSelectHandler);
        RaycastHitManager.onSelectObjectEvent.AddListener((targetModel) =>
        {
            Data_RTSP data = webApiHandler.GetDataByDevicePath(targetModel.name);
            if (data != null)
            {
                ShowData(data, targetModel);
                targetModel.GetChild(0).gameObject.SetActive(true);
            }
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
        else
        {
            CCTV_InfoPanel target = openedPanels.FirstOrDefault(panel => targetModel.name.Contains(panel.data.DevicePath));
            if (target != null)
            {
                target.ToClose();
                target.onClickZoomButtn.RemoveAllListeners();
                targetModel.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void OnLandmarkToggleOnHandler(bool isOn, ILandmarkHandler result)
    {
        if (result is CCTV_LandMark landmark)
        {
            if (isOn) ShowData(landmark.data, landmark.targetModel);
            else OnCancellSelectHandler(result.targetModel);
            landmark.targetModel.GetChild(0).gameObject.SetActive(isOn);
        }
    }

    /// <summary>
    /// 創建CCTV獨立視窗
    /// </summary>
    private void ShowData(Data_RTSP data, Transform targetModel)
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
        CCTV_InfoPanel infoPanel = Instantiate(infoPanelPrefab, containerForCCTVPanel);

        //設定連接線
        infoPanel.targetModel = targetModel;
        infoPanel.lineSegment1 = Instantiate(linePrefab, containerForCCTVPanel);
        infoPanel.lineSegment2 = Instantiate(linePrefab, containerForCCTVPanel);
        infoPanel.lineSegment1.SetSiblingIndex(infoPanel.transform.GetSiblingIndex() - 1);
        infoPanel.lineSegment2.SetSiblingIndex(infoPanel.transform.GetSiblingIndex() - 1);

        infoPanel.containerForDrag = containerForCCTVPanel;
        infoPanel.ShowData(data);
        infoPanel.onClickZoomButtn.AddListener(OnZoomHandler);
        infoPanel.onClickCloseButton.AddListener((data) =>
        {
            infoPanel.onClickZoomButtn.RemoveAllListeners();
            infoPanel.onClickCloseButton.RemoveAllListeners();
            openedPanels.Remove(infoPanel);
            //取消模型的選取狀態
            Transform targetModel = modelList.FirstOrDefault(model => model.name.Contains(data.devicePath));
            if (targetModel != null) RaycastHitManager.CancellObjectSelected(targetModel);
        });

        infoPanel.onDraggedEvent.AddListener(() =>
        {
            if (openedPanels.Contains(infoPanel) == false) openedPanels.Add(infoPanel);
            currentPanel = null;
        });

        currentPanel = infoPanel;
    }

    private void OnZoomHandler(CCTV_InfoPanel data)
    {
        fullScreenPlayer.Show(data);
    }

    protected override void OnShowHandler() => GetAllCCTVInfo();

    protected override void OnCloseHandler()
    {
        LandmarkManager_RE.RemoveLandmarks<CCTV_LandMark, Data_RTSP>();
    }

    [ContextMenu("- [WebAPI] 取得所有CCTV設備資訊")]
    private void GetAllCCTVInfo()
    {
        void onFailed(long responseCode, string msg)
        {
        }
        webApiHandler.LoadDatas(() => { if (isOn) ShowLandmarks(); }, onFailed);
    }
    /// <summary>
    /// 顯示圖標
    /// </summary>
    private void ShowLandmarks()
        => LandmarkManager_RE.AddLandMarks(landmarkPrefab, webApiHandler.datas, modelList);

    public override void OnInit(Action onInitComplete = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// [WebAPI資料項] CCTV連線資訊
    /// </summary>
    [Serializable]
    public class CCTV_DataHandler
    {
        [SerializeField] private List<Data_RTSP> _datas;
        public List<Data_RTSP> datas => _datas;
        public void LoadDatas(Action onSuccess, Action<long, string> onFailed)
        {
            void onSuccessHandler(long responseCode, string jsonString)
            {
                _datas = JsonConvert.DeserializeObject<List<Data_RTSP>>(jsonString);
                onSuccess.Invoke();
            }
            WebAPI_SearchDeviceAsset.SearchDeviceAsset("+CCTV", onSuccessHandler, onFailed);
        }
        public Data_RTSP GetDataByDevicePath(string modelName) => _datas.FirstOrDefault(data => modelName.Contains(data.DevicePath));
    }
}
