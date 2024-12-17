using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// 機櫃內設定RU清單
/// </summary>
public class RackRUList : MonoBehaviour
{
    [Header(">>> 點擊項目時Invoke")]
    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    [Header(">>> UI組件")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform deviceRUItemContainer;
    [SerializeField] private DeviceRUItem deviceRUItemPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<DeviceRUItem> itemList { get; set; } = new List<DeviceRUItem>();

    /// <summary>
    /// 資產列表目前所選的項目
    /// </summary>
    private ListItem_Device_RE selectedListItem { get; set; }
    /// <summary>
    /// 資產列表目前所選的機櫃資料
    /// </summary>
    private Data_ServerRackAsset dataRack { get; set; }

    public void ShowRULayout(ListItem_Device_RE target)
    {
        selectedListItem = target;
        dataRack = (Data_ServerRackAsset)selectedListItem.data;

        //清空資料 
        itemList.ForEach(item =>
        {
            item.isOn = false;
            item.OnClickItemEvent.RemoveAllListeners();
        });
        itemList.Clear();
        ObjectPoolManager.PushToPool<DeviceRUItem>(deviceRUItemContainer);

        //建立資料
        dataRack.containers.ForEach(deviceAsset =>
        {
            DeviceRUItem item = ObjectPoolManager.GetInstanceFromQueuePool(deviceRUItemPrefab, deviceRUItemContainer);
            item.toggleGroup = toggleGroup;
            item.ShowData(deviceAsset);
            item.OnClickItemEvent.AddListener(OnClickItemEvent.Invoke);
            itemList.Add(item);
        });
        scrollRect.verticalNormalizedPosition = 1;
    }
}