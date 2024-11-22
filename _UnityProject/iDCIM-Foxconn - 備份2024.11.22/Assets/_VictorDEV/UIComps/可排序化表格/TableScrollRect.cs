using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class TableScrollRect<DATA, ROW_PREFAB> : MonoBehaviour where ROW_PREFAB : TableRow<DATA>
{
    [Header(">>> [資料項]")] private List<DATA> _dataList;

    [Header(">>> [Event] - 點擊項目時Invoke")]
    public UnityEvent<DATA> onClickItemEvent = new UnityEvent<DATA>();

    [Header(">>> [組件] - ScrollRect選單")]
    [SerializeField] private TableHeaderColumn columnPrefab;
    [SerializeField] protected ROW_PREFAB rowPrefab;

    [Header(">>> 欄位標題設定")]
    [SerializeField] protected List<ColumnSet> headers = new List<ColumnSet>();

    private Transform _header { get; set; }
    private Transform header => _header ??= transform.GetChild(0).Find("Header");
    private ScrollRect _scrollRect { get; set; }
    protected ScrollRect scrollRect => _scrollRect ??= transform.GetChild(0).Find("Content").GetChild(0).GetComponent<ScrollRect>();
    private ToggleGroup _headerGroup { get; set; }
    private ToggleGroup headerGroup => _headerGroup ??= header.GetComponent<ToggleGroup>();
    private ToggleGroup _rowGroup { get; set; }
    private ToggleGroup rowGroup => _rowGroup ??= scrollRect.GetComponent<ToggleGroup>();

    protected List<TableRow<DATA>> rowList { get; set; } = new List<TableRow<DATA>>();

    private void Start()
    {
        SetHeaders(headers);
    }

    /// <summary>
    /// 設置標題與欄寬 Tuple{標題, 欄寬}
    /// </summary>
    public void SetHeaders(List<ColumnSet> headers)
    {
        foreach (Transform child in header)
        {
            if (child.TryGetComponent<TableHeaderColumn>(out TableHeaderColumn column))
            {
                Destroy(child.gameObject);
            }
        }
        headers.ForEach(columnSet =>
        {
            TableHeaderColumn column = Instantiate(columnPrefab, header);
            column.label = columnSet.label;
            column.width = columnSet.width;
            column.toggleGroup = headerGroup;
            column.onClickEvent.AddListener(onClickHeaderSortHandler);
        });
    }

    /// <summary>
    /// 設置資料
    /// </summary>
    public void ShowData(List<DATA> data)
    {
        ClearContainer();
        scrollRect.verticalNormalizedPosition = 1;
        _dataList = data;
        _dataList.ForEach(data =>
        {
            TableRow<DATA> item = ObjectPoolManager.GetInstanceFromQueuePool(rowPrefab, scrollRect.content);
            item.toggleGroup = rowGroup;
            item.SetData(data);
            item.onClickItem.AddListener((data) =>
            {
                onClickItemEvent.Invoke(data);
                onClickItemEventHandler(data);
            });
            rowList.Add(item);
        });
        OnShowDataHandler(data);
    }
    protected abstract void OnShowDataHandler(List<DATA> data);

    public void AddRowDataItem(TableRow<DATA> item)
    {
        item.transform.parent = scrollRect.content;
    }
    public void ClearContainer()
    {
        ObjectPoolManager.PushToPool<AccessRecord_TableRow>(scrollRect.content.transform);
        rowList.ForEach(row => row.onClickItem.RemoveAllListeners());
    }

    /// <summary>
    /// 點擊標題時排序
    /// </summary>
    protected abstract void onClickHeaderSortHandler(bool isDesc, string label);
    /// <summary>
    /// 點擊項目時
    /// </summary>
    protected abstract void onClickItemEventHandler(DATA data);

    [Serializable]
    public class ColumnSet
    {
        public string label;
        public float width;
    }
}

