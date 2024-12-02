using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static DeviceConfigure_DataHandler;

public class Comp_StockDeviceList : MonoBehaviour
{
    [Header(">>> [資料項] - Data_iDCIMAsset")]
    [SerializeField] private List<StockDeviceSet> _data;

    [Header(">>> 點擊該資料項時Invoke")]
    public UnityEvent<StockDeviceListItem> onSelectDeviceModel = new UnityEvent<StockDeviceListItem>();

    [Header(">>> 當Drag產生暫時的設備時Invoke")]
    public UnityEvent<StockDeviceListItem> onCreateTempDeviceModel = new UnityEvent<StockDeviceListItem>();

    [Header(">>> [Prefab] - 列表項目")]
    [SerializeField] private StockDeviceListItem listItemPrefab;

    private List<StockDeviceListItem> listItems { get; set; } = new List<StockDeviceListItem>();

    /// <summary>
    /// 顯示資料
    /// </summary>
    public void ShowData(List<StockDeviceSet> data)
    {
        _data = data;
        txtAmount.SetText($"共{data.Count}台");

        ObjectPoolManager.PushToPool<StockDeviceListItem>(scrollRect.content);

        _data.ForEach(data =>
        {
            StockDeviceListItem item = ObjectPoolManager.GetInstanceFromQueuePool(listItemPrefab, scrollRect.content);
            item.ShowData(data);
            item.toggleGroup = toggleGroup;
            item.onSelectDeviceModel.AddListener(onSelectDeviceModel.Invoke);
            item.onCreateTempDeviceModel.AddListener(onCreateTempDeviceModel.Invoke);
            listItems.Add(item);
        });
        scrollRect.verticalNormalizedPosition = 1;
    }

    private void OnEnable()
    {
        listItems.ForEach(item => item.onCreateTempDeviceModel.AddListener(onCreateTempDeviceModel.Invoke));
        scrollRect.verticalNormalizedPosition = 1;
    }
    private void OnDisable() => listItems.ForEach(item => item.onCreateTempDeviceModel.RemoveAllListeners());

    public void UpdateList(StockDeviceListItem removeItem)
    {
        listItems.Remove(removeItem);
        txtAmount.SetText($"共{listItems.Count}台");
    }

    #region[>>> Componenets]
    private ToggleGroup _toggleGroup { get; set; }
    private ToggleGroup toggleGroup => _toggleGroup ??= transform.GetComponent<ToggleGroup>();
    private ScrollRect _scrollRect { get; set; }
    private ScrollRect scrollRect => _scrollRect ??= transform.GetChild(2).Find("Container").GetChild(0).GetComponent<ScrollRect>();
    private TextMeshProUGUI _txtAmount { get; set; }
    private TextMeshProUGUI txtAmount => _txtAmount ??= transform.Find("txtAmount").GetComponent<TextMeshProUGUI>();
    #endregion
}
