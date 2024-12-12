using System;
using System.Collections.Generic;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// 門禁管理器
/// </summary>
public class AccessControlManager_Ver2 : ModulePage
{
    [Header(">>> 欲顯示的 Landmark圖標")]
    [SerializeField] private List<Landmark_RE> landmarkList;

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
    private void OnSelectObjectHandler(Transform target)
    {
        Debug.Log($"OnSelectObjectHandler: {target.name}");
    }
    private void OnDeselectObjectHandler(Transform target)
    {
        Debug.Log($"OnDeselectObjectHandler: {target.name}");
    }

    protected override void RemoveEventListener()
    {
        RaycastHitManager.onSelectObjectEvent.RemoveAllListeners();
        RaycastHitManager.onDeselectObjectEvent.RemoveAllListeners();
    }

    protected override void OnCloseHandler()
    {
        landmarkList.ForEach(landmark =>
        {
            landmark.isOn = false;
            landmark.gameObject.SetActive(false);
        });
    }
    protected override void OnShowHandler()
    {
        landmarkList.ForEach(landmark => landmark.gameObject.SetActive(true));
    }
}
