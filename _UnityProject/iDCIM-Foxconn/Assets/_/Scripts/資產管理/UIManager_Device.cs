using System.Collections.Generic;
using UnityEngine;

public class UIManager_Device : MonoBehaviour
{
    [SerializeField] private GameObject uiObj;
    [SerializeField] private DeviceModelVisualizer deviceModelVisualizer;
    [SerializeField] private RackList rackList;
    [SerializeField] private RackInfoPanel rackInfoPanelPrefab;

    private RackInfoPanel currentPanel { get; set; }
    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;
            uiObj.SetActive(value);
        }
    }
    private void Start() => SetRackModelList();

    /// <summary>
    /// 設定機櫃模型List，給RackList列表
    /// </summary>
    private void SetRackModelList()
    {
        //依name關鍵字篩選機櫃模型
        List<Transform> rackModelList = new List<Transform>();
        deviceModelVisualizer.ModelList.ForEach(model =>
        {
            if (model.name.Contains("RACK") || model.name.Contains("ATEN-PCE")) rackModelList.Add(model);
        });

        rackList.rackModels = rackModelList;
        rackList.onClickItemEvent.AddListener(ShowRackInfoPanel);
    }

    /// <summary>
    /// 顯示機櫃資訊視窗
    /// </summary>
    /// <param name="listItem"></param>
    private void ShowRackInfoPanel(ListItem_Rack listItem)
    {
        print($"ShowRackInfoPanel: {listItem.name}");
        return;

        currentPanel = ObjectPoolManager.GetInstanceFromQueuePool<RackInfoPanel>(rackInfoPanelPrefab, transform.GetChild(0), true);
        currentPanel.SetListItem(listItem);
    }
}