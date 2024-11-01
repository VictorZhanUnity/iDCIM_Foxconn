using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RackList : MonoBehaviour
{
    [Header(">>> 點擊時Invoke")]
    public UnityEvent<ListItem_Rack> onClickItemEvent = new UnityEvent<ListItem_Rack>();

    [Header(">>> UI組件")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ListItem_Rack listItemPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<ListItem_Rack> listItems { get; set; } = new List<ListItem_Rack>();

    public List<Transform> rackModels
    {
        set
        {
            RemoveListItems();

            value.ForEach(target =>
            {
                ListItem_Rack listItem = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_Rack>(listItemPrefab, scrollRect.content);
                listItem.rackModel = target;
                listItem.toggleGroup = toggleGroup;
                listItem.onClickItemEvent.AddListener(onClickItemEvent.Invoke);
                listItems.Add(listItem);
            });
        }
    }

    /// <summary>
    /// 移除列表項目
    /// </summary>
    private void RemoveListItems()
    {
        listItems.ForEach(listItem => listItem.onClickItemEvent.RemoveAllListeners());
        listItems.Clear();
        ObjectPoolManager.PushToPool<ListItem_Rack>(scrollRect.content);
    }
}