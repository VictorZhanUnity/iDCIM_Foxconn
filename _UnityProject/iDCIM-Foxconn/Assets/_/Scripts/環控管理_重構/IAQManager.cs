using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;

/// <summary>
/// IAQ管理器
/// </summary>
public class IAQManager : ModulePage
{
    [Header(">>> 欲顯示的 IAQ Landmark圖標")]
    [SerializeField] private List<Landmark_RE> landmarkList;

    [Header(">>> 環控資料管理器")]
    [SerializeField] private BlackboxDataManager blackboxDataManager;

    public override void OnInit(Action onInitComplete = null)
    {
        LandmarkManager_Ver3.AddLandmarks(landmarkList);

        //設定感測器Tag
        List<string> tagNames = modelList.Select(model => model.name.Split(",")[0]).ToList();
        tagNames = tagNames.SelectMany(tag => tag.Contains("04") ? new[] { $"{tag}/Smoke/Status" }
        : new[] { $"{tag}/RT/Value", $"{tag}/RT/Status", $"{tag}/RH/Value", $"{tag}/RH/Status" }).ToList();
        //加入環控項目Tag
        blackboxDataManager.AddTags(tagNames);

        Debug.Log(">>> IAQManager OnInit");
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
