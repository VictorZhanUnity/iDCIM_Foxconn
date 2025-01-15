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
    [SerializeField] private CCTVFullScreenPlayer fullScreenPlayer;
    [SerializeField] private CCTV_9Grid cctv9Grid;
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

        if (panelEntrance != null)
        {
            //openedPanels.Add(panelEntrance.data.url, panelEntrance);
        }

        //�]�m�ʵ����C�����
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
        deviceModelVisualizer.onSelectedEvent.AddListener((soData, listItem, modelName)=>CreatePanel(soData, listItem));

        //�����Ҧ��v�����(���F���ФJ�f����)
        btnCloseAllWindows.onClick.AddListener(() =>
        {
            CloseAllPanel();
            cctv9Grid.Close();
            CheckAmountOfOpenedWindow();
        });

        //�p�a���I���p��
        minimap.onClickPin.AddListener((index) =>
        {
            ListItem_CCTV listItem = scrollViewContainer.GetChild(index).GetComponent<ListItem_CCTV>();
            listItem.isOn = true;
        });

       // cctv9Grid.onClickScaleBtn.AddListener(fullScreenPlayer.Show);
    }

    /// <summary>
    /// �����Ҧ��v�����(���F���ФJ�f����)
    /// </summary>
    private void CloseAllPanel()
    {
        //�����ؼе���
        if (currentPanel != null) currentPanel.Close();
        currentPanel = null;

        List<string> keys = openedPanels.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            openedPanels[keys[i]].Close();
            openedPanels.Remove(keys[i]);
        }
    }

    /// <summary>
    /// �إ߸�T���O
    /// </summary>
    private void CreatePanel(SO_RTSP data, ListItem_CCTV listItem)
    {
        if (currentPanel != null)
        {
          /*  if (currentPanel._data == data)
            {
                currentPanel.ToShining();
                return;
            }*/
            currentPanel.Close();
            currentPanel = null;
        }
        //�p�G�w�����}�L�������A�h�i�洣��
        if (openedPanels.TryGetValue(data.url, out Panel_CCTV panel))
        {
            panel.ToShining();
            return;
        }

        //�Y���E�c��Ҧ��A�h�ΤE�c�漽��
        if (cctv9Grid.isON)
        {
            cctv9Grid.Play(data, listItem);
            CheckAmountOfOpenedWindow();
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

        //�������Q�즲�ɡA�Y�]���v�窱�A
        currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<Panel_CCTV>(panelPrefab, transform);
        currentPanel.listItem = listItem;
        //currentPanel.onClickScale.AddListener(fullScreenPlayer.Show);
        currentPanel.onDragged.AddListener(() =>
        {
            openedPanels.TryAdd(data.url, currentPanel);
            currentPanel = null;

            CheckAmountOfOpenedWindow();
        });
        //currentPanel.ShowData(data);
        currentPanel.onClose.AddListener((data) =>
        {
            openedPanels.Remove(data.url);
            currentPanel = null;

            CheckAmountOfOpenedWindow();
        });
        CheckAmountOfOpenedWindow();
    }

    /// <summary>
    /// �ˬd�}�Ҫ������ƶq�P�ثe�O�_�w�}�ҵ���
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
    /// �]�m�E�c�漽��
    /// </summary>
    public void Set9GridPlayer()
    {
        List<Panel_CCTV> list = openedPanels.Values.ToList();
        if (currentPanel != null)
        {
            /*SO_RTSP data = currentPanel._data;
            ListItem_CCTV listItem = currentPanel.listItem;
            CloseAllPanel();
            cctv9Grid.Play(data, listItem);*/
        }
        else
        {
            CloseAllPanel();
        }

        list.ForEach((panel) =>
        {
            //cctv9Grid.Play(panel._data, panel.listItem);
        });

        CheckAmountOfOpenedWindow();
    }
    private void OnValidate()
    {
        deviceModelVisualizer ??= GetComponent<DeviceModelVisualizerWithLandmark>();
        uiObj ??= transform.GetChild(0).gameObject;

    }
}
