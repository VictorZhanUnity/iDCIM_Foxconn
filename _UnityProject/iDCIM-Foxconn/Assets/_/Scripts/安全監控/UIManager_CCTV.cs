using System.Collections.Generic;
using System.Linq;
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

    [Space(10)]
    [SerializeField] private Panel_CCTV panelEntrance;
    [SerializeField] private Minimap_CCTV minimap;
    [SerializeField] private Button btnCloseAllWindows;

    private Panel_CCTV currentPanel { get; set; } = null;

    /// <summary>
    /// {RTSP URL, ��T���O}
    /// </summary>
    private Dictionary<string, Panel_CCTV> openedPanels { get; set; } = new Dictionary<string, Panel_CCTV> { };

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

                // �p�a��
                minimap.SetLandMarkWithListItem(listItem, i);
            }
        });
        deviceModelVisualizer.onSelectedEvent.AddListener(CreatePanel);

        //�����Ҧ��v�����(���F���ФJ�f����)
        btnCloseAllWindows.onClick.AddListener(() =>
        {
            List<string> keys = openedPanels.Keys.ToList();
            for (int i = 1; i < keys.Count; i++)
            {
                openedPanels[keys[i]].Close();
                openedPanels.Remove(keys[i]);
            }
        });

        /*minimap.onClickPin.AddListener((index) =>
        {
            ListItem_CCTV listItem = scrollViewContainer.GetChild(index).GetComponent<ListItem_CCTV>();
            listItem.isOn = true;
        });*/
    }
    /// <summary>
    /// �إ߸�T���O
    /// </summary>
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

        //�p�G�w�����}�L�������A�h�i�洣��
        if (openedPanels.TryGetValue(data.url, out Panel_CCTV panel))
        {
            panel.ToShining();
            return;
        }

        //�Ҧ��D�v�窱�A�������A�h�i������
        openedPanels.ToList().ForEach(panel =>
        {
            if (panel.Value.isPinOn == false)
            {
                panel.Value.Close();
                openedPanels.Remove(panel.Key);
            }
        });

        currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<Panel_CCTV>(panelPrefab, uiObj.transform);
        currentPanel.onClickScale.AddListener(ShowFullScreen);
        currentPanel.onDragged.AddListener(() =>
        {
            openedPanels.TryAdd(data.url, currentPanel);
            currentPanel = null;

            CheckAmountOfOpenedWindow();
        });
        currentPanel.ShowData(data);
        currentPanel.onClose.AddListener((data) =>
        {
            openedPanels.Remove(data.url);
            currentPanel = null;

            CheckAmountOfOpenedWindow();
        });
    }

    private void CheckAmountOfOpenedWindow()
    {
        btnCloseAllWindows.gameObject.SetActive(
            openedPanels.Count > 1
            );
    }

    private void ShowFullScreen(Sprite sprite)
    {
        print($"ShowFullScreen: {sprite.name}");
    }

    private void OnValidate()
    {
        deviceModelVisualizer ??= GetComponent<DeviceModelVisualizerWithLandmark>();
        uiObj ??= transform.GetChild(0).gameObject;
        if (panelEntrance != null)
        {
            openedPanels.Add(panelEntrance.data.url, panelEntrance);
        }
    }
}
