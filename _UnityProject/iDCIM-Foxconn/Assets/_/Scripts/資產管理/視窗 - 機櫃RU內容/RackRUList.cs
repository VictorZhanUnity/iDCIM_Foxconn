using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RackRUList : MonoBehaviour
{
    [SerializeField] private Transform deviceRUItemContainer;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private DeviceRUItem deviceRUItemPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    public UnityEvent<DeviceRUItem> OnClickItemEvent = new UnityEvent<DeviceRUItem>();

    private List<DeviceRUItem> itemList { get; set; } = new List<DeviceRUItem>();

    public void ShowRULayout(List<Transform> models)
    {
        itemList.ForEach(item => item.OnClickItemEvent.RemoveAllListeners());
        ObjectPoolManager.PushToPool<DeviceRUItem>(deviceRUItemContainer);
        itemList.Clear();

        models.ForEach(model =>
        {
            DeviceRUItem item = ObjectPoolManager.GetInstanceFromQueuePool<DeviceRUItem>(deviceRUItemPrefab, deviceRUItemContainer);
            item.DeviceModel = model;
            item.toggleGroup = toggleGroup;
            item.OnClickItemEvent.AddListener(OnClickItemEvent.Invoke);
        });

        scrollRect.verticalNormalizedPosition = 1;
    }
}