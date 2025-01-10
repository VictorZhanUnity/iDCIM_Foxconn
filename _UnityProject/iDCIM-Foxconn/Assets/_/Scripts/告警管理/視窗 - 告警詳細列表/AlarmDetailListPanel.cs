using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using static AlarmHistoryDataManager;

public class AlarmDetailListPanel : MonoBehaviour
{
     private ListItem_AlarmDetail ListItemPrefab => transform.Find("Prefab_ListItem_AlarmDetail").GetComponent<ListItem_AlarmDetail>();
     private ListItem_AlarmDetail _listItemPrefab;

    public void ReceiveAlarmDataList(List<Data_AlarmHistoryData> data, string title)
    {
        alarmHistoryDataList = data;
        TxtTitle.SetText(title.Trim());
        UpdateUI();
    }

    private void UpdateUI()
    {
        int amountOfAlarms = alarmHistoryDataList.SelectMany(data => data.alarms).Count();
        gameObject.SetActive(amountOfAlarms>0);

        foreach (Transform child in ScrollRectInstance.content)
        {
            Destroy(child.gameObject);  
        }
        
        alarmHistoryDataList.SelectMany(data => data.alarms.Select(alarm=> new {historyData=data, alarmData = alarm}))
            .OrderByDescending(dataSet=>dataSet.alarmData.AlarmTime).ToList().ForEach(dataSet =>
        {
            ListItem_AlarmDetail item = Instantiate(ListItemPrefab, ScrollRectInstance.content);
            item.ReceiveAlaramData(dataSet.alarmData, dataSet.historyData.tagName);
        });
        DotweenHandler.DoInt(TxtAmount, 0, amountOfAlarms);
    }

    private void OnDisable() => Destroy(gameObject);

    #region [Components]

    [Header("[資料項] - 告警歷史記錄清單")] private List<Data_AlarmHistoryData> alarmHistoryDataList;

    private Transform Container => _container ??= transform.Find("Panel").Find("Container");
    private Transform _container;

    private ScrollRect ScrollRectInstance =>
        _scrollRect ??= Container.Find("Table").Find("ScrollRect").GetComponent<ScrollRect>();

    private ScrollRect _scrollRect;
    private TextMeshProUGUI TxtTitle => _txtTitle ??= Container.Find("txtTitle").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtTitle;
    private TextMeshProUGUI TxtAmount => _txtAmount ??= Container.Find("txtAmount").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _txtAmount;

    #endregion
}
