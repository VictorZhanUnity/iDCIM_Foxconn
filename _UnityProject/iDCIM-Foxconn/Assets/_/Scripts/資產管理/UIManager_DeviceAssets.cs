using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.CameraUtils;

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

            if (value) WebAPI_GetAllRackAndDevice();
        }
    }
    private void Start()
    {
        deviceModelVisualizer.onClickEvent.AddListener(ShowRackInfoPanel);
        SetRackModelList();
    }

    /// <summary>
    /// [WebAPI] ���o�Ҧ����d�P��]��
    /// </summary>
    private void WebAPI_GetAllRackAndDevice()
    {
        void onSuccess(long responseCode, string jsonString)
        {
            print($"WebAPI_GetAllRackAndDevice: {jsonString}");
        }
        WebAPIManager.GetAllDCRContainer(onSuccess, onFailed);
    }

    private void onFailed(long responseCode, string msg)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// �]�w���d�ҫ�List�A��RackList�C��
    /// </summary>
    private void SetRackModelList()
    {
        //��name����r�z����d�ҫ�
        List<Transform> rackModelList = new List<Transform>();
        deviceModelVisualizer.ModelList.ForEach(model =>
        {
            if (model.name.Contains("RACK") || model.name.Contains("ATEN-PCE")) rackModelList.Add(model);
        });

        rackList.rackModels = rackModelList;
        rackList.onClickItemEvent.AddListener(ShowRackInfoPanel);
    }

    /// <summary>
    /// ��ܾ��d��T����
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