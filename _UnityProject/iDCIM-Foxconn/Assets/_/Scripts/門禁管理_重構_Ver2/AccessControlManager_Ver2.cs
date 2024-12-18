using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;

/// <summary>
/// ���T�޲z��
/// </summary>
public class AccessControlManager_Ver2 : ModulePage
{
    [Header(">>> [Event] �I��Landmark��Invoke")]
    public UnityEvent<Transform> onClickLandmark = new UnityEvent<Transform>();
    [Header(">>> [Event] �����I��Landmark��Invoke")]
    public UnityEvent<Transform> onCancellLandmark = new UnityEvent<Transform>();

    [Header(">>> ����ܪ� Landmark�ϼ�")]
    [SerializeField] private List<Landmark_RE> landmarkList;

    protected override void OnShowHandler()
    {
        landmarkList.ForEach(landmark => landmark.gameObject.SetActive(true));

        //�w�]�I��
        landmarkList[0].isOn = true;
        onClickLandmark?.Invoke(landmarkList[0].targetModel);
    }
    protected override void OnCloseHandler()
    {
        landmarkList.ForEach(landmark =>
        {
            landmark.isOn = false;
            landmark.gameObject.SetActive(false);
        });
    }

    #region [Initialize]
    public override void OnInit(Action onInitComplete = null)
    {
        LandmarkManager_Ver3.AddLandmarks(landmarkList);
        Debug.Log(">>> AccessControlManager_Ver2 OnInit");
        onInitComplete?.Invoke();
    }
    protected override void InitEventListener()
    {
        RaycastHitManager.onSelectObjectEvent.AddListener(OnSelectObjectHandler);
        RaycastHitManager.onDeselectObjectEvent.AddListener(OnDeselectObjectHandler);
    }
    protected override void RemoveEventListener()
    {
        RaycastHitManager.onSelectObjectEvent.RemoveListener(OnSelectObjectHandler);
        RaycastHitManager.onDeselectObjectEvent.RemoveListener(OnDeselectObjectHandler);
    }
    private void OnSelectObjectHandler(Transform target)
    {
        if (target.name.Contains("CCTV") == false) onClickLandmark?.Invoke(target);
    }

    private void OnDeselectObjectHandler(Transform target)
    {
        if (target.name.Contains("CCTV") == false) onCancellLandmark?.Invoke(target);
    }
    #endregion
}
