using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using VictorDev.RTSP;

public class CCTV_Manager_Ver3 : ModulePage
{
    [Header(">>> ����ܪ� Landmark�ϼ�")]
    [SerializeField] private List<Landmark_RE> landmarkList;

    #region [>>> Components]
    [Header(">>> [Prefab] - CCTV��T����")]
    [SerializeField] private CCTV_InfoPanel infoPanelPrefab;
    [SerializeField] private RectTransform containerForCCTVPanel;

    [Header(">>> CCTV���̼������")]
    [SerializeField] private CCTV_FullScreenPlayer fullScreenPlayer;

    /// <summary>
    /// �ثe��CCTV��T����
    /// </summary>
    private CCTV_InfoPanel currentPanel { get; set; } = null;

    private List<CCTV_InfoPanel> openedPanels = new List<CCTV_InfoPanel>();
    public RectTransform linePrefab;
    #endregion

    private void OnSelectObjectHandler(Transform target)
    {
        Landmark_RE landmark = landmarkList.FirstOrDefault(landmark => landmark.targetModel == target);
        if (landmark != null)
        {
            CCTVLandmarkDisplay landmarkDisplay = landmark.GetComponent<CCTVLandmarkDisplay>();
            ShowData(landmarkDisplay.rtspChannel, landmark.targetModel);
        }
    }
    private void OnDeselectObjectHandler(Transform target)
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
    }

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

        //���F�b�䥦������i�H�Q�I��A�ҥH�bOnInit�ɺ�ť�ƥ�
        RaycastHitManager.onSelectObjectEvent.AddListener(OnSelectObjectHandler);
        RaycastHitManager.onDeselectObjectEvent.AddListener(OnDeselectObjectHandler);

        Debug.Log(">>> CCTV_Manager_Ver3 OnInit");
        onInitComplete?.Invoke();
    }
    protected override void OnShowHandler()
    {
        landmarkList.ForEach(landmark => landmark.gameObject.SetActive(true));
    }
    protected override void OnCloseHandler()
    {
        landmarkList.ForEach(landmark =>
        {
      //      landmark.isOn = false;
            landmark.gameObject.SetActive(false);
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
