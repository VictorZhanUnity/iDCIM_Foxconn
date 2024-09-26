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
    [SerializeField] private CCTV_FullScreenPlayer fullScreenPlayer;
    [SerializeField] private CCTV_9Grid cctv9Grid;
    [SerializeField] private Minimap_CCTV minimap;
    [SerializeField] private Button btnCloseAllWindows;

    private Panel_CCTV currentPanel { get; set; } = null;

    /// <summary>
    /// {RTSP URL, 資訊面板}
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

        if (panelEntrance != null)
        {
            //openedPanels.Add(panelEntrance.data.url, panelEntrance);
        }

        //設置監視器列表資料
        deviceModelVisualizer.onInitlializedWithLandMark.AddListener((selectableObjects, landmarks) =>
        {
            for (int i = 0; i < selectableObjects.Count; i++)
            {
                ListItem_CCTV listItem = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_CCTV>(listitemPrefab, scrollViewContainer);
                listItem.SetupSelectableObjectAndLandmark(selectableObjects[i], landmarks[i]);
                listItem.toggleGroup = toggleGroup;

                listItem.minimapLandmark = minimap.landmarkList[i];
            }
        });
        deviceModelVisualizer.onSelectedEvent.AddListener(CreatePanel);

        //關閉所有釘選視窗(除了機房入口視窗)
        btnCloseAllWindows.onClick.AddListener(() =>
        {
            CloseAllPanel();
            cctv9Grid.Close();
            CheckAmountOfOpenedWindow();
        });

        //小地圖點選聯結
        minimap.onClickPin.AddListener((index) =>
        {
            ListItem_CCTV listItem = scrollViewContainer.GetChild(index).GetComponent<ListItem_CCTV>();
            listItem.isOn = true;
        });
    }

    /// <summary>
    /// 關閉所有釘選視窗(除了機房入口視窗)
    /// </summary>
    private void CloseAllPanel()
    {
        List<string> keys = openedPanels.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            openedPanels[keys[i]].Close();
            openedPanels.Remove(keys[i]);
        }
        //關閉目標視窗
        currentPanel?.Close();
        currentPanel = null;
    }

    /// <summary>
    /// 建立資訊面板
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

        //如果已有打開過的視窗，則進行提示
        if (openedPanels.TryGetValue(data.url, out Panel_CCTV panel))
        {
            panel.ToShining();
            return;
        }

        //若為九宮格模式，則用九宮格播放
        if (cctv9Grid.isON)
        {
            cctv9Grid.Play(data);
            CheckAmountOfOpenedWindow();
            return;
        }

        //所有非釘選狀態的視窗，則進行關閉
        openedPanels.ToList().ForEach(panel =>
        {
            if (panel.Value.isPinOn == false)
            {
                panel.Value.Close();
                openedPanels.Remove(panel.Key);
            }
        });

        //當視窗被拖曳時，即設為釘選狀態
        currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<Panel_CCTV>(panelPrefab, transform);
        currentPanel.onClickScale.AddListener(fullScreenPlayer.Show);
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
        CheckAmountOfOpenedWindow();
    }

    /// <summary>
    /// 檢查開啟的視窗數量與目前是否已開啟視窗
    /// </summary>
    private void CheckAmountOfOpenedWindow()
    {
        bool isShow;
        if (cctv9Grid.isON)
        {
            isShow = cctv9Grid.dictGridItems.Count > 0;
        }
        else
        {
            isShow = currentPanel != null || openedPanels.Count > 0;
        }
        btnCloseAllWindows.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 設置九宮格播放
    /// </summary>
    public void Set9GridPlayer()
    {
        if (currentPanel != null)
        {
            cctv9Grid.Play(currentPanel.data);
            currentPanel.Close();
            currentPanel = null;
        }
        openedPanels.Values.ToList().ForEach((panel) =>
        {
            cctv9Grid.Play(panel.data);
        });
        CloseAllPanel();
    }
    private void OnValidate()
    {
        deviceModelVisualizer ??= GetComponent<DeviceModelVisualizerWithLandmark>();
        uiObj ??= transform.GetChild(0).gameObject;

    }
}
