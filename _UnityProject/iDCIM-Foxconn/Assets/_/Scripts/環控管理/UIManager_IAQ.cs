using System.Linq;
using UnityEngine;
using VictorDev.RevitUtils;

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
        //點選IAQ設備時，顯示其資訊面板
        deviceModelVisualizer.onSelectedEvent.AddListener((soData, listItem, modelName)=>
        {
            iaqDevicePanel.ModelID = RevitHandler.GetDevicePath(modelName);
        });

        iaqDevicePanel.onClickIAQIndex.AddListener(ShowIAQIndexHistoryPanel);
        iaqRealtimeIndexPanel.onClickIAQIndex.AddListener(ShowIAQIndexHistoryPanel);
        iaqRealtimeIndexPanel.onUpdateIAQInfo.AddListener(iaqDevicePanel.ShowData);
        iaqRealtimeIndexPanel.WebAPI_GetRealtimeIAQIndex();
    }

    /// <summary>
    /// 顯示IAQ單一指數歷史資訊面板
    /// </summary>
    public void ShowIAQIndexHistoryPanel(IAQIndexDisplayer iaqIndexDisplayer)
        => iaqIndexHistoryPanel.ShowData(iaqIndexDisplayer);
}
