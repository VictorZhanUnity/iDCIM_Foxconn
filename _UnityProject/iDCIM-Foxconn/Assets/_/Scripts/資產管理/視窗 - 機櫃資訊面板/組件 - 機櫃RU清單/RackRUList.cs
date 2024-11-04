using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static VictorDev.RevitUtils.RevitHandler;

/// <summary>
/// ���d���]�wRU�M��
/// </summary>
public class RackRUList : MonoBehaviour
{
    [Header(">>> [��ƶ� ] �]�ƲM��������")]
    [SerializeField] private ListItem_Device listItem;
    [SerializeField] private Data_ServerRackAsset data;

    [Header(">>> �I�����خ�Invoke")]
    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    [Header(">>> UI�ե�")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform deviceRUItemContainer;
    [SerializeField] private DeviceRUItem deviceRUItemPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<DeviceRUItem> itemList { get; set; } = new List<DeviceRUItem>();

    public void ShowRULayout(ListItem_Device target)
    {
        listItem = target;
        data = (Data_ServerRackAsset)listItem.data;

        //�M�Ÿ��
        itemList.ForEach(item =>
        {
            item.OnClickItemEvent.RemoveAllListeners();
            item.SetToggleWithoutNotify(false);
        });
        itemList.Clear();
        ObjectPoolManager.PushToPool<DeviceRUItem>(deviceRUItemContainer);

        //�إ߸��
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