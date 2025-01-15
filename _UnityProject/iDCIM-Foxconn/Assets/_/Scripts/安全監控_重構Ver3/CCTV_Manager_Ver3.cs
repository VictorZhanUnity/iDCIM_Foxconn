using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RTSP;
using Debug = VictorDev.Common.Debug;

public class CCTV_Manager_Ver3 : ModulePage, IRaycastHitReceiver
{
    [Header(">>> ����ܪ� Landmark�ϼ�")]
    [SerializeField] private List<Landmark_RE> landmarkList;

    #region [>>> Components]
    [Header(">>> [Prefab] - CCTV��T����")]
    [SerializeField] private CCTV_InfoPanel infoPanelPrefab;
    [SerializeField] private RectTransform containerForCCTVPanel;

    [Header(">>> CCTV���̼������")]
    [SerializeField] private CCTVFullScreenPlayer fullScreenPlayer;

    /// <summary>
    /// �ثe��CCTV��T����
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
    /// �T�{�I���ﹳ�O�_�����޲z���C���W���ﹳ
    /// </summary>
    private bool IsTargetInList(Transform target, out Landmark_RE landmark)
    {
        landmark = landmarkList.FirstOrDefault(landmark => landmark.targetModel == target);
        return landmark != null;
    }

    public void OnMouseOverObjectEvent(Transform target) { }
    public void OnMouseExitObjectEvent(Transform target) { }

    /// <summary>
    /// �Ы�CCTV�W�ߵ���
    /// </summary>
    private void ShowData(SoData_RTSP_Channel data, Transform targetModel)
    {
        //�ˬd�O�_�w�}��
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

        //�]�w�s���u
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
        //���F�b�䥦������i�H�Q�I��A�ҥH�bOnInit�ɺ�ť�ƥ�
    }
    protected override void RemoveEventListener()
    {
        //���F�b�䥦�Ҳխ����ɡA��i�H�I��A�ҥH������Listener
        /*RaycastHitManager.onSelectObjectEvent.RemoveListener(OnSelectObjectHandler);
        RaycastHitManager.onDeselectObjectEvent.RemoveListener(OnDeselectObjectHandler);*/
    }


    #endregion
}
