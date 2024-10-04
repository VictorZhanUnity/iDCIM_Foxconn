using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XCharts.Runtime;

public class MainDoorRecordPanel : MonoBehaviour
{
    [Header(">>> 資料項目集")]
    [SerializeField] private List<Data_AccessRecord> recordDataList;

    [Header(">>> 點擊項目時觸發")]
    public UnityEvent<ListItem_AccessRecord> onClickItemEvent = new UnityEvent<ListItem_AccessRecord>();

    [Header(">>> UI物件")]
    [SerializeField] private GameObject uiObj;
    [SerializeField] private LineChart lineChart;
    [SerializeField] private ListItem_AccessRecord listItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle defaultToggle;

    private List<ListItem_AccessRecord> listItems { get; set; } = new List<ListItem_AccessRecord>();

    public bool isOn
    {
        set
        {
            uiObj.SetActive(value);
        }
    }

    /// <summary>
    /// 設定Data
    /// </summary>
    public void ShowData(List<Dictionary<string, string>> dataList)
    {
        uiObj.SetActive(true);
        recordDataList.Clear();
        dataList.ForEach(data => recordDataList.Add(new Data_AccessRecord(data)));
        UpdateUI();
    }

    /// <summary>
    /// 更新圖表與表格
    /// </summary>
    private void UpdateUI()
    {
        // 更新圖表

        // 更新表格
        listItems.ForEach(item => item.onClickItemEvent.RemoveAllListeners());
        listItems.Clear();
        ObjectPoolManager.PushToPool<ListItem_AccessRecord>(scrollRect.content);

        recordDataList.ForEach(data =>
        {
            ListItem_AccessRecord listItem = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_AccessRecord>(listItemPrefab, scrollRect.content);
            listItem.recordData = data;
            listItem.toggleGroup = toggleGroup;
            listItem.onClickItemEvent.AddListener(onClickItemEvent.Invoke);
            listItems.Add(listItem);
        });

        scrollRect.verticalNormalizedPosition = 1;

        defaultToggle.isOn = true;
    }

    public void Hide()
    {
        uiObj.SetActive(false);
    }

    public void ToggleOff(Data_User userData) => defaultToggle.isOn = true;
}
