using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [框架] 門禁資料接收器
/// </summary>
public abstract class AccessRecordDataReceiver : MonoBehaviour, IAccessRecordDataReceiver
{
    public abstract void ReceiveData(DataAccessRecord datas);
}

public interface IAccessRecordDataReceiver
{

    /// <summary>
    /// 接收資料
    /// </summary>
    abstract void ReceiveData(DataAccessRecord datas);
}