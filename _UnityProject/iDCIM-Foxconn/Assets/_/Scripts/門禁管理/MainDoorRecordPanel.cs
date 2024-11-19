using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XCharts.Runtime;

public class MainDoorRecordPanel : MonoBehaviour
{
    [Header(">>> ��ƶ��ض�")]
    [SerializeField] private List<Data_AccessRecord_OLD> recordDataList;

    [Header(">>> �I�����خ�Ĳ�o")]
    public UnityEvent<ListItem_AccessRecord_OLD> onClickItemEvent = new UnityEvent<ListItem_AccessRecord_OLD>();

    [Header(">>> UI����")]
    [SerializeField] private GameObject uiObj;
    [SerializeField] private LineChart lineChart;
    [SerializeField] private ListItem_AccessRecord_OLD listItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle defaultToggle;

    private List<ListItem_AccessRecord_OLD> listItems { get; set; } = new List<ListItem_AccessRecord_OLD>();

    public bool isOn
    {
        set
        {
            uiObj.SetActive(value);
        }
    }

    /// <summary>
    /// �]�wData
    /// </summary>
    public void ShowData(List<Dictionary<string, string>> dataList)
    {
        uiObj.SetActive(true);
        recordDataList.Clear();
        dataList.ForEach(data => recordDataList.Add(new Data_AccessRecord_OLD(data)));

        //�̶i�X�ɶ��Ƨ�
        TableSortKeyHandler("AccessTimeStamp", true);
    }

    /// <summary>
    /// ��s�Ϫ�P���
    /// </summary>
    private void UpdateUI()
    {
        // ��s�Ϫ�

        // ��s���
        listItems.ForEach(item => item.onClickItemEvent.RemoveAllListeners());
        listItems.Clear();
        ObjectPoolManager.PushToPool<ListItem_AccessRecord_OLD>(scrollRect.content);

        recordDataList.ForEach(data =>
        {
            ListItem_AccessRecord_OLD listItem = ObjectPoolManager.GetInstanceFromQueuePool<ListItem_AccessRecord_OLD>(listItemPrefab, scrollRect.content);
            listItem.recordData = data;
            listItem.toggleGroup = toggleGroup;
            listItem.onClickItemEvent.AddListener(onClickItemEvent.Invoke);
            listItems.Add(listItem);
        });

        scrollRect.verticalNormalizedPosition = 1;

        defaultToggle.isOn = true;
    }

    /// <summary>
    /// ������Ƨ�
    /// </summary>
    public void TableSortKeyHandler(string keyName, bool isDesc)
    {
        recordDataList = (isDesc) ? recordDataList.OrderByDescending(record => record.GetValue(keyName)).ToList()
            : recordDataList.OrderBy(record => record.GetValue(keyName)).ToList();
        UpdateUI();
    }


    public void ToggleOff(Data_User userData) => defaultToggle.isOn = true;
}
