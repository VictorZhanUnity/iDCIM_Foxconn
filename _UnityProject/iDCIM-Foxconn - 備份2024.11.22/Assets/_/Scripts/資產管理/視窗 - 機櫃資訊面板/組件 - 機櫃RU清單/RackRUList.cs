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
    [Header(">>> [資料項 ] 設備清單選取項目")]
    [SerializeField] private ListItem_Device listItem;
    [SerializeField] private Data_ServerRackAsset data;

    [Header(">>> 點擊項目時Invoke")]
    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    [Header(">>> UI組件")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform deviceRUItemContainer;
    [SerializeField] private DeviceRUItem deviceRUItemPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<DeviceRUItem> itemList { get; set; } = new List<DeviceRUItem>();

    public void ShowRULayout(ListItem_Device target)
    {
        listItem = target;
        data = (Data_ServerRackAsset)listItem.data;

        //清空資料
        itemList.ForEach(item =>
        {
            item.OnClickItemEvent.RemoveAllListeners();
            item.SetToggleWithoutNotify(false);
        });
        itemList.Clear();
        ObjectPoolManager.PushToPool<DeviceRUItem>(deviceRUItemContainer);

        //建立資料
        data.containers.ForEach(deviceAsset =>
        {
            DeviceRUItem item = ObjectPoolManager.GetInstanceFromQueuePool<DeviceRUItem>(deviceRUItemPrefab, deviceRUItemContainer);
            item.toggleGroup = toggleGroup;
            item.ShowData(deviceAsset);
            item.OnClickItemEvent.AddListener(OnClickItemEvent.Invoke);
        });
        scrollRect.verticalNormalizedPosition = 1;


/*
        itemList.ForEach(item => item.OnClickItemEvent.RemoveAllListeners());
        ObjectPoolManager.PushToPool<DeviceRUItem>(deviceRUItemContainer);
        itemList.Clear();

        models.ForEach(model =>
        {
            DeviceRUItem item = ObjectPoolManager.GetInstanceFromQueuePool<DeviceRUItem>(deviceRUItemPrefab, deviceRUItemContainer);
            item.DeviceModel = model;
            item.toggleGroup = toggleGroup;
            item.OnClickItemEvent.AddListener(OnClickItemEvent.Invoke);
        });*/

    }
}