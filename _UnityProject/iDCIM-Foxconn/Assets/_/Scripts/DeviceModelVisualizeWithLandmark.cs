using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    public List<Landmark> landmarkList;

    public UnityEvent<List<SelectableObject>, List<Landmark>> onInitlializedWithLandMark = new UnityEvent<List<SelectableObject>, List<Landmark>>();
    public UnityEvent<SO_RTSP, ListItem_CCTV> onSelectedEvent = new UnityEvent<SO_RTSP, ListItem_CCTV>();

    public List<SelectableObject> selectableObjects { get; private set; } = new List<SelectableObject>();


    override protected void Start()
    {
        int counter = 0;
        //依照模型建立Landmark與SelectableObject架構
        modelList.ForEach(model =>
        {
            models.Add(model);

            SelectableObject selectableObj = model.AddComponent<SelectableObject>();
            selectableObj.CreateSoData(model.name, Config_RTSP[counter], sprites[counter++]);
            selectableObjects.Add(selectableObj);

            Landmark landMark = ObjectPoolManager.GetInstanceFromQueuePool<Landmark>(landMarkPrefab, LandmarkManager.container);

            landMark.name = $"LandMark_{model.name}";
            landMark.Initialize(model, offsetHeight, landmarkCategory);
            landMark.onToggleChanged.AddListener(selectableObj.SetIsOnWithoutNotify);
            landmarkList.Add(landMark);
            LandmarkManager.AddLandMark(landMark);

            selectableObj.onToggleEvent.AddListener((isOn) => landMark.SetToggleIsOnWithNotify(isOn));
            selectableObj.onSelectedEvent.AddListener(onSelectedEvent.Invoke);
        });
        onInitlializedWithLandMark.Invoke(selectableObjects, landmarkList);

        //如果只有一個地標的話，直接設定選取中
        selectableObjects[0].IsOn = (models.Count == 1);
    }

    override public bool isOn
    {
        set
        {
            base.isOn = value;
            landmarkList.ForEach(landmark => landmark.gameObject.SetActive(value));
        }
    }

    //ForTest
    private List<string> Config_RTSP = new List<string>() {
        "rtsp://admin:sks12345@ibms.sks.com.tw:554/2/1",
        "rtsp://admin:TCIT5i2020@192.168.0.181/profile1",
        "rtsp://admin:sks12345@ibms.sks.com.tw:554/1/1",
        "rtsp://admin:sks12345@ibms.sks.com.tw:554/3/1",
        "rtsp://admin:sks12345@ibms.sks.com.tw:554/4/1",
        "rtsp://admin:sks12345@ibms.sks.com.tw:554/5/1",
        "rtsp://admin:sks12345@ibms.sks.com.tw:554/6/1",
        "rtsp://admin:sks12345@ibms.sks.com.tw:554/7/1",
    };
    public List<Sprite> sprites = new List<Sprite>();
}
