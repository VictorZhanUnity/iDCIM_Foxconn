using System.Linq;
using UnityEngine;

public class UIManager_IAQ : MonoBehaviour
{
    [Header(">>> IAQ�������ƧY�ɭ��O")]
    [SerializeField] private IAQRealtimeIndexPanel iaqRealtimeIndexPanel;
    [Header(">>> ��@IAQ�]�Ƥ��U�����ƭ��O")]
    [SerializeField] private IAQDevicePanel iaqDevicePanel;
    [Header(">>> IAQ��@���ƾ��v��T���O")]
    [SerializeField] private IAQ_IndexHistoryPanel iaqIndexHistoryPanel;

    [Header(">>> UI�ե�")]
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
            ShowIAQIndexPanel(iaqRealtimeIndexPanel.eachIAQData.Values.First());
        });

        iaqDevicePanel.onClickIAQIndex.AddListener(ShowIAQIndexHistoryPanel);

        iaqRealtimeIndexPanel.onClickIAQIndex.AddListener(ShowIAQIndexHistoryPanel);
        iaqRealtimeIndexPanel.WebAPI_GetRealtimeIAQIndex();
    }

    /// <summary>
    ///��@IAQ�]�Ƥ��U�����ƭ��O
    /// </summary>
    public void ShowIAQIndexPanel(Data_IAQ iaqData)
     => iaqDevicePanel.ShowData(iaqData);

    /// <summary>
    /// ���IAQ��@���ƾ��v��T���O
    /// </summary>
    public void ShowIAQIndexHistoryPanel(IAQIndexDisplayer iaqIndexDisplayer)
        => iaqIndexHistoryPanel.ShowData(iaqIndexDisplayer);
}
