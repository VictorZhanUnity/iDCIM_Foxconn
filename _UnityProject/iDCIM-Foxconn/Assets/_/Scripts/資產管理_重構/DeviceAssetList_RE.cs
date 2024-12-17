using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 資產設備列表
/// </summary>
public class DeviceAssetList_RE : MonoBehaviour
{
    [Header(">>> 點擊該資料項時Invoke")]
    public UnityEvent<ListItem_Device_RE> onClickItemEvent = new UnityEvent<ListItem_Device_RE>();

    [Header(">>> [Prefab] 列表項目")]
    [SerializeField] private ListItem_Device_RE ltemPrefab;

    private List<Data_iDCIMAsset> dataList { get; set; }
    /// <summary>
    /// 過濾篩選後的資料
    /// </summary>
    private List<Data_iDCIMAsset> filterDataList { get; set; }
    private List<ListItem_Device_RE> itemList { get; set; } = new List<ListItem_Device_RE>();

    public void ReceiveData(List<Data_iDCIMAsset> data)
    {
        dataList = data;

        //過濾資料...

        filterDataList = dataList;

        //依設備名稱排序
        filterDataList = filterDataList.OrderBy(data => data.deviceName).ToList();

        //清除先前項目
        itemList.ForEach(item =>
        {
            item.isOn = false;
            item.onClickItemEvent.RemoveAllListeners();
        });
        itemList.Clear();
        ObjectPoolManager.PushToPool<ListItem_Device_RE>(scrollRect.content);

        //建立項目
        filterDataList.ForEach(data =>
        {
            ListItem_Device_RE item = ObjectPoolManager.GetInstanceFromQueuePool(ltemPrefab, scrollRect.content);
            item.toggleGroup = toggleGroup;
            item.ShowData(data);
            item.onClickItemEvent.AddListener(onClickItemEvent.Invoke);
            itemList.Add(item);
        });

        scrollRect.verticalNormalizedPosition = 1;
    }

    /// <summary>
    /// 搜尋設備名稱
    /// </summary>
    public void FilterByDeviceName(string keyword)
    {
        itemList.ForEach((item) =>
        {
            if (string.IsNullOrEmpty(keyword)) item.gameObject.SetActive(true);
            else item.gameObject.SetActive(item.data.deviceName.Contains(keyword));
        });
        scrollRect.verticalNormalizedPosition = 1;
    }

    #region [Components]
    private ScrollRect _scrollRect { get; set; }
    private ScrollRect scrollRect => _scrollRect ??= transform.Find("Container").GetChild(1).GetComponent<ScrollRect>();

    private ToggleGroup _toggleGroup { get; set; }
    private ToggleGroup toggleGroup => _toggleGroup ??= transform.GetComponent<ToggleGroup>();
    #endregion
}
