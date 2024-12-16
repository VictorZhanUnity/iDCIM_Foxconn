using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeviceAssetList_RE : MonoBehaviour
{
    [Header(">>> 點擊該資料項時Invoke")]
    public UnityEvent<ListItem_Device_RE> onClickItemEvent = new UnityEvent<ListItem_Device_RE>();

    [Header(">>> 列表")]
    [SerializeField] private ScrollRect scrollRect;

    [Header(">>> [Prefab] 列表項目")]
    [SerializeField] private ListItem_Device_RE ltemPrefab;

    private List<ListItem_Device_RE> ltemList { get; set; } = new List<ListItem_Device_RE>();

    public void ReceiveData(List<Data_ServerRackAsset> datas)
    {

    }
    public void ReceiveData(List<Data_DeviceAsset> datas)
    {
        ltemList.ForEach(data => data.onClickItemEvent.RemoveAllListeners());
        ltemList.Clear();
        ObjectPoolManager.PushToPool<ListItem_Device_RE>(scrollRect.content);

        datas.ForEach(data =>
        {
            ListItem_Device_RE item = ObjectPoolManager.GetInstanceFromQueuePool(ltemPrefab, scrollRect.content);
            item.ShowData(data);
            item.onClickItemEvent.AddListener(onClickItemEvent.Invoke);
        });

        scrollRect.verticalNormalizedPosition = 1;
    }
}
