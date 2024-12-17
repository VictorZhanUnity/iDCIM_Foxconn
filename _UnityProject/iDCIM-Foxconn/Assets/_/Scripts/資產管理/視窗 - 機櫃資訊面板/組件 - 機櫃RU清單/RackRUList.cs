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
    [Header(">>> �I�����خ�Invoke")]
    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    [Header(">>> UI�ե�")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform deviceRUItemContainer;
    [SerializeField] private DeviceRUItem deviceRUItemPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<DeviceRUItem> itemList { get; set; } = new List<DeviceRUItem>();

    /// <summary>
    /// �겣�C��ثe�ҿ諸����
    /// </summary>
    private ListItem_Device_RE selectedListItem { get; set; }
    /// <summary>
    /// �겣�C��ثe�ҿ諸���d���
    /// </summary>
    private Data_ServerRackAsset dataRack { get; set; }

    public void ShowRULayout(ListItem_Device_RE target)
    {
        selectedListItem = target;
        dataRack = (Data_ServerRackAsset)selectedListItem.data;

        //�M�Ÿ�� 
        itemList.ForEach(item =>
        {
            item.isOn = false;
            item.OnClickItemEvent.RemoveAllListeners();
        });
        itemList.Clear();
        ObjectPoolManager.PushToPool<DeviceRUItem>(deviceRUItemContainer);

        //�إ߸��
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