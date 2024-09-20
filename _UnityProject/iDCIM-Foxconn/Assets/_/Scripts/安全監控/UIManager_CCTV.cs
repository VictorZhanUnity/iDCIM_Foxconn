using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_CCTV : MonoBehaviour
{
    [SerializeField] private List<SO_RTSP> rtspList = new List<SO_RTSP>();

    [SerializeField] private DeviceModelVisualizerWithLandmark deviceModelVisualizer;
    [SerializeField] private GameObject uiObj;
    [SerializeField] private Transform scrollViewContainer;

    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private ListItem_CCTV listitemPrefab;
    [SerializeField] private Panel_CCTV panelPrefab;

    private Panel_CCTV currentPanel { get; set; } = null;

    private Dictionary<SO_RTSP, Panel_CCTV> openedPanels { get; set; } = new Dictionary<SO_RTSP, Panel_CCTV> { };

    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;
            uiObj.SetActive(value);
        }
    }

    private void Awake()
    {
        uiObj.SetActive(false);
        deviceModelVisualizer.onInitlializedWithLandMark.AddListener((selectableObjects, landmarks) =>
        {
            for (int i = 0; i < selectableObjects.Count; i++)
            {
                ListItem_CCTV listItem = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_CCTV>(listitemPrefab, scrollViewContainer);
                listItem.SetupSelectableObjectAndLandmark(selectableObjects[i], landmarks[i]);
                listItem.toggleGroup = toggleGroup;
            }
        });
        deviceModelVisualizer.onSelectedEvent.AddListener(CreatePanel);
    }
    private void CreatePanel(SO_RTSP data)
    {
        if (currentPanel != null)
        {
            if (currentPanel.data == data)
            {
                currentPanel.ToShining();
                return;
            }
            currentPanel.Close();
            currentPanel = null;
        }
        if (openedPanels.TryGetValue(data, out Panel_CCTV panel))
        {
            panel.ToShining();
            return;
        }

        currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<Panel_CCTV>(panelPrefab, uiObj.transform);
        currentPanel.onClickScale.AddListener(ShowFullScreen);
        currentPanel.onDragged.AddListener(() =>
        {
            openedPanels.TryAdd(data, currentPanel);
            currentPanel = null;
        });
        currentPanel.Show(data);
        currentPanel.onClose.AddListener((data) =>
        {
            openedPanels.Remove(data);
            currentPanel = null;
        });
    }

    private void ShowFullScreen(Sprite sprite)
    {
        print($"ShowFullScreen: {sprite.name}");
    }

    private void OnValidate()
    {
        deviceModelVisualizer ??= GetComponent<DeviceModelVisualizerWithLandmark>();
        uiObj ??= transform.GetChild(0).gameObject;
    }
}
