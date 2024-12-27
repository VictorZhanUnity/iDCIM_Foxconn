using System.Collections.Generic;

/// <summary>
/// 動態生成機櫃RU空格
/// </summary>
public class DeviceEmptyRuCreator : DeviceAssetDataReceiver
{
    private DeviceConfigureDataManager _deviceConfigureDataManager { get; set; }
    private DeviceConfigureDataManager deviceConfigureDataManager => _deviceConfigureDataManager ??= GetComponent<DeviceConfigureDataManager>();

    public override void ReceiveData(List<Data_ServerRackAsset> datas)
    {
    }
}
