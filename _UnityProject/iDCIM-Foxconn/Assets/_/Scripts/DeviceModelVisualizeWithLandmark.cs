using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VictorDev.Advanced;

public class DeviceModelVisualizerWithLandmark : DeviceModelVisualizer
{
    [Space(20)]

    [Header(">>> 地標類型")]
    [SerializeField] private LandmarkCategory landmarkCategory;
    [Header(">>> 地標樣式")]
    [SerializeField] private Landmark landMarkPrefab;
    [Header(">>> 地標高度")]
    [SerializeField] private float offsetHeight;

    [Header(">>> 地標列表")]
    [SerializeField] private List<Landmark> landmarkList;

    override protected void Awake()
    {
        //依照模型建立Landmark與SelectableObject架構
        modelList.ForEach(model =>
        {
            model.tag = landmarkCategory.ToString();
            models.Add(model);

            SelectableObject selectableObj = model.AddComponent<SelectableObject>();
            Landmark landMark = ObjectPoolManager.GetInstanceFromQueuePool<Landmark>(landMarkPrefab, LandmarkManager.container);

            landMark.name = $"LandMark_{model.name}";
            landMark.Initialize(model, offsetHeight, landmarkCategory);
            landMark.onToggleChanged.AddListener(selectableObj.SetIsOnWithoutNotify);
            landmarkList.Add(landMark);
            LandmarkManager.AddLandMark(landMark);

            selectableObj.onSelectedEvent.AddListener(landMark.SetToggleIsOnWithNotify);
        });
    }

    override public bool isOn
    {
        set
        {
            base.isOn = value;
            landmarkList.ForEach(landmark => landmark.gameObject.SetActive(value));
        }
    }
}
