using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [框架] IAQ資料接收器
/// </summary>
public abstract class BlackboxDataReceiver : MonoBehaviour, IIAQDataReceiver
{
    public abstract void ReceiveData(List<Data_Blackbox> datas);
}

public interface IIAQDataReceiver
{
    abstract void ReceiveData(List<Data_Blackbox> datas);
}