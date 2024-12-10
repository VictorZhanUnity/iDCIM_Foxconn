using System.Linq;
using UnityEngine;
using VictorDev.RevitUtils;

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
        //�I��IAQ�]�ƮɡA��ܨ��T���O
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
    /// ���IAQ��@���ƾ��v��T���O
    /// </summary>
    public void ShowIAQIndexHistoryPanel(IAQIndexDisplayer iaqIndexDisplayer)
        => iaqIndexHistoryPanel.ShowData(iaqIndexDisplayer);
}
