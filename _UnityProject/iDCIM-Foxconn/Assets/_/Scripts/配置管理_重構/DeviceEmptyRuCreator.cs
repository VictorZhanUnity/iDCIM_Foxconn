using System.Collections.Generic;

/// <summary>
/// �ʺA�ͦ����dRU�Ů�
/// </summary>
public class DeviceEmptyRuCreator : DeviceAssetDataReceiver
{
    private DeviceConfigureDataManager _deviceConfigureDataManager { get; set; }
    private DeviceConfigureDataManager deviceConfigureDataManager => _deviceConfigureDataManager ??= GetComponent<DeviceConfigureDataManager>();

    public override void ReceiveData(List<Data_ServerRackAsset> datas)
    {
    }
}
