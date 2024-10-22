using UnityEngine;

public class UIManager_IAQ : MonoBehaviour
{
    [Header(">>> IAQ平均指數即時面板")]
    [SerializeField] private IAQRealtimeIndexPanel iaqRealtimeIndexPanel;
    [Header(">>> 單一IAQ設備之各項指數面板")]
    [SerializeField] private IAQDevicePanel iaqDevicePanel;
    [Header(">>> IAQ單一指數歷史資訊面板")]
    [SerializeField] private IAQ_IndexHistoryPanel iaqIndexHistoryPanel;

    [Header(">>> UI組件")]
    [SerializeField] private DeviceModelVisualizerWithLandmark _deviceModelVisualizer;
    [SerializeField] private GameObject uiObj;
    public DeviceModelVisualizerWithLandmark deviceModelVisualizer => _deviceModelVisualizer;

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
        iaqRealtimeIndexPanel.onClickIAQIndex.AddListener(ShowIAQIndexHistoryPanel);
        iaqRealtimeIndexPanel.WebAPI_GetRealtimeIAQIndex();
    }

    /// <summary>
    ///單一IAQ設備之各項指數面板
    /// </summary>
    public void ShowIAQIndexPanel(Transform targetModel)
    {/*
        if (currentIAQIndexPanel != null)
        {
            *//*   if (currentIAQIndexPanel.data == iaqIndexDisplayer) return;
               currentIAQIndexPanel.Close();
               currentIAQIndexPanel = null;*//*
        }
        //建立Panel
        IAQIndexPanel newPanel = ObjectPoolManager.GetInstanceFromQueuePool<IAQIndexPanel>(iaqIndexPanel, panelContaitner);
        *//*   newPanel.ShowData(iaqIndexDisplayer);
           newPanel.onClose.AddListener(() => currentIAQIndexPanel = null);*//*
        currentIAQIndexPanel = newPanel;*/
    }

    /// <summary>
    /// 顯示IAQ單一指數歷史資訊面板
    /// </summary>
    public void ShowIAQIndexHistoryPanel(IAQIndexDisplayer iaqIndexDisplayer)
        => iaqIndexHistoryPanel.ShowData(iaqIndexDisplayer);
}
