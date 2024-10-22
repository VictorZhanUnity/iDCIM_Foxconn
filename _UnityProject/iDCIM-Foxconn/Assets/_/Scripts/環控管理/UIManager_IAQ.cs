using UnityEngine;

public class UIManager_IAQ : MonoBehaviour
{
    [Header(">>> IAQ平均指數即時面板")]
    [SerializeField] private IAQRealtimeIndexPanel iaqRealtimeIndexPanel;
    [Header(">>> 單一IAQ設備之各項指數面板")]
    [SerializeField] private IAQIndexPanel iaqIndexPanelPrefab;
    [Header(">>> IAQ單一指數歷史資訊面板")]
    [SerializeField] private IAQ_IndexHistoryPanel iaqIndexDetailPanelPrefab;
    [SerializeField] private Transform panelContaitner;

    [Header(">>> UI組件")]
    [SerializeField] private DeviceModelVisualizerWithLandmark _deviceModelVisualizer;
    [SerializeField] private GameObject uiObj;
    public DeviceModelVisualizerWithLandmark deviceModelVisualizer => _deviceModelVisualizer;

    private IAQIndexPanel currentIAQIndexPanel;
    private IAQ_IndexHistoryPanel currentIndexHistoryPanel;

    public bool isOn
    {
        set
        {
            _deviceModelVisualizer.isOn = value;
            uiObj.SetActive(value);

            if (value) iaqRealtimeIndexPanel.WebAPI_GetRealtimeIAQIndex();
        }
    }

    private void Start()
    {
        deviceModelVisualizer.onSelectedEvent.AddListener((data, data1)=>
        {
          
        });
        iaqRealtimeIndexPanel.WebAPI_GetRealtimeIAQIndex();
    }

    /// <summary>
    ///單一IAQ設備之各項指數面板
    /// </summary>
    public void ShowIAQIndexPanel(Transform targetModel)
    {
        if (currentIAQIndexPanel != null)
        {
            /*   if (currentIAQIndexPanel.data == iaqIndexDisplayer) return;
               currentIAQIndexPanel.Close();
               currentIAQIndexPanel = null;*/
        }
        //建立Panel
        IAQIndexPanel newPanel = ObjectPoolManager.GetInstanceFromQueuePool<IAQIndexPanel>(iaqIndexPanelPrefab, panelContaitner);
        /*   newPanel.ShowData(iaqIndexDisplayer);
           newPanel.onClose.AddListener(() => currentIAQIndexPanel = null);*/
        currentIAQIndexPanel = newPanel;
    }

    /// <summary>
    /// 顯示IAQ單一指數歷史資訊面板
    /// </summary>
    public void ShowIAQIndexHistoryPanel(IAQIndexDisplayer iaqIndexDisplayer)
    {
        if (currentIndexHistoryPanel != null)
        {
            if (currentIndexHistoryPanel.data == iaqIndexDisplayer) return;
            currentIndexHistoryPanel.Close();
            currentIndexHistoryPanel = null;
        }
        //建立Panel
        IAQ_IndexHistoryPanel newPanel = ObjectPoolManager.GetInstanceFromQueuePool<IAQ_IndexHistoryPanel>(iaqIndexDetailPanelPrefab, panelContaitner);
        newPanel.ShowData(iaqIndexDisplayer);
        newPanel.onClose.AddListener(() => currentIndexHistoryPanel = null);
        currentIndexHistoryPanel = newPanel;
    }
}
