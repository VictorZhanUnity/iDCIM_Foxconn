using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.CameraUtils;
using static UnityEngine.GraphicsBuffer;

public class UIManager_DeviceAssets : MonoBehaviour
{
    [SerializeField] private GameObject uiObj;
    [SerializeField] private DeviceModelVisualizer deviceModelVisualizer;
    [SerializeField] private RackList rackList;
    [SerializeField] private RackInfoPanel rackInfoPanel;
    private RackInfoPanel currentPanel { get; set; }
    public bool isOn
    {
        set
        {
            deviceModelVisualizer.isOn = value;

            uiObj.SetActive(value);
        }
    }
    private void Start()
    {
        deviceModelVisualizer.onClickEvent.AddListener(ShowRackInfoPanel);
        SetRackModelList();
    }

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

        Demo_Rack data = DemoDataCenter.DemoRackList.FirstOrDefault(item => item.rack == listItem.rackModel);
        if (data != null)
        {
            OrbitCamera.MoveTargetTo(data.rack);
            rackInfoPanel.Show(data);
            GameManager.ToSelectTarget(data.rack);
        }

    }


    private void ShowRackInfoPanel(Transform target)
    {
        print($"ShowRackInfoPanel: {target.name}");
        Demo_Rack data = DemoDataCenter.DemoRackList.FirstOrDefault(item => item.rack == target);
        if (data != null)
        {
            OrbitCamera.MoveTargetTo(data.rack);
            rackInfoPanel.Show(data);
        }
    }
}