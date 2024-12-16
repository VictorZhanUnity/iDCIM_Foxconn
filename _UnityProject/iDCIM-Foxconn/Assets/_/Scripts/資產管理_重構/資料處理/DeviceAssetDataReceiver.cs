using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [框架] 資產資料接收器
/// </summary>
public abstract class DeviceAssetDataReceiver : MonoBehaviour, IDeviceAssetDataReceiver
{
    public abstract void ReceiveData(List<Data_ServerRackAsset> datas);
}

public interface IDeviceAssetDataReceiver
{

    /// <summary>
    /// 接收資料
    /// </summary>
    abstract void ReceiveData(List<Data_ServerRackAsset> datas);
}