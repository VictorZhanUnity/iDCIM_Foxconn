using System.Linq;
using UnityEngine;

/// <summary>
/// 資產管理 - 資料載入
/// </summary>
public class DeviceAssetDataManager : DeviceConfigureDataManager
{
    [Header(">>> 資產管理器")]
    [SerializeField] private DeviceAssetManager deviceAssetManager;

    [ContextMenu("- 取得所有機櫃與設備")]
    private void GetAllDeviceAssets() => GetDeviceAssets();

    /// <summary>
    /// 設置相對應模型
    /// </summary>
    override protected void SetDeviceModel()
    {
        dataRack.ForEach(rack =>
        {
            rack.model = deviceAssetManager.modelList.FirstOrDefault(model => model.name.Contains(rack.deviceName));
        });
        dataRack.SelectMany(rack => rack.containers).ToList().ForEach(device =>
        {
            device.model = deviceAssetManager.modelList.FirstOrDefault(model => model.name.Contains(device.deviceName));
        });
    }
}
