using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VictorDev.Common;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// HUD - 機房資產使用率
/// </summary>
public class UseageOfDeviceAssetResource : MonoBehaviour
{
    [Header(">>> [資料項] 機房全機櫃與設備資料")]
    [SerializeField] private List<Data_ServerRackAsset> data;

    [Header(">>> UI組件")]
    [SerializeField] private ProgressBarController pbWatt;
    [SerializeField] private ProgressBarController pbRuSpace;
    [SerializeField] private ProgressBarController pbWeight;

    public void ShowUsage(List<Data_ServerRackAsset> data)
    {
        this.data = data;

        //設置進度條
        pbWatt.MaxValue = (int)data.Sum(rackData => rackData.information.watt);
        pbRuSpace.MaxValue = (int)data.Sum(rackData => rackData.information.heightU);
        pbWeight.MaxValue = (int)data.Sum(rackData => rackData.information.weight);

        float usageWatt = data.Sum(rackData => rackData.usageOfWatt);
        float usageRuSpace = data.Sum(rackData => rackData.usageOfRU);
        float usageWeight = data.Sum(rackData => rackData.usageOfWeight);


        DotweenHandler.ToLerpValue(0, usageWatt, (value) => pbWatt.value = value, Random.Range(1f, 5));
        DotweenHandler.ToLerpValue(0, usageRuSpace, (value) => pbRuSpace.value = value, Random.Range(1f, 5));
        DotweenHandler.ToLerpValue(0, usageWeight, (value) => pbWeight.value = value, Random.Range(1f, 5));
    }
}
