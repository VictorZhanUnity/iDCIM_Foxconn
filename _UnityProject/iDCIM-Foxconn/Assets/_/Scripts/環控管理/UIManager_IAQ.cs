using UnityEngine;

public class UIManager_IAQ : MonoBehaviour
{
    [Header(">>> IAQ�������ƧY�ɭ��O")]
    [SerializeField] private IAQRealtimeIndexPanel iaqRealtimeIndexPanel;
    [Header(">>> ��@IAQ�]�Ƥ��U�����ƭ��O")]
    [SerializeField] private IAQIndexPanel iaqIndexPanelPrefab;
    [Header(">>> IAQ��@���ƾ��v��T���O")]
    [SerializeField] private IAQ_IndexHistoryPanel iaqIndexDetailPanelPrefab;
    [SerializeField] private Transform panelContaitner;

    [Header(">>> UI�ե�")]
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
    ///��@IAQ�]�Ƥ��U�����ƭ��O
    /// </summary>
    public void ShowIAQIndexPanel(Transform targetModel)
    {
        if (currentIAQIndexPanel != null)
        {
            /*   if (currentIAQIndexPanel.data == iaqIndexDisplayer) return;
               currentIAQIndexPanel.Close();
               currentIAQIndexPanel = null;*/
        }
        //�إ�Panel
        IAQIndexPanel newPanel = ObjectPoolManager.GetInstanceFromQueuePool<IAQIndexPanel>(iaqIndexPanelPrefab, panelContaitner);
        /*   newPanel.ShowData(iaqIndexDisplayer);
           newPanel.onClose.AddListener(() => currentIAQIndexPanel = null);*/
        currentIAQIndexPanel = newPanel;
    }

    /// <summary>
    /// ���IAQ��@���ƾ��v��T���O
    /// </summary>
    public void ShowIAQIndexHistoryPanel(IAQIndexDisplayer iaqIndexDisplayer)
    {
        if (currentIndexHistoryPanel != null)
        {
            if (currentIndexHistoryPanel.data == iaqIndexDisplayer) return;
            currentIndexHistoryPanel.Close();
            currentIndexHistoryPanel = null;
        }
        //�إ�Panel
        IAQ_IndexHistoryPanel newPanel = ObjectPoolManager.GetInstanceFromQueuePool<IAQ_IndexHistoryPanel>(iaqIndexDetailPanelPrefab, panelContaitner);
        newPanel.ShowData(iaqIndexDisplayer);
        newPanel.onClose.AddListener(() => currentIndexHistoryPanel = null);
        currentIndexHistoryPanel = newPanel;
    }
}
