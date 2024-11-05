using System.Collections.Generic;
using UnityEngine;

public class UIManager_AccessDoor : MonoBehaviour
{
    [SerializeField] private DeviceModelVisualizerWithLandmark deviceModelVisualizerWithLandmark;
    [SerializeField] private GameObject uiObj;
    [SerializeField] private MainDoorRecordPanel mainDoorRecordPanel;
    [SerializeField] private AccoutDetailPanel accoutDetailPanelPrefab;

    private AccoutDetailPanel currentPanel;

    public bool isOn
    {
        set
        {
            deviceModelVisualizerWithLandmark.isOn = value;
            if (value) GetMainDoorRecordData();

            uiObj.SetActive(value);
        }
    }

    private void Awake()
    {
        uiObj.SetActive(false);
    }

    /// <summary>
    /// [WebAPI]取得機房入口記錄
    /// </summary>
    private void GetMainDoorRecordData()
    {
        List<Dictionary<string, string>> data = DemoDataCenter.accessRecords;
        mainDoorRecordPanel.ShowData(data);

        mainDoorRecordPanel.onClickItemEvent.AddListener((listItem) =>
        {
            if (listItem.isOn)
            {
                if (currentPanel != null)
                {
                    if (listItem.recordData == currentPanel.userData) return;
                    currentPanel.Close();
                }
                currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<AccoutDetailPanel>(accoutDetailPanelPrefab, transform.GetChild(0).GetChild(1), true);
                currentPanel.ShowData(listItem.recordData);
                currentPanel.onClickCloseBtn.AddListener((data) =>
                {
                    mainDoorRecordPanel.ToggleOff(data);
                    currentPanel = null;
                });
            }
        });
    }

    public void Show()
    {
        uiObj.SetActive(true);
    }

    public void Hide()
    {
        uiObj.SetActive(false);
    }
}
