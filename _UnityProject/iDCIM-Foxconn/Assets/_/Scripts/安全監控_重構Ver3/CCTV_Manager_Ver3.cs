using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RTSP;
using Debug = VictorDev.Common.Debug;

public class CCTV_Manager_Ver3 : ModulePage, IRaycastHitReceiver
{
    [Header(">>> 欲顯示的 Landmark圖標")]
    [SerializeField] private List<Landmark_RE> landmarkList;

    #region [>>> Components]
    [Header(">>> [Prefab] - CCTV資訊視窗")]
    [SerializeField] private CCTV_InfoPanel infoPanelPrefab;
    [SerializeField] private RectTransform containerForCCTVPanel;

    [Header(">>> CCTV全屏播放視窗")]
    [SerializeField] private CCTV_FullScreenPlayer fullScreenPlayer;

    /// <summary>
    /// 目前的CCTV資訊視窗
    /// </summary>
    private CCTV_InfoPanel currentPanel { get; set; } = null;

    private List<CCTV_InfoPanel> openedPanels = new List<CCTV_InfoPanel>();
    public RectTransform linePrefab;
    #endregion

    public void OnSelectObjectHandler(Transform target)
    {
        if (IsTargetInList(target, out Landmark_RE landmark))
        {
            CCTVLandmarkDisplay landmarkDisplay = landmark.GetComponent<CCTVLandmarkDisplay>();
            ShowData(landmarkDisplay.rtspChannel, landmark.targetModel);

            target.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void OnDeselectObjectHandler(Transform target)
    {
        if (IsTargetInList(target, out Landmark_RE landmark))
        {
            if (currentPanel != null && target == currentPanel.targetModel)
            {
                currentPanel.ToClose();
                currentPanel = null;
            }
            else
            {
                openedPanels.FirstOrDefault(panel => panel.targetModel == target)?.ToClose();
            }
            target.GetChild(0).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 確認點擊對像是否為此管理器列表上的對像
    /// </summary>
    private bool IsTargetInList(Transform target, out Landmark_RE landmark)
    {
        landmark = landmarkList.FirstOrDefault(landmark => landmark.targetModel == target);
        return landmark != null;
    }

    public void OnMouseOverObjectEvent(Transform target) { }
    public void OnMouseExitObjectEvent(Transform target) { }

    /// <summary>
    /// 創建CCTV獨立視窗
    /// </summary>
    private void ShowData(SoData_RTSP_Channel data, Transform targetModel)
    {
        //檢查是否已開啟
        if (currentPanel != null)
        {
            if (currentPanel.data == data)
            {
                currentPanel.ToBlink();
                return;
            }
            landmarkList.FirstOrDefault(landmark => landmark.targetModel == currentPanel.targetModel).SetToggleStatus(false);
            currentPanel.onClickZoomButtn.RemoveAllListeners();
            currentPanel.onCloseEvent.RemoveAllListeners();
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
        infoPanel.onClickZoomButtn.AddListener(fullScreenPlayer.Show);
        infoPanel.onClickCloseButton.AddListener((data) =>
        {
            infoPanel.onClickZoomButtn.RemoveAllListeners();
            infoPanel.onClickCloseButton.RemoveAllListeners();
            openedPanels.Remove(infoPanel);
            OnDeselectObjectHandler(infoPanel.targetModel);
        });

        infoPanel.onDraggedEvent.AddListener(() =>
        {
            if (openedPanels.Contains(infoPanel) == false) openedPanels.Add(infoPanel);
            currentPanel = null;
        });

        currentPanel = infoPanel;
        currentPanel.onCloseEvent.AddListener((panel) => landmarkList.FirstOrDefault(landmark => landmark.targetModel == panel.targetModel).SetToggleStatus(false));
    }

    #region [>>> Initialize] 
    public override void OnInit(Action onInitComplete = null)
    {
        LandmarkManager_Ver3.AddLandmarks(landmarkList, false);
        Debug.Log(">>> CCTV_Manager_Ver3 OnInit");
        onInitComplete?.Invoke();
    }
    protected override void OnShowHandler()
    {
        landmarkList.ForEach(landmark => landmark.gameObject.SetActive(true));
        modelList.ForEach(model => model.GetComponent<Collider>().enabled = true);
    }
    protected override void OnCloseHandler()
    {
        landmarkList.ForEach(landmark =>
        {
            landmark.gameObject.SetActive(false);
        });
        modelList.ForEach(model =>
        {
            model.GetComponent<Collider>().enabled = false;
        });
    }

    protected override void InitEventListener()
    {
        //為了在其它頁面亦可以被點選，所以在OnInit時監聽事件
    }
    protected override void RemoveEventListener()
    {
        //為了在其它模組頁面時，亦可以點選，所以不移除Listener
        /*RaycastHitManager.onSelectObjectEvent.RemoveListener(OnSelectObjectHandler);
        RaycastHitManager.onDeselectObjectEvent.RemoveListener(OnDeselectObjectHandler);*/
    }


    #endregion
}
