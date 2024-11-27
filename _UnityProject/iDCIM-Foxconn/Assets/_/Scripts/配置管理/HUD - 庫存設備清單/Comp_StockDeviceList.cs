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
    public UnityEvent<StockDeviceListItem, RackSpacer> onDeployDeviceModel = new UnityEvent<StockDeviceListItem, RackSpacer>();

    [Header(">>> [Prefab] - 列表項目")]
    [SerializeField] private StockDeviceListItem listItemPrefab;

    #region[組件]
    private ToggleGroup _toggleGroup { get; set; }
    private ToggleGroup toggleGroup => _toggleGroup ??= transform.GetComponent<ToggleGroup>();
    private ScrollRect _scrollRect { get; set; }
    private ScrollRect scrollRect => _scrollRect ??= transform.GetChild(2).Find("Container").GetChild(0).GetComponent<ScrollRect>();
    private TextMeshProUGUI _txtAmount { get; set; }
    private TextMeshProUGUI txtAmount => _txtAmount ??= transform.Find("txtAmount").GetComponent<TextMeshProUGUI>();
    #endregion

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
            item.onDeployDeviceModel.AddListener(onDeployDeviceModel.Invoke);
        });

        scrollRect.verticalNormalizedPosition = 1;
    }
}
